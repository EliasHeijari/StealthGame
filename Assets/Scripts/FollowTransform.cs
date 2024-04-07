using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    Transform targetTransform;

    public Transform target { get { return targetTransform; } set { targetTransform = value; } }

    private void Update()
    {
        if (target == null) return;
        transform.position = target.position;
        transform.rotation = target.rotation;
    }
}
