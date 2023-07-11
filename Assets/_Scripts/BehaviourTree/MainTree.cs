using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTree : Node
{
    public MainTree(string name = "Behaviour Tree"):base(name)
    {
        
    }
    public override Status Process()
    {
        return childrens[currentChild].Process();
    }
}
