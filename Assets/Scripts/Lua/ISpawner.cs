using MoonSharp.Interpreter;

namespace Assets.Scripts.Lua
{
    public interface ISpawner
    {
        void Wait(double time);

        void Debug(string message);

        void Spawn(Table table);
    }
}
