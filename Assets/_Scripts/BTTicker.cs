using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTTicker : MonoBehaviour
{    
    [SerializeField] [Range(1,10)] private int updatesPerTick = 3;
    [SerializeField] [Range(0.05f, 1f)] private float tickLenght = 0.2f;
    private Action[] actions;
    private float counter;
    private float tickDuration;
    private int updatedActionIndex;
    private int indexAction;
    private Queue<int> freeActionIndexes;
    private Dictionary<BTCharacter, int> registeredBTs;
    private void Awake()
    {
        freeActionIndexes = new();
        registeredBTs = new();
        actions = new Action[updatesPerTick];
        tickDuration = tickLenght / updatesPerTick;
    }
    void Start()
    {
        
    }

    void Update()
    {
        counter += Time.deltaTime;
        if(counter >= tickDuration)
        {
            actions[updatedActionIndex]?.Invoke();
            counter = 0;
            updatedActionIndex = ++updatedActionIndex < updatesPerTick ? updatedActionIndex : 0;
        }
    }
    public void RegisterCharacterTick(BTCharacter behaviourTree)
    {
        if(freeActionIndexes.TryDequeue(out var reusedIndex))
        {
            actions[reusedIndex] += behaviourTree.Tick;
            registeredBTs.Add(behaviourTree, reusedIndex);
        }        
        Debug.Log("action index in array: " + indexAction + "; action array size is: " + updatesPerTick + "("+actions.Length+")");
        actions[indexAction] += behaviourTree.Tick;
        registeredBTs.Add(behaviourTree, indexAction);
        NextIndex();
    }
    public void UnregisterCharacterTick(BTCharacter behaviourTree)
    {
        int index = registeredBTs[behaviourTree];
        actions[index] -= behaviourTree.Tick;
        freeActionIndexes.Enqueue(index);
        registeredBTs.Remove(behaviourTree);
    }
    private void NextIndex()
    {
        indexAction = ++indexAction < updatesPerTick ? indexAction : 0;
    }
}

