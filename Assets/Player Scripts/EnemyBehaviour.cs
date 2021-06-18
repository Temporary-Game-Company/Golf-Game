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

    private GameObject player;
    private PlayerBehaviour playerScript;

    private Vector3 target;
    private float approachDistance = 4f;

    private Stopwatch clock;

    public bool approach;
    public bool returning;
    public bool attacking;

    private bool blocked;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;

        player = GameObject.Find("Player");
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        playerScript = player.GetComponent<PlayerBehaviour>();
        target = player.transform.position + new Vector3(approachDistance, 0, 0);
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
        UnityEngine.Debug.Log(clock.Elapsed);

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
        while (!playerScript.action && clock.Elapsed.TotalMilliseconds <= 2500)
        {
            spriteRenderer.sprite = chargingAttack;
            await Task.Delay(25);
        }
        while (!playerScript.action && clock.Elapsed.TotalMilliseconds <= 3500)
        {
            spriteRenderer.sprite = hitting;
            await Task.Delay(25);
        }

        clock.Stop();

        spriteRenderer.sprite = idle;
        playerScript.action = false;

        if (clock.Elapsed.TotalMilliseconds >= 2500 && clock.Elapsed.TotalMilliseconds <= 3500)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
