using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBehaviour : MonoBehaviour
{
    ParticleSystem particleSystem;
    Vector3 boxSize;
    float height;
    float width;
    Vector3 direction;

    [SerializeField] float speed;
    
    // Start is called before the first frame update
    void Start()
    {
        particleSystem = gameObject.GetComponent<ParticleSystem>();

        boxSize = gameObject.GetComponent<BoxCollider2D>().size;
        width = boxSize[0];
        height = boxSize[1];
        direction = gameObject.GetComponent<Transform>().rotation * Vector3.right;
        
        var emitterSettings = particleSystem.main;
        emitterSettings.startSpeed = speed;
        emitterSettings.startLifetime = width/speed;

        var emitterShape = particleSystem.shape;
        emitterShape.position = new Vector3(-width/2, 0f, 0f);
        emitterShape.scale = new Vector3(width, 1f, 1f);
        emitterShape.radius = height / (2 * width);

        var emission = particleSystem.emission;
        emission.rateOverTime = 2 * height;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D objectInside)
    { 
        if (objectInside.tag == "Ball")
        {
            objectInside.GetComponent<Rigidbody2D>().AddForce((speed/10) * direction);
        }
    }
}
