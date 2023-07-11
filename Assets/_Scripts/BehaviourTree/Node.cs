using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class Node
{
    public string nameNode;
    public enum Status { sucess, running, failure}
    public Status status;
    public List<Node> childrens;
    public int currentChild;

    public Node(string name)
    {
        nameNode = name;
    }
    public virtual Status Process()
    {
        if(childrens != null)
        {
            childrens[currentChild].Process();
        }
        return Status.sucess;
    }
    public void AddChild(Node childNode)
    {
        if(childrens == null)
        {
            childrens = new();
        }
        childrens.Add(childNode);
    }
    public void PrintNode(StringBuilder stringBuilder,int nodeLevel = 0)
    {   
        stringBuilder.Append(new string('-', nodeLevel));
        stringBuilder.Append(nameNode);
        stringBuilder.Append("\n");

        if(childrens != null) 
        {
            foreach (var node in childrens)
            {
                node.PrintNode(stringBuilder, nodeLevel + 1);
            }
        }        
    }
}
