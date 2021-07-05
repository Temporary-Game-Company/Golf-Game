using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : StateMachineBehaviour
{
     override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
     {
          Debug.Log(5);
          animator.SetBool("Poofing", false);
     }
}
