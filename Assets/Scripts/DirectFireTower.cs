namespace Assets.Scripts
{
    public class DirectFireTower : Tower<DirectFireTowerModel, DirectFireTowerLevel>
    {
        public float Damage
        {
            get { return Model.Levels[Level].Damage; }
        }

        protected override void Fire(Enemy target)
        {
            target.SetHit(Damage);
        }
    }
}
