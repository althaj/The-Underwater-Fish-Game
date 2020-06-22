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

        private PrototypeControlsInput controlsInput;
        private float moveVector = 0f;

        void Awake()
        {
            controlsInput = new PrototypeControlsInput();
            controlsInput.World.Move.performed += OnMove;
        }

        private void OnEnable()
        {
            controlsInput.World.Move.Enable();
        }

        private void OnDisable()
        {
            controlsInput.World.Move.Disable();
        }

        void Update()
        {
            transform.Translate(Vector3.right * moveVector * speed * Time.deltaTime);
        }

        void OnMove(CallbackContext ctx)
        {
            moveVector = ctx.ReadValue<float>();
        }
    }
}