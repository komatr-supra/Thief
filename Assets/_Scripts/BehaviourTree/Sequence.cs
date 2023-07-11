using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    public Sequence(string name) : base(name)
    {
    }
    public override Status Process()
    {
        Status sequenceStatus = childrens[currentChild].Process();
        if(sequenceStatus != Status.sucess) return sequenceStatus;
        
        currentChild++;
        if(currentChild >= childrens.Count)
        {
            currentChild = 0;
            return Status.sucess;
        }
        return Status.running;
    }
}
