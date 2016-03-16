namespace Assets.Scripts
{
    public class DirectFireTower : Tower
    {
        public float Damage = 0.3f;

        protected override void Fire(Enemy target)
        {
            target.SetHit(Damage);
        }
    }
}
