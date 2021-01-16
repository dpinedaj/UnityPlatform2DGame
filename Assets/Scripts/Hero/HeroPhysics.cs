namespace Hero
{
    using UnityEngine;
    public class HeroPhysics
    {
        private readonly Hero _hc;
        private const KeyCode JumpButton = KeyCode.Space;

        public HeroPhysics(Hero hc)
        {
            _hc = hc;
        }

        private void Flip()
        {
            // TODO Check if it's the class which must have this void
            _hc.facingRight = !_hc.facingRight;
            _hc.heroRen.flipX = !_hc.heroRen.flipX;
            _hc.attackTrans.localPosition = new Vector3( - _hc.attackTrans.localPosition.x, 0, 0);
            _hc.sideTrans.localPosition = new Vector3(- _hc.sideTrans.localPosition.x, 0, 0);
        }
        
        private void MoveHorizontal(float move)
        {
            var velX = move * _hc.speed;
            _hc.heroRb.velocity = new Vector2(velX, _hc.heroRb.velocity.y);
            if ((move > 0 && !_hc.facingRight) || (move < 0 && _hc.facingRight)) Flip();
        }
        private void Jump()
        {
            _hc.heroRb.AddForce(new Vector2(0, _hc.jumpPower), ForceMode2D.Impulse);
            _hc.onFloor = false;
        }

        public void HandleMovements()
        {
            // Horizontal
            var move = Input.GetAxis("Horizontal");
            if (move != 0) MoveHorizontal(move);
            // Vertical
            if (_hc.onFloor && Input.GetKeyDown(JumpButton)) Jump();
        }
        
    }
}