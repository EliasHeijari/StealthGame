using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EvolveGames
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("PlayerController")]
        [SerializeField, Range(1, 10)] float walkingSpeed = 3.0f;
        [SerializeField, Range(2, 20)] float runningSpeed = 4.0f;
        [SerializeField, Range(0.5f, 10)] float lookSpeed = 2.0f;
        [SerializeField, Range(10, 120)] float lookXLimit = 80.0f;
        [Space(20)]
        [Header("Advance")]
        [SerializeField] float RunningFOV = 65.0f;
        [SerializeField] float SpeedToFOV = 4.0f;
        [SerializeField] float gravity = 20.0f;
        [SerializeField] float timeToRunning = 2.0f;
        [HideInInspector] public bool canMove = true;
        [HideInInspector] public bool CanRunning = true;

        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Vector3 moveDirection = Vector3.zero;
        float rotationX = 0;
        [HideInInspector] public bool isRunning = false;
        float InstallFOV;
        [SerializeField] private Camera cam;
        [HideInInspector] public bool Moving;
        [HideInInspector] public float vertical;
        [HideInInspector] public float horizontal;
        [HideInInspector] public float Lookvertical;
        [HideInInspector] public float Lookhorizontal;
        float RunningValue;
        [HideInInspector] public float WalkingValue;
        private bool isGrounded = false;

        void Start()
        {
            characterController = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            InstallFOV = cam.fieldOfView;
            RunningValue = runningSpeed;
            WalkingValue = walkingSpeed;
        }

        void Update()
        {
            updateMovement();
        }

        void updateMovement()
        {
            // Perform raycast to check if the player is grounded
            isGrounded = Physics.Raycast(transform.position, -Vector3.up, characterController.height / 2 + 0.1f);

            if (!characterController.isGrounded)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            isRunning = CanRunning ? Input.GetKey(KeyCode.LeftShift) : false;
            vertical = canMove ? (isRunning ? RunningValue : WalkingValue) * Input.GetAxisRaw("Vertical") : 0;
            //horizontal = canMove ? (isRunning ? RunningValue : WalkingValue) * Input.GetAxisRaw("Horizontal") : 0;
            if (isRunning) RunningValue = Mathf.Lerp(RunningValue, runningSpeed, timeToRunning * Time.deltaTime);
            else RunningValue = WalkingValue;
            float movementDirectionY = moveDirection.y;

            // Modify the movement calculation to ensure it's normalized
            Vector3 desiredMove = (forward * vertical) + (right * horizontal);
            desiredMove = Vector3.ClampMagnitude(desiredMove, 1.0f); // Normalize the desired movement vector
            moveDirection = desiredMove * (isRunning ? RunningValue : WalkingValue);


            moveDirection.y = movementDirectionY;

            characterController.Move(moveDirection * Time.deltaTime);
            Moving = horizontal < 0 || vertical < 0 || horizontal > 0 || vertical > 0 ? true : false;

            if (Cursor.lockState == CursorLockMode.Locked && canMove)
            {
                Lookvertical = -Input.GetAxisRaw("Mouse Y");
                Lookhorizontal = Input.GetAxisRaw("Mouse X");

                rotationX += Lookvertical * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                transform.rotation *= Quaternion.Euler(0, Lookhorizontal * lookSpeed, 0);

                if (isRunning && Moving) cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, RunningFOV, SpeedToFOV * Time.deltaTime);
                else cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, InstallFOV, SpeedToFOV * Time.deltaTime);
            }
        }
    }
}
