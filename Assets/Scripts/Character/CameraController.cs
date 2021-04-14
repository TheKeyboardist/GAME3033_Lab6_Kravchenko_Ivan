using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private GameObject FollowTarget;
        [SerializeField] private float RotationSpeed = 1f;
        [SerializeField] private float HorizontalDamping = 1f;


        private Transform FollowTargetTransform;

        private Vector2 PreviousMouseInput;

        // Start is called before the first frame update
        private void Start()
        {
            FollowTargetTransform = FollowTarget.transform;
            PreviousMouseInput = Vector2.zero;
        }

        public void OnLook(InputValue delta)
        {
            Vector2 aimValue = delta.Get<Vector2>();

            FollowTargetTransform.rotation *=
                Quaternion.AngleAxis(
                    Mathf.Lerp(PreviousMouseInput.x, aimValue.x, 1f / HorizontalDamping) * RotationSpeed,
                    transform.up
                );

            PreviousMouseInput = aimValue;

            transform.rotation = Quaternion.Euler(0 , FollowTargetTransform.transform.rotation.eulerAngles.y, 0);
            
            FollowTargetTransform.localEulerAngles = Vector3.zero;

          
        }
    }
}
