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

    public BallController ballController; // BallController passes itself to this object.

    // Start is called before the first frame update
    void Start()
    {
        turn = 1;

        ui = GameObject.Find("Main Combat");

        ballController = GameObject.Find("Player").GetComponent<BallController>();

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

    public void ChooseAction(string actionChosen)
    {
        ChangeTurn(PLAYER_ACTION);
        if (actionChosen == "attack")
        {
            ballController.CreateBall();
        }
    }

    public async void EnemyTurn()
    {
        foreach (GameObject enemy in enemies)
        {
            EnemyBehaviour behaviour = enemy.GetComponent<EnemyBehaviour>();

            behaviour.Approach();
        }

        await WaitEnemyTurn();

        ChangeTurn(PLAYER_TURN);
    }

    public async Task WaitEnemyTurn()
    {
        foreach (GameObject enemy in enemies)
        {
            EnemyBehaviour behaviour = enemy.GetComponent<EnemyBehaviour>();

            while (behaviour.attacking)
            {
                await Task.Delay(25);
            }
        }
    }
}
