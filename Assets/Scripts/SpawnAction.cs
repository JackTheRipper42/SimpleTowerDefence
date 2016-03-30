namespace Assets.Scripts
{
    public class SpawnAction : ISpawnerAction
    {
        public SpawnAction(EnemyId id)
        {
            Id = id;
        }

        public EnemyId Id { get; private set; }

        public override string ToString()
        {
            return string.Format("spawn {0}", Id);
        }
    }
}