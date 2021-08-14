using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ONLY STUFF WHICH CAN APPLY TO BOTH BATTLE AND OVERWORLD!
public class PlayerBehaviour : MonoBehaviour
{
    public static int maxHealth = 100;
    public static int maxMana = 100;
    private static int health = 100;
    private static int mana = 100;
    private static int xp;

    private BarMasker healthMask;
    private BarMasker manaMask;

    private string currentScene;

    [SerializeField] private GameObject combatObject;
    [SerializeField] private GameObject overworldObject;

    public bool action;

    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);

        SceneManager.activeSceneChanged += ChangedActiveScene;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        action = false;

        currentScene = SceneManager.GetActiveScene().name; // Saves current scene name.
        UpdateScene();
        
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
        currentScene = next.name; // Saves new scene name.

        UpdateScene();
    }

    private void UpdateScene()
    {
        switch (currentScene)
        {
            case "Overworld":
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
