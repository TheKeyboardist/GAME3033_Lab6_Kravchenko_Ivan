using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttackState : ZombieStates
{

    private GameObject FollowTarget;

    private float AttackRange = 0.80f;
    public ZombieAttackState(GameObject followTarget ,ZombieComponent zombie, StateMachine stateMachine) : base(zombie, stateMachine)
    {

        FollowTarget = followTarget;
        UpdateInterval = 2.0f;
    }



    // Start is called before the first frame update
    public override void Start()
    {
        OwnerZombie.ZombieNavMesh.isStopped = true;
        OwnerZombie.ZombieNavMesh.ResetPath();
        OwnerZombie.ZombieAnimator.SetFloat("MovementZ",0.0f);
        OwnerZombie.ZombieAnimator.SetBool("isAttacking", true);



    }

    public override void IntervalUpdate()
    {
        base.IntervalUpdate();
        //Add Damage to Object
    }

    // Update is called once per frame
    public override void Update()
    {
        OwnerZombie.transform.LookAt(FollowTarget.transform.position, Vector3.up);

        float distanceBetween = Vector3.Distance(OwnerZombie.transform.position, FollowTarget.transform.position);
        if (distanceBetween > AttackRange)
        {
            StateMachine.ChangeState(ZombieStateType.Follow);
        }

        //Zombie Health < 0 Die.
    }

    public override void Exit()
    {
        base.Exit();

        OwnerZombie.ZombieAnimator.SetBool("isAttacking", true);

    }
}
