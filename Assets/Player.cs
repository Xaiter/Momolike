using UnityEngine;
using System.Collections;

namespace Momolike
{

    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour
    {
        // Private Fields
        private bool _isSprinting = false;
        private float _velocity;
        private float verticalRotation = 0;
        private float verticalVelocity = 0;
        CharacterController characterController;



        // Public Fields
        public float movementSpeed = 5.0f;
        public float mouseSensitivity = 5.0f;
        public float jumpSpeed = 1.0f;
        public float upDownRange = 15.0f;



        // Methods
        void Start()
        {
            Screen.lockCursor = true;
            characterController = GetComponent<CharacterController>();
        }

        void Update()
        {
            // Rotation
            float rotLeftRight = Input.GetAxis("Mouse X") * mouseSensitivity;
            transform.Rotate(0, rotLeftRight, 0);


            verticalRotation += Input.GetAxis("Mouse Y") * mouseSensitivity;
            verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
            Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);


            // Movement
            float forwardSpeed = Input.GetAxis("Vertical") * movementSpeed;
            float sideSpeed = Input.GetAxis("Horizontal") * movementSpeed;

            verticalVelocity += Physics.gravity.y * Time.deltaTime;

            if (characterController.isGrounded && Input.GetButton("Jump"))
            {
                verticalVelocity = jumpSpeed;
            }

            if (forwardSpeed == 0)
                _isSprinting = false;
            else if (characterController.isGrounded && Input.GetKey(KeyCode.LeftShift))
                _isSprinting = true;

            if (_isSprinting)
                forwardSpeed = forwardSpeed * 1.25f;

            _velocity = forwardSpeed + sideSpeed / 2;

            var speed = new Vector3(sideSpeed, verticalVelocity, forwardSpeed);
            speed = transform.rotation * speed;
            characterController.Move(speed * Time.deltaTime);
        }

        void OnGUI()
        {
            //GUI.Box(new Rect(50, 50, 150, 20), "Speed:" + _velocity.ToString("0.000"));
        }
    }
}