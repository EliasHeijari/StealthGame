using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : NetworkBehaviour, IInteractable
{
    public enum EnemyState
    {
        Patrol,
        Searching,
        Attacking
    }

    [SerializeField] List<Transform> patrolPoints = new List<Transform>();
    [SerializeField] private float hearingDistance = 8f;
    int index = 0;
    NavMeshAgent agent;
    Transform playerTransform;
    int playerLayer;
    [SerializeField]
    float fov = 90.0f;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = patrolPoints[index].position;

        GameObject player = GameObject.Find("Player");
        Debug.Assert(player != null, "Player is null!");
        playerTransform = player.transform;
        playerLayer = 1 << 7;
    }

    // Update is called once per frame
    void Update()
    {
        updateAgent();
        if (playerInSight())
        {
            Debug.Log("Game Over, Wasted!");
        }
    }

    void updateAgent()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.2)
        {
            index = (index + 1) % patrolPoints.Count;
            agent.destination = patrolPoints[index].position;
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

    public void SoundHeard(Transform soundTransform)
    {
        Vector3 soundPosition = soundTransform.position;
        if (!IsSoundOnRange(soundPosition)) return;

        // Move Towards sound
        agent.destination = soundPosition;

    }

    private bool IsSoundOnRange(Vector3 soundPosition)
    {
        if (Vector3.Distance(transform.position, soundPosition) < hearingDistance)
        {
            return true;
        }
        return false;
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

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }

    private void OnDrawGizmos()
    {
        foreach(var t in patrolPoints)
        {
            Gizmos.DrawSphere(t.position, 0.4f);
        }
        Gizmos.DrawWireSphere(transform.position, hearingDistance);
    }
}

