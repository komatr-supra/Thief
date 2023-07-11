using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafInteract : Node
{
    private Character character;
    private GameObject gameObject;
    public LeafInteract(string name, Character character, GameObject interactableObject) : base(name)
    {
        this.character = character;
        gameObject = interactableObject;
    }

    public override Status Process()
    {
        return Interact(gameObject, character);
    }
    private Node.Status Interact(GameObject target, Character character)
    {
        if(target.TryGetComponent<Loot>(out Loot loot))
        {
            return character.TakeLoot(loot) ? Node.Status.sucess : Node.Status.failure;
        }
        if(target.TryGetComponent<IUsable>(out IUsable usableObject))
        {
            return usableObject.Use() ? Node.Status.sucess : Node.Status.failure;
        }
        return Node.Status.failure;
    } 
}
