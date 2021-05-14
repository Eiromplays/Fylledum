using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(CharacterController))]

    public class PlayerController : MonoBehaviour
    {
        public float speed = 7.5f;
        public float maxSpeed = 50f;

        public RawImage staminaBar;
        public float sprintMultiplier = 3f;
        public float sprintLength = 10f;
        public float staminaMultiplier = 20f;
        public float staminaRegainCooldown = 1f;

        public float jumpStaminaUsage = 15f;

        public float jumpSpeed = 8.0f;
        public float gravity = 20.0f;
        public Transform playerCameraParent;
        public float lookSpeed = 2.0f;
        public float lookXLimit = 60.0f;

        CharacterController _characterController;
        Vector3 _moveDirection = Vector3.zero;
        Vector2 _rotation = Vector2.zero;

        float stamina = 100f;
        float defaultSpeed;
        float cooldownTimer;

        [HideInInspector]
        public bool canMove = true;

        private Animator _animator;

        private Rigidbody _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();

            Cursor.lockState = CursorLockMode.Locked;
            _characterController = GetComponent<CharacterController>();
            _rotation.y = transform.eulerAngles.y;

            defaultSpeed = speed;

            _animator = GetComponent<Animator>();
        }

        // ReSharper restore Unity.ExpensiveCode
        // ReSharper disable Unity.PerformanceAnalysis
        private void Update()
        {
            bool debounce = false;

            if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
            {
                _animator.SetFloat("Speed", 0f);
                debounce = true;
            }

            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            float curSpeedX = canMove ? speed * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? speed * Input.GetAxis("Horizontal") : 0;

            float currentY = _moveDirection.y;
            _moveDirection = (forward * curSpeedX) + (right * curSpeedY);
            _moveDirection.y = currentY;

            _moveDirection.x = Mathf.Clamp(_moveDirection.x, -maxSpeed, maxSpeed);
            _moveDirection.z = Mathf.Clamp(_moveDirection.z, -maxSpeed, maxSpeed);

            // Sprinting calculations
            if (Input.GetKey(KeyCode.LeftShift) && stamina > 0)
            {
                speed = defaultSpeed * sprintMultiplier;
                cooldownTimer = 0f;

                stamina -= 1f * Time.deltaTime * staminaMultiplier;
                _animator.SetFloat("Speed", 3f);
            }
            else
            {
                speed = defaultSpeed;

                cooldownTimer = Mathf.Clamp(cooldownTimer + (1f * Time.deltaTime), 0f, staminaRegainCooldown);

                stamina += (cooldownTimer == staminaRegainCooldown) ? 1f * Time.deltaTime * staminaMultiplier : 0f;
                stamina = Mathf.Clamp(stamina, 0f, 100f);

                if(!debounce) _animator.SetFloat("Speed", 2f);
            }

            //Debug.Log($"Magnitude {_rigidbody.velocity.magnitude}");

            staminaBar.transform.localScale = new Vector3(stamina / 100f, 1, 1);

            if (_characterController.isGrounded)
            {
                // We are grounded, so check for jump input

                if (Input.GetButtonDown("Jump") && canMove && stamina >= jumpStaminaUsage)
                {
                    _moveDirection.y = jumpSpeed;
                    stamina -= jumpStaminaUsage;
                    cooldownTimer = 0f;
                }
            }

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            _moveDirection.y -= gravity * Time.deltaTime;

            // Move the controller
            _characterController.Move(_moveDirection * Time.deltaTime);

            // Player and Camera rotation
            if (!canMove) return;

            _rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
            _rotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
            _rotation.x = Mathf.Clamp(_rotation.x, -lookXLimit, lookXLimit);
            playerCameraParent.localRotation = Quaternion.Euler(_rotation.x, 0, 0);
            transform.eulerAngles = new Vector2(0, _rotation.y);
        }
    }
}