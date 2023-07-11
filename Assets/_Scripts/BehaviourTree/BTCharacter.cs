using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.AI;

public class BTCharacter
{
    private NavMeshAgent navMeshAgent;
    private MainTree behaviourTree;
    private Node.Status treeStatus = Node.Status.running;
    public NavmeshMask usedNavMeshMask;
    public void Tick() {
        if(treeStatus != Node.Status.sucess) treeStatus = behaviourTree.Process();
    }
    
    public void SetBehaviourTree(MainTree tree) => behaviourTree = tree;    
    
    
}
