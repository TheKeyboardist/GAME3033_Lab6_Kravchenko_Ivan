using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent (typeof(StateMachine))]
public class ZombieComponent : MonoBehaviour
{

    public NavMeshAgent ZombieNavMesh { get; private set; }
    public Animator ZombieAnimator { get; private set; }

    public StateMachine StateMachine { get; private set; }


    public GameObject FollowTarget;

    [SerializeField] private bool _Debug;


    private void Awake()
    {
        ZombieNavMesh = GetComponent<NavMeshAgent>();
        ZombieAnimator = GetComponent<Animator>();
        StateMachine = GetComponent<StateMachine>();



    }
    // Start is called before the first frame update
    void Start()
    {
        if (_Debug)
        {
            Initialize(FollowTarget);
        }
    }

    public void Initialize(GameObject followTarget)
    {
        FollowTarget = followTarget;

        ZombieIdleState idleState = new ZombieIdleState(this, StateMachine);
        StateMachine.AddState(ZombieStateType.Idle, idleState);

        ZombieFollowState followState = new ZombieFollowState(FollowTarget, this, StateMachine);
        StateMachine.AddState(ZombieStateType.Follow, followState);

        ZombieAttackState attackState = new ZombieAttackState(FollowTarget, this, StateMachine);
        StateMachine.AddState(ZombieStateType.Attack, attackState);


        ZombieDeadState deadState = new ZombieDeadState(this, StateMachine);
        StateMachine.AddState(ZombieStateType.Dead, deadState);


        //StateMachine.Intialize(ZombieStateType.Idle);
        //StateMachine.Intialize(ZombieStateType.Follow);
        //StateMachine.Intialize(ZombieStateType.Attack);
        StateMachine.Intialize(ZombieStateType.Dead);



    }

}
