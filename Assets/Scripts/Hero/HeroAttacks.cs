using System.Linq;

namespace Hero
{
    using UnityEngine;
    public class HeroAttacks
    {
        private readonly Hero _hc;
        private const KeyCode AttackButton = KeyCode.X;
        public GameObject[] DestroyedEnemies;
        
        public HeroAttacks(Hero hc)
        {
            _hc = hc;
        }
        public void HandleAttacks()
        {
            if (_hc.attackTime <= 0)
            {
                if (Input.GetKeyDown(AttackButton))
                {
                    _hc.onAttack = true;
                    Collider2D[] damage = Physics2D.OverlapCircleAll(_hc.attackTrans.position, _hc.attackRange, _hc.enemies);
                    _hc.attackTime = _hc.startTimeAttack;
                    DestroyedEnemies = damage.Select(enemy => enemy.gameObject).ToArray();
                }
            }
            else
            {
                _hc.attackTime -= Time.deltaTime;
                _hc.onAttack = false;
            }
        }
        public void DrawAttackRange()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_hc.attackTrans.position, _hc.attackRange);
        }
    }
}