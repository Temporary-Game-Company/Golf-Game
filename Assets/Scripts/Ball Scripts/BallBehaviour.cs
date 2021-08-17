using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;


public class BallBehaviour : MonoBehaviour // Defines useful behaviours for the ball.
{
    public double damage = 50d;
    private Rigidbody2D rb2D;
    public Boolean launched;
    public int bounceCount;
    public double comboCount;

    private BallController ballController; // BallController passes itself to this object.
    private BattleManager battleManager;

    private const float poofTime = 0.6f;
    private float slowTimer = poofTime;
    private Animator animator;
    public bool destroyed;

    private TrailRenderer trail;
    private Material trailMat;
    private Color TempColor;
    private GradientAlphaKey[] tempAlphaGradient;
    private Gradient tempGradient;
    private float alphaCoefficient;

    // debugInfo is used only to send data to the debug overlay
    private DebugDisplay debugInfo;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent(typeof(Rigidbody2D)) as Rigidbody2D;

        ballController = GameObject.Find("BattleManager").GetComponent<BallController>();

        animator = GetComponent<Animator>();

        trail = GetComponent<TrailRenderer>();
        trailMat = trail.material;

        // debugInfo is used only to send data to the debug overlay
        debugInfo = GameObject.Find("Debug Overlay").GetComponent<DebugDisplay>();

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
                Debug.Log("Poof");
                Poof();
            }
        }
        else
        {
            slowTimer = poofTime;
        }

        // Updates trail opacity and length
        TempColor = trailMat.color;
        TempColor.a = (float) (0.7 * (1 - Math.Pow(1.2, -rb2D.velocity.magnitude + 4) + 0.05));
        trailMat.color = TempColor;
    
        tempGradient = new Gradient();
        tempGradient.SetKeys(
            trail.colorGradient.colorKeys,
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, (float) (rb2D.velocity.magnitude/(2*Math.Sqrt(1 + (rb2D.velocity.magnitude-6) * (rb2D.velocity.magnitude-6)))+0.5))}
        );
        
        trail.colorGradient = tempGradient;

        // Updates debug info
        //debugInfo.UpdateDebug("Ball Speed", rb2D.velocity.magnitude.ToString());
        //debugInfo.UpdateDebug("Ball Velocity", rb2D.velocity.ToString());
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
            collision.collider.gameObject.GetComponent<EnemyBehaviour>().TakeDamage((int) (damage * (Math.Pow(1.42f, comboCount))));
            comboCount += 1;
        }

        else if (comboCount > 0)
        {
            comboCount -= 1;
        }
        
        if (bounceCount >= 6)
        {
            Poof();
        }

        UpdateTrail();
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
        await Task.Delay(500);
        rb2D.drag = 15;
        while (animator.GetBool("Poofing"))
        {
            await Task.Delay(25);
        }
    }

    private void UpdateTrail() // Updates the color of the ball's trail to reflect the current combo
    {
        switch(comboCount)
        {
            case 0: TempColor = new Color (1f, 1f, 1f); break;
            case 1: TempColor = new Color (1f, 0.9f, 0.36f);break;
            case 2: TempColor = new Color (1f, 0.56f, 1f); break;
            default: TempColor = new Color (1f, 1f, 1f); break;
        }
        
        tempGradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(TempColor, 0f), new GradientColorKey(TempColor, 1f)},
            trail.colorGradient.alphaKeys
        );
        trail.colorGradient = tempGradient;
    }
}   
