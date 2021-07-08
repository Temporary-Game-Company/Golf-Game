using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;


public class BallBehaviour : MonoBehaviour // Defines useful behaviours for the ball.
{
    private Rigidbody2D rb2D;

    public Boolean launched;

    private int bounceCount;

    private const float poofTime = 0.6f;

    private float slowTimer = poofTime;

    private BallController ballController; // BallController passes itself to this object.

    private BattleManager battleManager;

    private Animator animator;

    public bool destroyed;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent(typeof(Rigidbody2D)) as Rigidbody2D;

        ballController = GameObject.Find("BattleManager").GetComponent<BallController>();

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (destroyed)
        {
            return;
        }
        
        slowTimer -= Time.deltaTime;

        if (rb2D.velocity.magnitude <= 2f && launched)
        {
            if (slowTimer <= 0)
            {
                Poof();
            }
        }
        else
        {
            slowTimer = poofTime;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        bounceCount += 1;
        if (collision.collider.gameObject.name == "Outer Box")
        {
            DestroyBall();
        }
        else if (collision.collider.gameObject.tag == "Enemy")
        {
            collision.collider.gameObject.GetComponent<EnemyBehaviour>().TakeDamage(50);
        }
        
        if (bounceCount >= 6)
        {
            Poof();
        }
    }

    public async void Launch(Quaternion direction, float force) // Launches the ball. Normally called from BallController.cs by the player game object.
    {
        launched = true;
        rb2D.transform.rotation = direction; // Points the ball towards the launch direction.
        rb2D.WakeUp(); // Turns on physics. Might be redundant?
        rb2D.AddForce(transform.right * force); // Launches the ball in the direction it is facing.

        await Task.Delay(1000);
    }

    private async void Poof()
    {
        destroyed = true;

        
        animator.SetBool("Poofing", true);

        await WaitForAnimation();
        
        DestroyBall();
    }

    private void DestroyBall()
    {
        Destroy(gameObject); // Banishes this ball instance
        ballController.ResetValues();
    }

    private async Task WaitForAnimation() // Task which waits for animation to end.
    {
        await Task.Delay(300);
        rb2D.drag = 15;
        while (animator.GetBool("Poofing"))
        {
            await Task.Delay(25);
        }
    }
}   
