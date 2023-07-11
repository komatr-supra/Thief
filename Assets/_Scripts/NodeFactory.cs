using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NodeFactory
{
    public static Node CreateNode(Character character, string name, NodeType mainNodeType, params (LeafType, object)[] subNodesTuple)
    {
        
        var mainNode = SelectMainNodeType(mainNodeType, name);
        foreach (var tupleNode in subNodesTuple)
        {
            mainNode.AddChild(CreateSubnode(name, character, tupleNode.Item1, tupleNode.Item2));
        }
        return mainNode;
    }
    private static Node SelectMainNodeType(NodeType nodeType, string name)
    {
        switch (nodeType)
        {
            case NodeType.selector : return new Selector(name);
            case NodeType.sequence : return new Sequence(name);            
        }
        return new Node("wrong node in factory! in SelectNodeType");
    }
    private static Node CreateSubnode(string name, Character character, LeafType leafType, object nodeData)
    {
        switch (leafType)
        {
            case LeafType.move : return new LeafMove($"leaf move of {name}", character, (Vector3)nodeData);
            case LeafType.interact : return new LeafMove($"leaf interact of {name}", character, (Vector3)nodeData);
        }
        return new Node("wrong created subnode in CreateSubnode factory");
    }
}
public enum NodeType
{
    selector, sequence
}
public enum LeafType
{
    move, interact
}
