namespace Assets.Scripts.Lua
{
    public interface IDebugger
    {
        void Log(string message);
        void LogError(string message);
        void LogWarning(string message);
    }
}