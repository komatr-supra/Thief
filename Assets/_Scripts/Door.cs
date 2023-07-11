using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Door : MonoBehaviour, IUsable
{
    [SerializeField] private float openingSpeed = 5f;
    private bool isOpen = false;
    public bool isLocked = false;
    private Coroutine doorCoroutine;
    
    public bool Use()
    {
        if(isLocked) return false;
        if(doorCoroutine == null)
        {
            doorCoroutine = StartCoroutine(OpenDoor());
            return true;
        } 
        return false;
    }
    private IEnumerator OpenDoor()
    {
        if(isOpen) yield break;
        float openAngle = transform.rotation.eulerAngles.y + 85;
        GetComponent<NavMeshObstacle>().enabled = false;
        while (transform.rotation.eulerAngles.y < openAngle)
        {
            transform.Rotate(Vector3.up, openingSpeed * Time.deltaTime);
            yield return null;
        }
        isOpen = true;
        Debug.Log("door is open");
        transform.rotation = Quaternion.Euler(0, openAngle, 0);
    }
}
