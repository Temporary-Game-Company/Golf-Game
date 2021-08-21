using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallController : MonoBehaviour // Used by the player to create and launch balls. Also handles player input.
{
    public GameObject ball;
    public GameObject ballTemplate; // Defined in the Unity editor: is instanced in order to create new balls
    public BallBehaviour launcher;
    [SerializeField] private GameObject bar; 
    [SerializeField] private GameObject arrow;

    public int state = 0; // 0 when the angle is being selected, 1 while the power is being selected, 2 after power is selected, 3 after the ball has been launched

    private int incrementDirection = 1; // Used by IncrementAngle and IncrementForce to change direction.

    public bool ballExists;

    public float MIN_ANGLE;
    public float MAX_ANGLE;
    [Range(0, 359)] public float launchAngle;
    public Quaternion launchDirection;
    
    public float MIN_FORCE;
    public float MAX_FORCE;
    public float FORCE_INCREMENT;
    public float force;

    // used for projecting the path of the ball
    bool projectionActive = false;
    Scene simScene;
    private PhysicsScene2D physicsSim;
    [SerializeField] private GameObject simulatedBall;
    [SerializeField] LineRenderer projectedPath;
    private int simSteps = 90;
    private Vector3[] pathPoints;
    private string[] collidableTags = {"Terrain", "Enemy", "SpecialTerrain"}; // An array of tags. All objects with these tags will be simulated when projecting the path the ball will take.
    //private Type[] typesToDisable;
    private List<GameObject> collidableObjects; // Objects to be simulated when projecting the path of the ball

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(typeof(string).GetType());
        
        ResetValues(); // Technically it's setting some of these for the first time
    }

    // Resets important variables prior to creating a new ball
    public void ResetValues()
    {
        MIN_ANGLE = 0f;
        MAX_ANGLE = 90f;
        MIN_FORCE = 30f;
        MAX_FORCE = 210f;
        FORCE_INCREMENT = (MAX_FORCE - MIN_FORCE) / 110;

        state = 0;
        launchAngle = 0f;
        force = MIN_FORCE;

        arrow.SetActive(false);
        bar.SetActive(false);

        ballExists = false;
    }

    // Creates a new ball to be launched.
    public void CreateBall()
    {
        ballExists = true;
        GameObject newBall = Instantiate(ballTemplate); // Temp variable
        Bind(newBall); // Makes sure the script actually uses the new ball

        arrow.SetActive(true);
    }

    // Sets all variables which call on the ball object to use the newly created ball
    void Bind(GameObject boundBall)
    {
        ball = boundBall;
        launcher = ball.GetComponent<BallBehaviour>();
    }

    // Fixed update is called at a fixed rate
    void FixedUpdate()
    {
        if (state == 0) // Angle is being selected
        {
            IncrementAngle();
        }

        if (state == 1) // Power is being selected
        {
            IncrementForce();
        }

        if (projectionActive)
        {
            if (simulatedBall != null)
            {
                BallBehaviour simBallBehaviour = simulatedBall.GetComponent<BallBehaviour>();
                simBallBehaviour.damage = 0d;
                simBallBehaviour.Launch(launchDirection, force);
            
                for (int i = 0; i < simSteps; i++)
                {
                    pathPoints[i] = simulatedBall.transform.position;
                    projectedPath.SetPosition(i, pathPoints[i]);
                    physicsSim.Simulate(Time.fixedDeltaTime);                
                }

                Destroy(simulatedBall);
            }
            simulatedBall = Instantiate(ballTemplate);
            SceneManager.MoveGameObjectToScene(simulatedBall, simScene);
        }
    }

    // Update is called once per frame
    void Update()
    {
        launchDirection = Quaternion.AngleAxis(launchAngle, Vector3.forward).normalized;

        // used to get player input
        if (Input.GetKeyDown("space") && ballExists)
        {
            if (state < 2) // select angle and force
            {
                state++;
                if (state == 1) // Power State
                {
                    bar.SetActive(true);
                    // StartProjection();
                    return;
                }
                else // Launch State
                {
                    arrow.SetActive(false);
                    bar.SetActive(false);
                }
            }

            if (state == 2 && !launcher.launched) // actually launch the ball
            {
                launcher.Launch(launchDirection, force); // See BallBehavior.cs for how this works
                StopProjection();
            }
        }
        
    }

    // Makes the angle bounce back and forth. For the arrow's behaviour see ArrowAngle.cs
    void IncrementAngle()
    {
        if (launchAngle <= MIN_ANGLE)
        {
            incrementDirection = 1;
        }
        if (launchAngle >= MAX_ANGLE)
        {
            incrementDirection = -1;
        }
        launchAngle += 3 * incrementDirection;
    }

    // Makes the force bounce back and forth. For the meter's behaviour see PowerBar.cs
    void IncrementForce()
    {
        if (force <= MIN_FORCE)
        {
            incrementDirection = 1;
        }

        if (force >= MAX_FORCE)
        {
            incrementDirection = -1;
        }

        force += FORCE_INCREMENT * incrementDirection;
    }

    void StartProjection()
    {   
        projectionActive = true;
        // Creates scene used for ball path projection
        CreateSceneParameters param = new CreateSceneParameters(LocalPhysicsMode.Physics2D); // initializes a new set of physics parameters for a scene based on our current scene's physics
        simScene = SceneManager.CreateScene("ProjectionSimulation", param); // creates a scene using param variable. Used to simulate the path of the ball for projection
        physicsSim = simScene.GetPhysicsScene2D(); // gets the physics of the new scene
        projectedPath.positionCount = simSteps; // sets the number of points the projection line should have
        pathPoints = new Vector3[simSteps]; // initializes an array of points for the projection line to use later

        foreach (string tag in collidableTags) // Goes through each tag in collideable tags, finds every object with the corresponding tags, sends an instance of these to the sim scene.
        {
            foreach (GameObject collidableObject in GameObject.FindGameObjectsWithTag(tag))
            {
                GameObject simObject = GameObject.Instantiate(collidableObject, collidableObject.transform.position, collidableObject.transform.rotation);
                SceneManager.MoveGameObjectToScene(simObject, simScene);

                // time to disable components which are rendered so there aren't two copies of everything
                // awful terrible code because I couldn't get a generic method or the type namespace to work for some reason
                // Type dummy = new Type();
                SpriteRenderer[] spritesToDisable = simObject.GetComponents<SpriteRenderer>();
                foreach (SpriteRenderer spriteToDisable in spritesToDisable)
                {
                    spriteToDisable.enabled = false;
                }

                ParticleSystem[] particlesToDisable = simObject.GetComponents<ParticleSystem>();
                foreach (ParticleSystem systemToDisaple in particlesToDisable)
                {
                    systemToDisaple.Stop();
                }


            }
        }
        projectedPath.gameObject.SetActive(true);
    }

    void StopProjection()
    {
        projectionActive = false;
        SceneManager.UnloadSceneAsync(simScene);
        projectedPath.gameObject.SetActive(false);
    }
}