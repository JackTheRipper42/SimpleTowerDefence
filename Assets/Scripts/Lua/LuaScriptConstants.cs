namespace Assets.Scripts.Lua
{
    public static class LuaScriptConstants
    {
        public const string SetupWaveFunctionName = "setupWave";
        public const string GetCashFunctionName = "getCash";
        public const string SpawnerGlobalNameSuffix = "Spawner";
        public const string DebugGlobalName = "debug";
        public static readonly char[] InvalidNameCharacters;
        public static readonly char[] DigitCharacters;

        static LuaScriptConstants()
        {
            InvalidNameCharacters = @"+-*/^=~<>(){}[];:,.#".ToCharArray();
            DigitCharacters = "0123456789".ToCharArray();
        }
    }
}
