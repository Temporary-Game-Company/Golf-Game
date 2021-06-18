using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{
    public static int health = 100;
    public bool action;

    // Start is called before the first frame update
    void Start()
    {
        action = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            action = true;
        }
    }

    public void TakeDamage(int attackPower)
    {
        health -= attackPower;
        Debug.Log(health);

        if (health <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        SceneManager.LoadScene("Game Over");
    }
}
