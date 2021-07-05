using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;

public class BattleManager : MonoBehaviour
{
    private const int PLAYER_TURN = 1;
    private const int PLAYER_ACTION = 2;
    private const int ENEMY_TURN = 3;

    private static int turn;
    private GameObject ui;
    private string action;

    public GameObject[] enemies;

    public BallController ballController;
    public PlayerBehaviour player; // BallController passes itself to this object.

    public GameManager gameManager;

    private bool waitBool;

    // Start is called before the first frame update
    void Start()
    {
        turn = 1;


        ui = GameObject.Find("Main Combat");
        gameManager = transform.parent.GetComponent<GameManager>();
        ballController = GetComponent<BallController>();

        player = GameObject.Find("Player").GetComponent<PlayerBehaviour>();

        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeTurn(int newTurn)
    {
        turn = newTurn;
        switch (turn)
        {
            case PLAYER_TURN:
                ui.SetActive(true);
                break;
            case PLAYER_ACTION:
                ui.SetActive(false);
                break;
            case ENEMY_TURN:
                ui.SetActive(false);
                EnemyTurn();
                break;
            default:
                ui.SetActive(false);
                break;
        }
    }

    public async void ChooseAction(string actionChosen)
    {
        await Task.Delay(1);

        action = actionChosen;

        ChangeTurn(PLAYER_ACTION);
        switch (action)  // Checks which action was chosen.
        {
            case "attack":
                ballController.CreateBall();
                await Task.Delay(10);

                await WaitForBall();
                break;
            default:
                break;
        }
    }

    public async void EnemyTurn()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null) // Checks if enemies are alive.
            {
                await EnemyAttack(enemy);
            }
        }

        ChangeTurn(PLAYER_TURN);
    }

    public void PlayerWins()  // In case of player win.
    {
        Debug.Log("Winner!");

        player.GainXP(50);
        
        gameManager.WinBattle();
    }

    private async Task WaitForBall() // Task which waits for a variable not to be true. (25 ms delay between checks)
    {
        while (ballController.ballExists)
        {
            await Task.Delay(25);
        }

        CheckEnemies();

        ChangeTurn(ENEMY_TURN);
    }

    private async Task EnemyAttack(GameObject enemy) // Task which waits for a variable not to be true. (25 ms delay between checks)
    {
        EnemyBehaviour behaviour = enemy.GetComponent<EnemyBehaviour>();

        behaviour.Approach();

        while (behaviour.attacking)
        {
            await Task.Delay(25);
        }

        await Task.Delay(2000);
    }

    private void CheckEnemies() // Checks if there are enemies left.
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
        {
            PlayerWins();
        }
    }
}
