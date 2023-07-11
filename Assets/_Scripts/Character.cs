using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    public BTTicker ticker;
    public GameObject apple;
    public GameObject backdoor;
    public GameObject frontdoor;
    public GameObject diamond;
    public GameObject car;
    [SerializeField] private Transform lootPosition;
    private Loot slot;
    private BTCharacter behaviourTree;
    public enum ActionState{idle, working}
    public NavMeshAgent NavMeshAgent {get; private set;}
    public ActionState State = ActionState.idle;
    private void Awake()
    {
    }
    private void Start()
    {
        behaviourTree = new BTCharacter();
        NavMeshAgent = GetComponent<NavMeshAgent>();
        MainTree tree = new MainTree("character main tree");

        Sequence steal = new Sequence("steal sequence");
        tree.AddChild(steal);
        Selector doorSelector = new Selector("Select door");
        steal.AddChild(doorSelector);

        Sequence sequenceGoAndOpenFrontDoor = new Sequence("sequence go and open front door");
        doorSelector.AddChild(sequenceGoAndOpenFrontDoor);
        Sequence sequenceGoAndOpenBackDoor = new Sequence("sequence go and open back door");
        doorSelector.AddChild(sequenceGoAndOpenBackDoor);

        LeafMove leafMoveToFrontDoor = new LeafMove("move to front door", this, frontdoor.transform.position);
        LeafMove leafMoveToBackDoor = new LeafMove("move to back door", this, backdoor.transform.position);
        LeafInteract backDoorLeaf = new LeafInteract("Interact with back door", this, backdoor);
        LeafInteract frontDoorLeaf = new LeafInteract("Interact with back door", this, frontdoor);

        sequenceGoAndOpenBackDoor.AddChild(leafMoveToBackDoor);
        sequenceGoAndOpenBackDoor.AddChild(backDoorLeaf);
        sequenceGoAndOpenFrontDoor.AddChild(leafMoveToFrontDoor);
        sequenceGoAndOpenFrontDoor.AddChild(frontDoorLeaf);

        Sequence sequenceGoAndPickDiamond = new Sequence("sequence go and pick diamond");
        steal.AddChild(sequenceGoAndPickDiamond);
        LeafMove goToDiamond = new LeafMove("go to the diamond", this, diamond.transform.position);
        LeafInteract pickDiamond = new LeafInteract("pick diamond", this, diamond);
        sequenceGoAndPickDiamond.AddChild(goToDiamond);
        sequenceGoAndPickDiamond.AddChild(pickDiamond);

        LeafMove goToCar = new LeafMove("go to car", this, car.transform.position);
        steal.AddChild(goToCar);

        
        steal.AddChild(NodeFactory.CreateNode(this, "go to apple", NodeType.sequence, 
                (LeafType.move, apple.transform.position), (LeafType.interact, apple.transform)));
        
        behaviourTree.SetBehaviourTree(tree);
        ticker.RegisterCharacterTick(behaviourTree);
    }
    
    public bool TakeLoot(Loot loot)
    {
        if(slot != null) return false;

        slot = loot;
        loot.transform.SetParent(lootPosition);
        loot.transform.localPosition = Vector3.zero;
        loot.transform.localScale = Vector3.one * 2;
        return true;
    }
}
    /*
    private NavMeshAgent navMeshAgent;
    [SerializeField] private CharacterTypeScriptable characterType;    
    [SerializeField] private GameObject diamond;
    [SerializeField] private GameObject car;
    [SerializeField] private GameObject backDoor;
    [SerializeField] private GameObject frontDoor;
    private enum ActionState{idle, working}
    [SerializeField] private ActionState state = ActionState.idle;
    private MainTree behaviourTree;
    [SerializeField]private Node.Status treeStatus = Node.Status.running;
    public NavmeshMask usedMask;
    [SerializeField] private Transform lootPosition;
    private Loot slot;
    [Range(0,1000)]
    public int money = 500;
    private void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void Start() {
        behaviourTree = new MainTree();

        Leaf needMoney = new Leaf("Havent got money", HaveMoney, true);

        Sequence steal = new Sequence("Steal something");
        Leaf goToBackDoor = new Leaf("Go to door", () => GoToObjectAndUse(backDoor, usedMask));
        Leaf goToFrontDoor = new Leaf("Go to front door", ()=> GoToObjectAndUse(frontDoor, usedMask));
        
        Selector goToDoor = new Selector("Go to door");
        
        Leaf goToDiamond = new Leaf("Go to diamond", () => GoToObjectAndTake(diamond));
        Leaf goToVan = new Leaf("Go to car", ReturnToCar);

        behaviourTree.AddChild(steal);

        goToDoor.AddChild(goToBackDoor);
        goToDoor.AddChild(goToFrontDoor);

        steal.AddChild(needMoney);
        steal.AddChild(goToDoor);
        steal.AddChild(goToDiamond);
        steal.AddChild(goToVan);

        StringBuilder sb = new();
        behaviourTree.PrintNode(sb, 0);
        Debug.Log(sb);

        
        
    }
    private void Update() {
        if(treeStatus != Node.Status.sucess) treeStatus = behaviourTree.Process();
    }
    private Node.Status HaveMoney()
    {
        if(money < 500) return Node.Status.failure;
        return Node.Status.sucess;
    }
    private Node.Status GoToObjectAndUse(GameObject target, NavmeshMask mask = NavmeshMask.basic | NavmeshMask.inner)
    {
        var s = GoToPosition(target.transform.position, mask);
        if(s != Node.Status.sucess) return s;
        
        if(target.TryGetComponent<IUsable>(out IUsable usableObject))
        {
            return usableObject.Use() ? Node.Status.sucess : Node.Status.failure;
        }
        return Node.Status.failure;

        
    }
    private Node.Status GoToObjectAndTake(GameObject target, NavmeshMask mask = NavmeshMask.basic | NavmeshMask.inner)
    {
        var s = GoToPosition(target.transform.position, mask);

        if(s != Node.Status.sucess) return s;

        if(slot != null) return Node.Status.failure;

        if(target.TryGetComponent<Loot>(out Loot loot))
        {
            slot = loot;
            loot.transform.SetParent(lootPosition);
            loot.transform.localPosition = Vector3.zero;
            loot.transform.localScale = Vector3.one * 2;
            return Node.Status.sucess;
        }
        return Node.Status.failure;
    } 
    private Node.Status ReturnToCar()
    {
        Node.Status s = GoToPosition(car.transform.position);
        if(s != Node.Status.sucess) return s;

        if(slot != null)
        {
            money += slot.ItemValue;
            slot.gameObject.SetActive(false);
            slot = null;
        }
        return Node.Status.sucess;
    }
    private Node.Status GoToPosition(Vector3 targetPosition, NavmeshMask mask = NavmeshMask.basic | NavmeshMask.inner)
    {
        if(state != ActionState.working)
        {
            NavMesh.SamplePosition(targetPosition, out var point, 5f, (int)mask);
            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(transform.position, targetPosition, (int)mask, path);
            navMeshAgent.SetPath(path);
            state = ActionState.working;
            return Node.Status.running;
        }
        if(Vector3.Distance(transform.position, navMeshAgent.destination) < 2f)
        {
            state = ActionState.idle;
            return Node.Status.sucess;
        }
        return Node.Status.running;
        
    }
}*/
