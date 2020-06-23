using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TUFG.Prototype.Controls;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace TUFG.Prototype
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;

        private Vector3[] groundRayOrigins = {
            new Vector3(-0.3f, 0.15f),
            new Vector3(0.3f, 0.15f)
        };
        private float groundRayLength = 0.3f;
        private bool isGrounded = true;
        private float gravity = 0;

        private PrototypeControlsInput controlsInput;
        private float moveVector = 0f;

        void Awake()
        {
            controlsInput = new PrototypeControlsInput();
            controlsInput.World.Move.performed += OnMove;
        }

        private void Start()
        {
            gravity = Physics2D.gravity.y;
        }

        private void OnEnable()
        {
            controlsInput.World.Move.Enable();
        }

        private void OnDisable()
        {
            controlsInput.World.Move.Disable();
        }

        void FixedUpdate()
        {
            isGrounded = false;
            RaycastHit2D hit;

            foreach(Vector3 groundRayOrigin in groundRayOrigins)
            {
                hit = Physics2D.Raycast(transform.position + groundRayOrigin, Vector2.down, groundRayLength);
                if (hit.collider != null)
                {
                    isGrounded = true;
                    transform.position = new Vector2(transform.position.x, hit.point.y);
                    break;
                }
            }

            if(isGrounded)
                transform.Translate(Vector3.right * moveVector * speed * Time.fixedDeltaTime);
            else
                transform.Translate(Vector3.up * gravity * Time.fixedDeltaTime);
        }

        void OnMove(CallbackContext ctx)
        {
            moveVector = ctx.ReadValue<float>();
        }
    }
}