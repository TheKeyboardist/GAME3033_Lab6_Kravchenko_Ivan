using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDeadState : ZombieStates
{
    public ZombieDeadState(ZombieComponent zombie, StateMachine stateMachine) : base(zombie, stateMachine)
    {

    }

    public override void Start()
    {
        base.Start();
        OwnerZombie.ZombieNavMesh.isStopped = true;
        OwnerZombie.ZombieNavMesh.ResetPath();

        OwnerZombie.ZombieAnimator.SetFloat("MovementZ", 0);
        OwnerZombie.ZombieAnimator.SetBool("isDead", true);
    }

    // Start is called before the first frame update
   public override void Exit()
    {
        base.Exit();
        OwnerZombie.ZombieNavMesh.isStopped = false;

        OwnerZombie.ZombieAnimator.SetBool("isDead", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
