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
        [SerializeField]
        private float speed = 5f;

        private Vector3[] groundRayOrigins = {
            new Vector3(-0.3f, 0.15f),
            new Vector3(0.3f, 0.15f)
        };
        private readonly float groundRayLength = 0.3f;
        private bool isGrounded = true;
        private float gravity = 0;

        private PrototypeControlsInput controlsInput;
        private Vector2 moveVector = Vector2.zero;

        private readonly float distanceToClimb = 0.33f;
        private bool isClimbing = false;
        private Vector2[] ladderBottoms;
        private Vector2[] ladderTops;
        private int currentLadderID = -1;

        #region Unity functions
        void Awake()
        {
            controlsInput = new PrototypeControlsInput();
            controlsInput.World.Move.performed += OnMoveInput;
        }

        private void Start()
        {
            gravity = Physics2D.gravity.y;
            BuildLadderData();
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
            if (isClimbing)
            {
                if(moveVector.x != 0)
                {
                    int ladderPosition = GetCloseLadder(World.LadderType.Any);
                    if (ladderPosition >= 0)
                        StopClimbing();
                }

                if (isClimbing)
                {
                    if(moveVector.y < 0 && transform.position.y >= ladderBottoms[currentLadderID].y || moveVector.y > 0 && transform.position.y <= ladderTops[currentLadderID].y)
                    {
                        transform.Translate(Vector3.up * moveVector.y * speed * Time.fixedDeltaTime);
                    }
                }

            } else
            {
                if (moveVector.y > 0)
                {
                    int ladderID = GetCloseLadder(World.LadderType.Bottom);
                    if (ladderID >= 0)
                        StartClimbing(ladderID, World.LadderType.Bottom);
                } else if (moveVector.y < 0)
                {
                    int ladderID = GetCloseLadder(World.LadderType.Top);
                    if (ladderID >= 0)
                        StartClimbing(ladderID, World.LadderType.Top);
                }

                if(!isClimbing)
                    if (isGrounded)
                        transform.Translate(Vector3.right * moveVector.x * speed * Time.fixedDeltaTime);
                    else
                        transform.Translate(Vector3.up * gravity * Time.fixedDeltaTime);

            }
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
                hit = Physics2D.Raycast(transform.position + groundRayOrigin, Vector2.down, groundRayLength);
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