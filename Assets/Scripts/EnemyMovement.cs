using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] List<Transform> points = new List<Transform>();
    int index = 0;
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = points[index].position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.2)
        {
            index = (index + 1) % points.Count;
            agent.destination = points[index].position;
        }
    }
}
