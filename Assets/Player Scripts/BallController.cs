using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour // Used by the player to create and launch balls. Also handles player input.
{
    private GameObject ball;
    public GameObject ballTemplate; // Defined in the Unity editor: is instanced in order to create new balls
    private BallBehaviour launcher;
    public int state = 0; // 0 when the angle is being selected, 1 while the power is being selected, 2 after power is selected, 3 after the ball has been launched
    [Range(0, 359)]

    private int incrementDirection = 1; // Used by IncrementAngle and IncrementForce to change direction.

    public float MIN_ANGLE;
    public float MAX_ANGLE;
    public float launchAngle;
    public Quaternion launchDirection;
    
    public float MIN_FORCE;
    public float MAX_FORCE;
    public float force;

    // Start is called before the first frame update
    void Start()
    {
        ResetValues(); // Technically it's setting some of these for the first time 
        CreateBall();
    }

    // Resets important variables prior to creating a new ball
    void ResetValues()
    {
        state = 0;
        launchAngle = 0f;
        force = 0f;

        MIN_ANGLE = 0f;
        MAX_ANGLE = 90f;
        MIN_FORCE = 1f;
        MAX_FORCE = 210f;
    }

    // Creates a new ball to be launched.
    public GameObject CreateBall()
    {
        GameObject newBall = Object.Instantiate(ballTemplate); // Temp variable
        Bind(newBall); // Makes sure the script actually uses the new ball
        ResetValues();
        return newBall;
    }

    // Sets all variables which call on the ball object to use the newly created ball
    void Bind(GameObject boundBall)
    {
        ball = boundBall;
        // ball.SetActive(true); // this line is redundant
        launcher = ball.GetComponent<BallBehaviour>();
        launcher.player = this; // feeds this object to the ball
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
        if (state < 2 && Input.GetKeyDown("space")) // select angle and force
        {
            state++;
        }

        if (state == 2 && Input.GetKeyDown("space")) // actually launch the ball
        {
            launcher.Launch(launchDirection, force); // See BallBehavior.cs for how this works
            state++; // Enter state 3. In state 3, nothing can happen until CreateBall() is called.
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