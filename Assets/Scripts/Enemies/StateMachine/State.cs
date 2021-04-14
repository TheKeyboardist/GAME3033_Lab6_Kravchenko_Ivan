using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class State
    {
        protected StateMachine StateMachine;
        public float UpdateInterval { get;  set; } = 1.0f;

        protected State(StateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        public virtual void Start()
        {

        }
        public virtual void IntervalUpdate()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void FixedUpdate()
        {

        }

        public virtual void Exit()
        {

        }

    }

//public class ZombieStates : State 
//{
//    protected ZombieComponent OwnerZombie;
//   public ZombieStates(ZombieComponent zombie, StateMachine stateMachine) : base(stateMachine)
//    {
//        OwnerZombie = zombie;
//    }
   
//}

