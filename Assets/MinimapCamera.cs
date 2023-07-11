using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    [SerializeField] private Transform followedObject;
    void LateUpdate()
    {
        transform.position = new Vector3(followedObject.position.x, transform.position.y, followedObject.position.z);
    }
}
