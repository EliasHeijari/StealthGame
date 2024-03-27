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
        float playerHeight = 1f;
        if (Physics.Raycast(transform.position, directionToPlayer + Vector3.up * playerHeight, out RaycastHit hit, 300f))
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

                if (!IsMaterialSeeThrough(hit.collider.gameObject))
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

    private bool IsMaterialSeeThrough(GameObject targetObject)
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
