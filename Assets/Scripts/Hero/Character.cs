using System.Collections.Generic;
using UnityEngine;

namespace Hero
{

    public class Character: MonoBehaviour
    {
        // CONSTANTS
        [Header("Animations")] [Space] public string runSpeedName = "runSpeed";
        public string jumpSpeedName = "verticalSpeed";
        public string onFloorName = "onFloor";
        public string onAttackName = "onAttack";
        public string attackCountName = "attackCount";

        // Define variables proper from the character
        [Header("Character Attributes")] [Space]
        public Rigidbody2D heroRb;
        public SpriteRenderer heroRen;
        public Collider2D heroColl;
        public Animator heroAnim;

        // Objects to handle collisions and blocks
        [Header("States Handlers")] [Space] 
        public Transform headTrans;
        public Transform feetTrans;
        public Transform rightTrans;
        public Transform leftTrans;

        // Define variables to interact with the environment
        public LayerMask layerFloor;

        // Define constants to manage Hero behavior
        [Header("Movement Values")] [Space] [Range(0f, 10f)]
        public float speed = 2f;

        [Range(0f, 30f)] public float jumpPower = 5f;

        // States variables
        [Header("Validation")] [Space] [SerializeField]
        private bool onFloor;

        [SerializeField] private bool onAttack;
        [SerializeField] private int attackCount = 1;
        [SerializeField] private bool facingRight = true;
        [SerializeField] private float floorRadius = 0.1f;
        private float _attackDelay;
        
        // Controls
        private const KeyCode JumpButton = KeyCode.Space;
        private const KeyCode AttackButton = KeyCode.X;
        
        // Weapons
        public string currentWeapon;
        private Dictionary<string, Dictionary<string, int>> _weapons;
        
        /*
         * The speed and position will be managed by the rigidBody attributes
         */

        private void Start()
        {
            // Initialize Every component
            heroRb.GetComponent<Rigidbody2D>();
            heroRen.GetComponent<SpriteRenderer>();
            heroColl.GetComponent<Collider2D>();
            heroAnim.GetComponent<Animator>();

            // Constraints
            heroRb.freezeRotation = true;
        }

        // Handle Animations
        private void Flip()
        {
            facingRight = !facingRight;
            heroRen.flipX = !heroRen.flipX;
        }

        private void RunAnimation(float velX)
        {
            if (onFloor) heroAnim.SetFloat(runSpeedName, Mathf.Abs(velX)); // Tricky on Anim greater or less 0.1f
        }

        private void JumpAnimation(float velY)
        {
            heroAnim.SetFloat(jumpSpeedName, velY);
            heroAnim.SetBool(onFloorName, onFloor);
        }

        private void AttackAnimation()
        {    
            // TODO Set animation in the UI
            heroAnim.SetInteger(attackCountName, attackCount);
            heroAnim.SetBool(onAttackName, onAttack);
        }

        private void HandleAnimations()
        {
            // Constant sending of information to the animator and Unity decide which animation go
            RunAnimation(heroRb.velocity.x);
            JumpAnimation(heroRb.velocity.y);
            AttackAnimation();
        }

        // Handle movements
        private void MoveHorizontal(float move)
        {
            var velX = move * speed;
            if (velX != 0) heroRb.velocity = new Vector2(velX, heroRb.velocity.y);
            if (move > 0 && !facingRight) Flip();
            else if (move < 0 && facingRight) Flip();
        }

        private void Jump()
        {
            heroRb.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
            onFloor = false;
        }
        
        private void HandleMovements()
        {
            // Horizontal
            var move = Input.GetAxis("Horizontal");
            MoveHorizontal(move);
            if (onFloor && Input.GetKeyDown(JumpButton)) Jump();
        }
        
        // Handle Attacks
        private void Attack()
        {
            onAttack = true;
        }

        private void StopAttack()
        {
            onAttack = false;
            attackCount = attackCount == 3 ? 1 : attackCount + 1;
        }
        
        private void HandleAttacks()
        {
            if (Input.GetKey(AttackButton))
            {
                Attack();
            }

            if (Input.GetKeyUp(AttackButton) && onAttack)
            {
                StopAttack();
            }
        }



        // Handle Collisions and Delimiters
        private void CheckFloor()
        {
            // TODO Modify to use this just when overlaps the feet or something like
            onFloor = Physics2D.OverlapCircle(feetTrans.position, floorRadius, layerFloor);
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
            HandleAttacks();
        }
    }
}