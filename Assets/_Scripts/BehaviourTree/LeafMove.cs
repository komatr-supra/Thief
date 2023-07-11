using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LeafMove : Node
{
    private Character character;
    private Vector3 targetPosition;
    public LeafMove(string name, Character character, Vector3 target) : base(name)
    {
        this.character = character;
        targetPosition = target;
    }
    public override Status Process()
    {
        return GoToPosition(targetPosition);
    }
    private Node.Status GoToPosition(Vector3 targetPosition, NavmeshMask mask = NavmeshMask.basic | NavmeshMask.inner)
        {
            if(character.State != Character.ActionState.working)
            {
                NavMesh.SamplePosition(targetPosition, out var point, 5f, (int)mask);
                NavMeshPath path = new NavMeshPath();
                NavMesh.CalculatePath(character.transform.position, targetPosition, (int)mask, path);
                character.NavMeshAgent.SetPath(path);
                character.State = Character.ActionState.working;
                Debug.Log("position set");
                return Node.Status.running;
            }
            else if(Vector3.Distance(character.transform.position, character.NavMeshAgent.destination) < 2f)
            {
                character.State = Character.ActionState.idle;
                Debug.Log("character at position");
                return Node.Status.sucess;
            }
            return Node.Status.running;
            
        }
    
}
