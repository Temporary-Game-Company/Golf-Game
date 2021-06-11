using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour // Defines useful behaviours for the ball.
{
    private Rigidbody2D rb2D;

    public int bounceCount;
    private int MAX_BOUNCES = 3;

    public BallController player; // BallController passes itself to this object.

    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent(typeof(Rigidbody2D)) as Rigidbody2D;
        bounceCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        bounceCount++;

        if (bounceCount >= MAX_BOUNCES)
        {
            player.CreateBall(); // TEMPORARY: immediately creates a new ball when the current one is destroyed. Method found in BallController.cs
            Object.Destroy(gameObject); // Banishes this ball instance
        }
    }

    public void Launch(Quaternion direction, float force) // Launches the ball. Normally called from BallController.cs by the player game object.
    {
        rb2D.transform.rotation = direction; // Points the ball towards the launch direction.
        rb2D.WakeUp(); // Turns on physics. Might be redundant?
        rb2D.AddForce(transform.right * force); // Launches the ball in the direction it is facing.
    }
}
