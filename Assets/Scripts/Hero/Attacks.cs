using UnityEngine;


namespace Hero
{
    public class Attacks : MonoBehaviour
    {
        // Animations names
        public string onAttackName = "onAttack";

        // To perform animations
        private Animator _anim;
        public float attackTime;
        public float startTimeAttack = 0.5f;

        // Define enemies attacked and the weapon range
        public Transform attackLocation;
        public float attackRange = 0.15f;

        // Who's an enemy
        public LayerMask enemies;

        // Controls
        private const KeyCode AttackButton = KeyCode.X;

        private void Start()
        {
            _anim = GetComponent<Animator>();
        }

        private void Update()
        {
            if (attackTime <= 0)
            {
                if (Input.GetKeyDown(AttackButton))
                {
                    _anim.SetBool(onAttackName, true);
                    Collider2D[] damage = Physics2D.OverlapCircleAll(attackLocation.position, attackRange, enemies);

                    foreach (var enemy in damage)
                    {
                        Destroy(enemy.gameObject);
                    }

                    attackTime = startTimeAttack;
                }
            }
            else
            {
                attackTime -= Time.deltaTime;
                _anim.SetBool(onAttackName, false);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackLocation.position, attackRange);
        }
    }
}