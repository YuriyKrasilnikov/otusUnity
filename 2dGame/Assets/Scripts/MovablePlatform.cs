using System;
using UnityEngine;

public class MovablePlatform : MonoBehaviour
{

    public Transform[] Target;
    [Range(0.0f, 10.0f)]
    public float Speed;


    Vector3 velocityVector;
    float lastDistance=-1;
    int currentTarget = 0;


    Rigidbody2D rigidBody2D;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        transform.position = Target[currentTarget].position;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Vector3.Distance(Target[currentTarget].position, transform.position));

        if (Vector3.Distance(Target[currentTarget].position, transform.position) > lastDistance)
        {
            //transform.position = Target[currentTarget].position;

            if (++currentTarget >= Target.Length)
            {
                currentTarget = 0;
            }

            velocityVector = Vector3.Normalize(Target[currentTarget].position - transform.position) * Speed;
            rigidBody2D.velocity = velocityVector;
            //Debug.Log("velocity " + rigidBody2D.velocity);
        }
        lastDistance = Vector3.Distance(Target[currentTarget].position, transform.position);

    }
}
