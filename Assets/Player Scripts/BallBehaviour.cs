using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class BallBehaviour : MonoBehaviour // Defines useful behaviours for the ball.
{
    private Rigidbody2D rb2D;

    private Boolean launched = false;

    public int bounceCount;

    private static float timer = 1f;

    public float slowTimer = timer;

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
        slowTimer -= Time.deltaTime;

        if (rb2D.velocity.magnitude <= 2f && launched == true)
        {
            if (slowTimer <= 0)
            {
                Destroy(gameObject); // Banishes this ball instance
                player.ResetValues();
            }
        }
        else
        {
            slowTimer = timer;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.gameObject.tag == "Enemy")
        {
            collision.collider.gameObject.GetComponent<EnemyBehaviour>().health -= 50;
        }
        else if (collision.collider.gameObject.name == "Outer Box")
        {
            Destroy(gameObject); // Banishes this ball instance
            player.ResetValues();
        }
    }

    public async void Launch(Quaternion direction, float force) // Launches the ball. Normally called from BallController.cs by the player game object.
    {
        rb2D.transform.rotation = direction; // Points the ball towards the launch direction.
        rb2D.WakeUp(); // Turns on physics. Might be redundant?
        rb2D.AddForce(transform.right * force); // Launches the ball in the direction it is facing.

        await System.Threading.Tasks.Task.Delay(1000);
        launched = true;
    }
}   
