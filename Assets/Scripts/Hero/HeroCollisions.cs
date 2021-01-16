namespace Hero
{
    using UnityEngine;
    public class HeroCollisions
    {
        private readonly Hero _hc;

        public HeroCollisions(Hero hc)
        {
            _hc = hc;
        }

        private void CheckFloor()
        {
            _hc.onFloor = Physics2D.OverlapCircle(_hc.feetTrans.position, _hc.contactRadius, _hc.layerFloor);
        }

        public void HandleDelimiters()
        {
            CheckFloor();
        }
        public void DrawCollidersRange()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_hc.feetTrans.position, _hc.contactRadius);
            Gizmos.DrawWireSphere(_hc.sideTrans.position, _hc.contactRadius);
        }
    }
}