using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour // Used by the player to create and launch balls. Also handles player input.
{
    private GameObject ball;
    public GameObject ballTemplate; // Defined in the Unity editor: is instanced in order to create new balls
    private BallBehaviour launcher;
    private GameObject bar;
    private GameObject ui;
    private GameObject arrow;

    public int state = 0; // 0 when the angle is being selected, 1 while the power is being selected, 2 after power is selected, 3 after the ball has been launched
    [Range(0, 359)]

    private int incrementDirection = 1; // Used by IncrementAngle and IncrementForce to change direction.

    public bool ballExists;

    public float MIN_ANGLE;
    public float MAX_ANGLE;
    public float launchAngle;
    public Quaternion launchDirection;
    
    private float MIN_FORCE;
    public float MAX_FORCE;
    public float force;

    // Start is called before the first frame update
    void Start()
    {
        bar = GameObject.Find("Power Bar");

        arrow = GameObject.Find("Arrow");
        
        ResetValues(); // Technically it's setting some of these for the first time
    }

    // Resets important variables prior to creating a new ball
    public void ResetValues()
    {
        state = 0;
        launchAngle = 0f;
        force = 0f;

        MIN_ANGLE = 0f;
        MAX_ANGLE = 90f;
        MIN_FORCE = 1f;
        MAX_FORCE = 210f;

        arrow.SetActive(false);
        bar.SetActive(false);

        ballExists = false;
    }

    // Creates a new ball to be launched.
    public void CreateBall()
    {
        ballExists = true;
        GameObject newBall = Object.Instantiate(ballTemplate); // Temp variable
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

        force += 5 * incrementDirection;

    }
}