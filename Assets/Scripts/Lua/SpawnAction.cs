namespace Assets.Scripts.Lua
{
    public class SpawnAction : ISpawnerAction
    {
        public SpawnAction(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }

        public override string ToString()
        {
            return string.Format("spawn {0}", Id);
        }
    }
}