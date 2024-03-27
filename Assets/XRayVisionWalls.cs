using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRayVisionWalls : MonoBehaviour
{
    private GameObject player;
    GameObject lastHittedObject;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null) Debug.LogError("Player does not have a player tag");
    }

    private void Update()
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, 300f))
        {
            if (hit.collider.gameObject != player)
            {
                if (hit.collider.gameObject != lastHittedObject)
                {
                    if (lastHittedObject != null)
                    {
                        MakeMaterialSolid(lastHittedObject);
                    }
                    lastHittedObject = hit.collider.gameObject;
                }

                if (!IsSeeThrough(hit.collider.gameObject))
                {
                    MakeMaterialSeeThrough(hit.collider.gameObject);
                }
            }
            else if (lastHittedObject != null)
            {
                MakeMaterialSolid(lastHittedObject);
            }
        }
    }

    private bool IsSeeThrough(GameObject targetObject)
    {
        if (targetObject.TryGetComponent(out MeshRenderer meshRenderer))
        {
            return meshRenderer.material.color.a < 0.9f;
        }
        return true;
    }

    private void MakeMaterialSeeThrough(GameObject targetObject)
    {
        if (targetObject.TryGetComponent(out MeshRenderer objectRender))
        {
            Color objectColor = objectRender.material.color;
            objectRender.material.color = new Color(objectColor.r, objectColor.g, objectColor.b, .5f);
        }
    }

    private void MakeMaterialSolid(GameObject targetObject)
    {
        if (targetObject.TryGetComponent(out MeshRenderer objectRender))
        {
            Color objectColor = objectRender.material.color;
            objectRender.material.color = new Color(objectColor.r, objectColor.g, objectColor.b, 1f);
        }
    }
}
