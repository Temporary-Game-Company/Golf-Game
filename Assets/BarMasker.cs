using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarMasker : MonoBehaviour
{
    private float max;
    private Rect rectangle;

    // Start is called before the first frame update
    void Start()
    {
        max = 1f;
        rectangle = GetComponent<RectTransform>().rect;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateMask(int variable, int maxVariable)
    {
        Debug.Log(4);
        transform.localScale = new Vector3((float)variable / maxVariable, 1, 1) * max;
    }
}
