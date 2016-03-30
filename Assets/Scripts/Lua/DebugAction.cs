namespace Assets.Scripts.Lua
{
    public class DebugAction : ISpawnerAction
    {
        public DebugAction(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }

        public override string ToString()
        {
            return string.Format("debug:'{0}'", Message);
        }
    }
}
