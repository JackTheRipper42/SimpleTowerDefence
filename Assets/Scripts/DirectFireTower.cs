namespace Assets.Scripts
{
    public class DirectFireTower : Tower<DirectFireTowerModel>
    {
        public float Damage { get; private set; }

        public override void Initialize(DirectFireTowerModel model)
        {
            Damage = model.Damage;
            base.Initialize(model);
        }

        protected override void Fire(Enemy target)
        {
            target.SetHit(Damage);
        }
    }
}
