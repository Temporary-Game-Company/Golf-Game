using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Threading.Tasks;
using System;

public class EnemyBehaviour : MonoBehaviour
{
    public int health;
    public int speed;
    public int attack;

    public Sprite idle;
    public Sprite chargingAttack;
    public Sprite hitting;

    private SpriteRenderer spriteRenderer;

    private Vector3 originalPosition;

    [SerializeField] private GameObject player;
    private PlayerBehaviour playerScript;

    private Vector3 target;
    private float approachDistance = 4f;

    private Stopwatch clock;

    public bool approach;
    public bool returning;
    public bool attacking;

    private bool blocked;

    // debugInfo is used only to send data to the debug overlay
    private DebugDisplay debugInfo;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        playerScript = player.GetComponent<PlayerBehaviour>();
        target = player.transform.position + new Vector3(approachDistance, 0, 0);

        // debugInfo is used only to send data to the debug overlay
        //debugInfo = GameObject.Find("Debug Overlay").GetComponent<DebugDisplay>();
    }

    // Update is called once per frame
    void Update()
    {
        if (approach)
        {
            Approach();
        }

        if (returning)
        {
            Return();
        }

        // Updates debug info
        //debugInfo.UpdateDebug(gameObject.name + "health", health.ToString());
    }

    public async void Approach()
    {
        approach = true;
        attacking = true;

        float step = (20 + speed) * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);

        if (Vector3.Distance(transform.position, target) <= 0f)
        {
            approach = false;

            // ATTACK HAPPENS HERE!

            await AttackPlayer();

            Return();
        }
    }

    private async Task AttackPlayer()
    {
        clock = Stopwatch.StartNew();
        bool blocked = await awaitAction();

        if (blocked)
        {
            playerScript.TakeDamage(attack / 2);
        }
        else
        {
            playerScript.TakeDamage(attack);
        }
    }

    private void Return()
    {
        returning = true;

        float step = (20 + speed) * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, originalPosition, step);

        if (Vector3.Distance(transform.position, originalPosition) <= 0f)
        {
            returning = false;
            attacking = false;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private async Task<bool> awaitAction()
    {
        playerScript.action = false;
        while (!playerScript.action && clock.Elapsed.TotalMilliseconds <= 700)
        {
            spriteRenderer.sprite = chargingAttack;
            await Task.Delay(25);
        }
        while (!playerScript.action && clock.Elapsed.TotalMilliseconds <= 1300)
        {
            spriteRenderer.sprite = hitting;
            await Task.Delay(25);
        }

        clock.Stop();

        spriteRenderer.sprite = idle;
        playerScript.action = false;

        return (clock.Elapsed.TotalMilliseconds >= 700 && clock.Elapsed.TotalMilliseconds <= 1300);
    }
}
