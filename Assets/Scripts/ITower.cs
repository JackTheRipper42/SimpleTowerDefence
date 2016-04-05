namespace Assets.Scripts
{
    public interface ITower
    {
        string Id { get; }

        int Level { get; }

        bool CanUpgrade();

        void Upgrade();
    }
}
