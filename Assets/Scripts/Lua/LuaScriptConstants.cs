namespace Assets.Scripts.Lua
{
    public static class LuaScriptConstants
    {
        public const string SetupWaveFunctionName = "setupWave";
        public const string SpawnerGlobalNameSuffix = "Spawner";
        public const string DebugGlobalName = "debug";
        public static char[] InvalidNameCharacters;
        public static char[] DigitCharacters;

        static LuaScriptConstants()
        {
            InvalidNameCharacters = @"+-*/^=~<>(){}[];:,.#".ToCharArray();
            DigitCharacters = "0123456789".ToCharArray();
        }
    }
}
