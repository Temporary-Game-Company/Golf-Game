using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public int health;
    public float speed;

    private Vector3 originalPosition;

    private GameObject player;

    private Vector3 target;
    private float approachDistance = 4f;

    public static bool approach;
    public static bool returning;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;

        player = GameObject.Find("Player");
        target = player.transform.position + new Vector3(approachDistance,0,0);
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

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);

        if (Vector3.Distance(transform.position, target) <= 0f)
        {
            approach = false;

            // ATTACK HAPPENS HERE!

            Return();
        }
    }

    public void Return()
    {
        returning = true;

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, originalPosition, step);

        if (Vector3.Distance(transform.position, originalPosition) <= 0f)
        {
            returning = false;
        }
    }
}
