using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMove : MonoBehaviour
{
    public Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
    }

    // Update is called once per frame
    public void Down()
    {
        transform.position = new Vector3(pos.x, pos.y-10f, pos.z);
    }

    public void Up()
    {
        transform.position = pos;
    }
}
