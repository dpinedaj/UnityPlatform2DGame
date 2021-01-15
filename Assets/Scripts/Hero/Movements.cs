using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hero
{
    public class Movements : MonoBehaviour
    {
        // CONSTANTS
        public string runSpeedName = "runSpeed";
        public string jumpSpeedName = "verticalSpeed";
        public string onFloorName = "onFloor";
        
        // Objects to handle collisions and blocks
        public Transform headTrans;
        public Transform feetTrans;
        public Transform rightTrans;
        public Transform leftTrans;
        public Transform attackTrans;

        // Define variables to interact with the environment
        public LayerMask layerFloor;

        // Define variables proper from the character
        private Rigidbody2D _heroRb;
        private SpriteRenderer _heroRen;
        private Animator _heroAnim;

        // Define constants to manage Hero behavior
        private const float Speed = 2f;
        private const float JumpPower = 5f;
        private const float FloorRadius = 0.1f;

        // States variables
        private bool _onFloor;
        private bool _onAttack;
        private bool _facingRight = true;

        // Controls
        private const KeyCode JumpButton = KeyCode.Space;
        
        /*
         * The Speed and position will be managed by the rigidBody attributes
         */

        private void Start()
        {
            // Initialize Every component
            _heroRb = GetComponent<Rigidbody2D>();
            _heroRen = GetComponent<SpriteRenderer>();
            _heroAnim = GetComponent<Animator>();

            // Constraints
            _heroRb.freezeRotation = true;
        }

        // Handle Animations
        private void Flip()
        {
            _facingRight = !_facingRight;
            _heroRen.flipX = !_heroRen.flipX;
            attackTrans.localPosition = new Vector3( - attackTrans.localPosition.x, 0, 0);
        }

        private void RunAnimation(float velX)
        {
            if (_onFloor) _heroAnim.SetFloat(runSpeedName, Mathf.Abs(velX)); // Tricky on Anim greater or less 0.1f
        }

        private void JumpAnimation(float velY)
        {
            _heroAnim.SetFloat(jumpSpeedName, velY);
            _heroAnim.SetBool(onFloorName, _onFloor);
        }

        private void HandleAnimations()
        {
            // Constant sending of information to the animator and Unity decide which animation go
            RunAnimation(_heroRb.velocity.x);
            JumpAnimation(_heroRb.velocity.y);
        }

        // Handle movements
        private void MoveHorizontal(float move)
        {
            var velX = move * Speed;
            if (velX != 0) _heroRb.velocity = new Vector2(velX, _heroRb.velocity.y);
            if ((move > 0 && !_facingRight) || (move < 0 && _facingRight)) Flip();
        }

        private void Jump()
        {
            _heroRb.AddForce(new Vector2(0, JumpPower), ForceMode2D.Impulse);
            _onFloor = false;
        }

        private void HandleMovements()
        {
            // Horizontal
            var move = Input.GetAxis("Horizontal");
            MoveHorizontal(move);
            if (_onFloor && Input.GetKeyDown(JumpButton)) Jump();
        }

        // Handle Collisions and Delimiters
        private void CheckFloor()
        {
            // TODO Modify to use this just when overlaps the feet or something like
            _onFloor = Physics2D.OverlapCircle(feetTrans.position, FloorRadius, layerFloor);
        }

        private void HandleDelimiters()
        {
            CheckFloor();
        }

        // Update is called once per frame

        private void Update()
        {
            HandleDelimiters();
            HandleMovements();
            HandleAnimations();
        }
    }
}