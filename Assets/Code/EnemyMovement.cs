using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 3f; // Speed of movement

    private Transform target;

    void Start()
    {
        target = pointB;
        Debug.Log($"Starting Target: {target.name}");
    }

    void Update()
    {
        // Move towards the target point
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Switch target when reaching the current target
        if (Vector3.Distance(transform.position, target.position) < 1f)
        {
            target = target == pointA ? pointB : pointA; // Toggle target
            Debug.Log($"Switching Target to: {target.name}");
        }
    }
}