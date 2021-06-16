using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public int health;
    public int speed;
    public int attack;

    private Vector3 originalPosition;

    private GameObject player;
    private PlayerBehaviour playerScript;

    private Vector3 target;
    private float approachDistance = 4f;

    public bool approach;
    public bool returning;
    public bool attacking;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;

        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerBehaviour>();
        target = player.transform.position + new Vector3(approachDistance, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        if (approach)
        {
            Approach();
        }

        if (returning)
        {
            Return();
        }
    }

    public void Approach()
    {
        approach = true;
        attacking = true;

        float step = (20 + speed) * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);

        if (Vector3.Distance(transform.position, target) <= 0f)
        {
            approach = false;

            // ATTACK HAPPENS HERE!
            AttackPlayer();

            Return();
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

    private void AttackPlayer()
    {
        Debug.Log(attack);
        playerScript.TakeDamage(attack);
    }
}
