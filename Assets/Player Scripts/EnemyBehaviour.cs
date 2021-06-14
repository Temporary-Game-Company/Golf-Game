using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public int health;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Debug.Log(1);
            Destroy(gameObject);
        }
    }

    public void Approach()
    {
        Debug.Log(player.transform.position);
    }
}
