using UnityEngine;

namespace Hero
{
    public class Hero: MonoBehaviour
    {
        /*
         * This class must instantiate the elements via Inspector and must handle the GameObjects behavior
         */
        // External Game Objects
        public LayerMask layerFloor;
        public LayerMask enemies;
        
        public Transform headTrans;
        public Transform feetTrans;
        public Transform sideTrans;
        public Transform attackTrans; 
        // Internal Game Objects
        public Animator heroAnim;
        public Rigidbody2D heroRb;
        public SpriteRenderer heroRen;
        
        public bool onFloor;
        public bool facingRight = true;
        public float attackTime;
        public bool onAttack;
        
        // Private constants
        public float speed = 2f;
        public float jumpPower = 5f;
        public float contactRadius = 0.005f;
        public float startTimeAttack = 0.5f;
        public float attackRange = 0.15f;
        
        
        private HeroPhysics _hP;
        private HeroCollisions _hC;
        private HeroAnimations _hA;
        private HeroAttacks _hT;

        

        private void Start()
        {
            // Initialize Every component
            heroRb = GetComponent<Rigidbody2D>();
            heroRen = GetComponent<SpriteRenderer>();
            heroAnim = GetComponent<Animator>();
            // Instantiate handlers classes
            _hP = new HeroPhysics(this);
            _hC = new HeroCollisions(this);
            _hA = new HeroAnimations(this);
            _hT = new HeroAttacks(this);

        }
        
        private void Update()
        {
         _hP.HandleMovements();
         _hC.HandleDelimiters();
         _hA.HandleAnimations();
         _hT.HandleAttacks();

         foreach (var enemy in _hT.DestroyedEnemies)
         {
             Destroy(enemy);
         }

        }

        private void OnDrawGizmosSelected()
        {
            _hC.DrawCollidersRange();
            _hT.DrawAttackRange();
            
        }
    }
}