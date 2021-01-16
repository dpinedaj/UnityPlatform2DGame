namespace Hero
{
    using UnityEngine;
    
    public class HeroAnimations
    {
        private readonly Hero _hc;
        private static readonly int RunSpeed = Animator.StringToHash("runSpeed");
        private static readonly int VerticalSpeed = Animator.StringToHash("verticalSpeed");
        private static readonly int OnFloor = Animator.StringToHash("onFloor");
        private static readonly int OnAttack = Animator.StringToHash("onAttack");

        public HeroAnimations(Hero hc)
        {
            _hc = hc;
        }
        private void RunAnimation(float velX)
        {
            if (_hc.onFloor) _hc.heroAnim.SetFloat(RunSpeed, Mathf.Abs(velX)); // Tricky on Anim greater or less 0.1f
        }

        private void JumpAnimation(float velY)
        {
            _hc.heroAnim.SetFloat(VerticalSpeed, velY);
            _hc.heroAnim.SetBool(OnFloor, _hc.onFloor);
        }

        private void AttackAnimation(bool attack)
        {
            _hc.heroAnim.SetBool(OnAttack, attack);
        }

        public void HandleAnimations()
        {
            // Constant sending of information to the animator and Unity decide which animation go
            RunAnimation(_hc.heroRb.velocity.x);
            JumpAnimation(_hc.heroRb.velocity.y);
            AttackAnimation(_hc.onAttack);
        }
    }
}