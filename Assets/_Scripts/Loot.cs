using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Loot : MonoBehaviour
{
    [SerializeField] private bool isRotating;
    [SerializeField] private float rotateSpeed = 20;
    [SerializeField] private int _value = 100;
    public int ItemValue => _value;    
    
    [SerializeField] private string _itemName = "not set";
    public string ItemName => _itemName;
    
    private bool _isAvaliable = true;
    public bool IsAvaliable
    {
        get { return _isAvaliable; }
        set { _isAvaliable = value; }
    }

    private void Start() {
        if(isRotating) StartCoroutine(RotateCoroutine());
    }
    private IEnumerator RotateCoroutine()
    {
        while (true)
        {
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
