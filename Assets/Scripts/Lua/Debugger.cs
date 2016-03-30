using UnityEngine;

namespace Assets.Scripts.Lua
{
    public class Debugger : IDebugger
    {
        public void Log(string message)
        {
            Debug.Log(message);
        }

        public void LogError(string message)
        {
            Debug.LogError(message);
        }

        public void LogWarning(string message)
        {
            Debug.LogWarning(message);
        }
    }
}
