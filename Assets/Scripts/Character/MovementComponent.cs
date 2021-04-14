using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

namespace Character
{
    
    public class MovementComponent : MonoBehaviour
    {
        [SerializeField] private float WalkSpeed;
        [SerializeField] private float RunSpeed;
        [SerializeField] private float JumpForce;

        [SerializeField] private LayerMask JumpLayerMask;
        [SerializeField] private float JumpThreshold = 0.1f;
        [SerializeField] private float JumpLandingCheckDelay = 0.1f;


        [SerializeField] private float MoveDirectionBuffer = 2f;

        private Vector2 InputVector = Vector2.zero;
        private Vector3 MoveDirection = Vector3.zero;
        
        //Comp
        private Animator PlayerAnimator;
        private PlayerController PlayerController;
        private Rigidbody PlayerRigidbody;
        
        
        //Reference 
        private Transform PlayerTransform;

        private Vector3 NextPositionCheck;


        //Animator Hashes
        private readonly int MovementXHash = Animator.StringToHash("MovementX");
        private readonly int MovementZHash = Animator.StringToHash("MovementZ");
        private readonly int IsRunningHash = Animator.StringToHash("IsRunning");
        private readonly int IsJumpingHash = Animator.StringToHash("IsJumping");


        private NavMeshAgent navMeshAgent;
        private void Awake()
        {
            PlayerController = GetComponent<PlayerController>();
            PlayerAnimator = GetComponent<Animator>();
            PlayerRigidbody = GetComponent<Rigidbody>();


            navMeshAgent = GetComponent<NavMeshAgent>();
            PlayerTransform = transform;
        }

        private void Update()
        {
            if (PlayerController.IsJumping) return;


            MoveDirection = PlayerTransform.forward * InputVector.y + PlayerTransform.right * InputVector.x;

            float currentSpeed = PlayerController.IsRunning ? RunSpeed : WalkSpeed;

            Vector3 movementDirection = MoveDirection * (currentSpeed * Time.deltaTime);

          NextPositionCheck = transform.position + MoveDirection * MoveDirectionBuffer;

           if(NavMesh.SamplePosition(NextPositionCheck, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            {
                transform.position += movementDirection;
            }


        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Ground") && !PlayerController.IsJumping) return;


            //navMeshAgent.enabled = true;
            PlayerController.IsJumping = false;
            PlayerAnimator.SetBool(IsJumpingHash, false);
        }

        public void OnMovement(InputValue value)
        {
            InputVector = value.Get<Vector2>();
            
            PlayerAnimator.SetFloat(MovementXHash, InputVector.x);
            PlayerAnimator.SetFloat(MovementZHash, InputVector.y);
        }

        public void OnRun(InputValue button)
        {
            PlayerController.IsRunning = button.isPressed;
            PlayerAnimator.SetBool(IsRunningHash, button.isPressed);
        }

        public void OnJump(InputValue button)
        {
            if (PlayerController.IsJumping) return;


            //navMeshAgent.isStopped = true;
            //navMeshAgent.enabled = false;



            PlayerController.IsJumping = button.isPressed;
            PlayerAnimator.SetBool(IsJumpingHash, button.isPressed);
            PlayerRigidbody.AddForce((PlayerTransform.up + MoveDirection)* JumpForce, ForceMode.Impulse);
            navMeshAgent.enabled = false;

            //Invoke(nameof(Jump),0.1f);
            //InvokeRepeating(nameof(LandingCheck), JumpLandingCheckDelay, 0.1f);
        }

        private void LandingCheck()
        {
            if (!Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 100f, JumpLayerMask)) return;
            {
                Debug.Log(hit.distance);
                if (!(hit.distance < JumpThreshold)||  !PlayerController.IsJumping ) return;
               
                    navMeshAgent.enabled = true;
                    navMeshAgent.isStopped = false;
                    PlayerController.IsJumping = false;
                    PlayerAnimator.SetBool(IsJumpingHash, false);

                    CancelInvoke(nameof(LandingCheck));
                
            }
        }

        public void Jump()
        {
            PlayerRigidbody.AddForce((transform.up + MoveDirection) * JumpForce, ForceMode.Impulse);
        }

    }
}
