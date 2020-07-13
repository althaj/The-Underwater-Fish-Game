﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TUFG.Controls;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace TUFG
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField]
        private float speed = 5f;

        private Vector3[] groundRayOrigins = {
            new Vector3(-0.3f, 0.15f),
            new Vector3(0.3f, 0.15f)
        };
        private readonly float groundRayLength = 0.3f;
        private bool isGrounded = true;
        private float gravity = 0;
        private LayerMask groundLayer;

        private ControlsInput controlsInput;
        private Vector2 moveVector = Vector2.zero;

        private readonly float distanceToClimb = 0.33f;
        private bool isClimbing = false;
        private Vector2[] ladderBottoms;
        private Vector2[] ladderTops;
        private int currentLadderID = -1;

        private Animator playerAnimator;

        #region Unity functions
        void Awake()
        {
            controlsInput = new ControlsInput();
            controlsInput.World.Move.performed += OnMoveInput;
        }

        private void Start()
        {
            gravity = Physics2D.gravity.y;
            BuildLadderData();
            playerAnimator = GetComponentInChildren<Animator>();

            groundLayer = LayerMask.GetMask("Ground");
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
            isGrounded = IsGrounded();
            Move();
        }
        #endregion

        #region Player Movement functions
        private void Move()
        {
            Vector2 translation = Vector2.zero;
            if (isClimbing)
            {
                if(Mathf.Abs(moveVector.x) > 0.5f)
                {
                    int ladderPosition = GetCloseLadder(World.LadderType.Any);
                    if (ladderPosition >= 0)
                        StopClimbing();
                }

                if (isClimbing)
                {
                    if(moveVector.y < 0 && transform.position.y >= ladderBottoms[currentLadderID].y || moveVector.y > 0 && transform.position.y <= ladderTops[currentLadderID].y)
                    {
                        translation = Vector3.up * moveVector.y * speed * Time.fixedDeltaTime;
                    }
                }

                playerAnimator.SetFloat("Speed", translation.y);

            } else
            {
                if (moveVector.y > 0.5f)
                {
                    int ladderID = GetCloseLadder(World.LadderType.Bottom);
                    if (ladderID >= 0)
                        StartClimbing(ladderID, World.LadderType.Bottom);
                } else if (moveVector.y < -0.5f)
                {
                    int ladderID = GetCloseLadder(World.LadderType.Top);
                    if (ladderID >= 0)
                        StartClimbing(ladderID, World.LadderType.Top);
                }

                if (!isClimbing)
                    if (isGrounded)
                        translation = Vector3.right * moveVector.x * speed * Time.fixedDeltaTime;
                    else
                        translation = Vector3.up * gravity * Time.fixedDeltaTime;

                playerAnimator.SetFloat("Speed", translation.x);
            }

            transform.Translate(translation);
            playerAnimator.SetBool("IsClimbing", isClimbing);
            playerAnimator.SetBool("IsGrounded", isGrounded);
        }

        private void BuildLadderData()
        {
            World.Ladder[] ladders = FindObjectsOfType<World.Ladder>();
            ladderTops = new Vector2[ladders.Length];
            ladderBottoms = new Vector2[ladders.Length];

            Vector2 ladderPosition;

            for (int i = 0; i < ladders.Length; i++)
            {
                ladderPosition = ladders[i].transform.position;
                ladderBottoms[i] = ladderPosition;
                ladderTops[i] = ladderPosition + Vector2.up * ladders[i].height;
            }
        }

        private int GetCloseLadder(World.LadderType ladderType)
        {
            if (ladderType == World.LadderType.Top || ladderType == World.LadderType.Any)
            {
                for (int i = 0; i < ladderTops.Length; i++)
                {
                    if (Vector2.Distance(transform.position, ladderTops[i]) < distanceToClimb)
                        return i;
                }
            }

            if (ladderType == World.LadderType.Bottom || ladderType == World.LadderType.Any)
            {
                for (int i = 0; i < ladderBottoms.Length; i++)
                {
                    if (Vector2.Distance(transform.position, ladderBottoms[i]) < distanceToClimb)
                        return i;
                }
            }

            return -1;
        }

        private void StartClimbing(int ladderID, World.LadderType ladderType)
        {
            Vector2 ladderPosition = ladderType == World.LadderType.Top ? ladderTops[ladderID] : ladderBottoms[ladderID];

            isClimbing = true;
            transform.position = ladderPosition;
            currentLadderID = ladderID;
        }

        private void StopClimbing()
        {
            currentLadderID = -1;
            isClimbing = false;
        }

        public bool IsGrounded()
        {
            if (isClimbing)
                return false;

            RaycastHit2D hit;

            foreach (Vector3 groundRayOrigin in groundRayOrigins)
            {
                hit = Physics2D.Raycast(transform.position + groundRayOrigin, Vector2.down, groundRayLength, groundLayer);
                if (hit.collider != null)
                {
                    transform.position = new Vector2(transform.position.x, hit.point.y);
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region Callbacks
        private void OnMoveInput(CallbackContext ctx)
        {
            moveVector = ctx.ReadValue<Vector2>();
        }
        #endregion

    }
}