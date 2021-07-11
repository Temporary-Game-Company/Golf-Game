using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ONLY STUFF WHICH CAN APPLY TO BOTH BATTLE AND OVERWORLD!
public class PlayerBehaviour : MonoBehaviour
{
    private static int maxHealth = 100;
    private static int maxMana = 100;
    private static int health = 100;
    private static int mana = 100;
    private static int xp;

    public bool action;

    public GameManager gameManager;

    public string currentScene;

    public GameObject combatObject;
    public GameObject overworldObject;

    public BarMasker healthMask;
    public BarMasker manaMask;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);

        SceneManager.activeSceneChanged += ChangedActiveScene;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        action = false;

        currentScene = SceneManager.GetActiveScene().name; // Saves current scene name.
        
        healthMask = GameObject.Find("HealthMask").GetComponent<BarMasker>();
        manaMask = GameObject.Find("ManaMask").GetComponent<BarMasker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            action = true;
        }
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        Debug.Log("changed player scene");
        currentScene = next.name; // Saves new scene name.

        Debug.Log(xp);

        switch (currentScene)
        {
            case "OverWorld":
                combatObject.SetActive(false);
                overworldObject.SetActive(true);
                break;
            case "Battle":
                combatObject.SetActive(true);
                overworldObject.SetActive(false);
                break;
            default:
                combatObject.SetActive(false);
                overworldObject.SetActive(false);
                break;
        }
    }
    
    public void TakeDamage(int attackPower)
    {
        health -= attackPower;

        if (health <= 0)
        {
            gameManager.GameOver();
        }

        healthMask.UpdateMask(health, maxHealth);
    }
    
    public void LoseMana(int manaCost)
    {
        mana -= manaCost;

        if (mana <= 0)
        {
            gameManager.GameOver();
        }

        manaMask.UpdateMask(mana, maxMana);
    }

    public void GainXP(int xp_gain)
    {
        xp += xp_gain;
    }
}
