using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


enum EnemyState
{
    Patrol,
    Searching,
    Attacking
}



public class EnemyMovement : MonoBehaviour, IInteractable
{
    [SerializeField] List<Transform> points = new List<Transform>();
    int index = 0;
    NavMeshAgent agent;
    Transform playerTransform;
    int playerLayer;
    [SerializeField]
    float fov = 90.0f;
    EnemyState state = EnemyState.Patrol;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = points[index].position;

        GameObject player = GameObject.Find("Player");
        Debug.Assert(player != null);
        playerTransform = player.transform;
        playerLayer = 1 << 7;
    }

    // Update is called once per frame
    void Update()
    {
        updateAgent();
        if (playerInSight())
        {
            state = EnemyState.Searching;
            agent.destination = playerTransform.position;
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
        Vector3 direction = Vector3.Normalize(playerTransform.position - transform.position);
        if(2 * Vector3.Angle(direction, transform.forward) < fov && !Physics.Raycast(transform.position, direction, Vector3.Distance(transform.position, playerTransform.position), ~playerLayer))
        {
            Debug.DrawLine(transform.position, playerTransform.position, Color.red);
            return true;
        }
        else
        {
            Debug.DrawLine(transform.position, playerTransform.position, Color.green);
            return false;
        }
    }

    public void Interact(Transform interactorTransform)
    {
        Destroy(gameObject);
    }

    public string GetInteractText()
    {
        return "Kill";
    }

    public Transform GetTransform()
    {
        return transform;
    }
}

