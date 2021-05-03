using UnityEngine;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(CharacterController))]

    public class PlayerController : MonoBehaviour
    {
        public float speed = 7.5f;
        public float jumpSpeed = 8.0f;
        public float gravity = 20.0f;
        public Transform playerCameraParent;
        public float lookSpeed = 2.0f;
        public float lookXLimit = 60.0f;

        CharacterController _characterController;
        Vector3 _moveDirection = Vector3.zero;
        Vector2 _rotation = Vector2.zero;

        [HideInInspector]
        public bool canMove = true;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            _characterController = GetComponent<CharacterController>();
            _rotation.y = transform.eulerAngles.y;
        }

        // ReSharper restore Unity.ExpensiveCode
        // ReSharper disable Unity.PerformanceAnalysis
        private void Update()
        {
            if (_characterController.isGrounded)
            {
                // We are grounded, so recalculate move direction based on axes
                Vector3 forward = transform.TransformDirection(Vector3.forward);
                Vector3 right = transform.TransformDirection(Vector3.right);
                float curSpeedX = canMove ? speed * Input.GetAxis("Vertical") : 0;
                float curSpeedY = canMove ? speed * Input.GetAxis("Horizontal") : 0;
                _moveDirection = (forward * curSpeedX) + (right * curSpeedY);

                if (Input.GetButton("Jump") && canMove)
                {
                    _moveDirection.y = jumpSpeed;
                }
            }

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            _moveDirection.y -= gravity * Time.deltaTime;

            // Move the controller
            _characterController.Move(_moveDirection * Time.deltaTime);

            // Player and Camera rotation
            if (canMove)
            {
                _rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
                _rotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
                _rotation.x = Mathf.Clamp(_rotation.x, -lookXLimit, lookXLimit);
                playerCameraParent.localRotation = Quaternion.Euler(_rotation.x, 0, 0);
                transform.eulerAngles = new Vector2(0, _rotation.y);
            }
        }
    }
}