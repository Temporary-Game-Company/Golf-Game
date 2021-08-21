using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBarMovement : MonoBehaviour
{
    private Scrollbar scrollbar;
    
    [SerializeField]
    private BallController controller;

    // Start is called before the first frame update
    void Start()
    {
        scrollbar = GetComponent<Scrollbar>();
    }

    // Update is called once per frame
    void Update()
    {
        scrollbar.value = (controller.force - controller.MIN_FORCE)/(controller.MAX_FORCE - controller.MIN_FORCE);
    }
}
