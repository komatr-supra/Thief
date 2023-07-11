using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
    public Selector(string name) : base(name)
    {
    }
    public override Status Process()
    {
        Status selectorStatus = childrens[currentChild].Process();
        if(selectorStatus != Status.failure) return selectorStatus;

        currentChild++;
        if(currentChild >= childrens.Count)
        {
            currentChild = 0;
            return Status.failure;
        }
        return Status.running;
    }
}
