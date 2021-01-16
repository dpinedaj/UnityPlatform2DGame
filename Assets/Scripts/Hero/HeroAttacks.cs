using UnityEngine;

namespace Hero
{
    public class HeroAttacks
    {
        private readonly Hero _hc;
        private const KeyCode AttackButton = KeyCode.X;
        
        public HeroAttacks(Hero hc)
        {
            _hc = hc;
        }
        public Collider2D[] HandleAttacks()
        {
            if (_hc.attackTime <= 0)
            {
                if (Input.GetKeyDown(AttackButton))
                {
                    _hc.onAttack = true;
                    Collider2D[] enemies = Physics2D.OverlapCircleAll(_hc.attackTrans.position, _hc.attackRange, _hc.enemies);
                    _hc.attackTime = _hc.startTimeAttack;
                    return enemies;
                }
            }
            else
            {
                _hc.attackTime -= Time.deltaTime;
                _hc.onAttack = false;
            }

            return new Collider2D[] { };
        }
    }
}