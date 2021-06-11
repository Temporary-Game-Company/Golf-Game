﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAngle : MonoBehaviour // Displays and turns the fancy arrow to indicate aim.
{
    private GameObject player;
    private BallController controller;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        controller = player.GetComponent<BallController>(); // Finds the BallController object which controls the launch angle.
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.state == 0) // If the angle is being selected, updates the arrow accordingly.
        {
            gameObject.transform.rotation = controller.launchDirection;
        }
        else if (controller.state >= 2) // Preferably we'd hide the arrow when it's not in use.
        {
           // gameObject.SetActive(false);
        }
    }
}
