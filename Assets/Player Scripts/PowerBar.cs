using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBar : MonoBehaviour // Displays and moves the fancy bar to indicate power.
{
    private BallController controller;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.localPosition = new Vector3(0f, -1.3f, -2f); // Places the indicator at the bottom of the bar.

        controller = GameObject.Find("BattleManager").GetComponent<BallController>(); // Finds the player BallController object to get the force.
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.localPosition = new Vector3(0f, -1.3f + 2.6f * (controller.force/controller.MAX_FORCE), -1f); // Moves the indicator up and down to show the current power.
    }
}
