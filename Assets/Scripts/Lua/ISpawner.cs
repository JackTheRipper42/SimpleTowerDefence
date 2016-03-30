namespace Assets.Scripts.Lua
{
    public interface ISpawner
    {
        void SpawnSmallWalker();

        void SpawnBigWalker();

        void Wait(double time);

        void Debug(string message);
    }
}
