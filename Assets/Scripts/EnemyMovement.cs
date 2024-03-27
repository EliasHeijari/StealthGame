using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;





public class EnemyMovement : MonoBehaviour
{
    [SerializeField] List<Transform> points = new List<Transform>();
    int index = 0;
    NavMeshAgent agent;
    Transform playerTransform;
    int playerLayer;
    [SerializeField]
    float fov = 90.0f;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = points[index].position;

        GameObject player = GameObject.Find("Player");
        Debug.Assert(player != null);
        playerTransform = player.transform;
        playerLayer = player.layer;
    }

    // Update is called once per frame
    void Update()
    {
        updateAgent();
        if (playerInSight())
        {
            // Do something?
        }
    }

    void updateAgent()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.2)
        {
            index = (index + 1) % points.Count;
            agent.destination = points[index].position;
        }
    }


    bool playerInSight()
    {

        Vector3 direction = playerTransform.position - transform.position;
        if(2 * Vector3.Angle(direction, transform.forward) < fov && !Physics.Raycast(transform.position, direction, Vector3.Distance(transform.position, playerTransform.position), ~playerLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

