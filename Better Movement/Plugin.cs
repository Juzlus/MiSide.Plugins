using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using PhotoModeActive.Patches;
using UnityEngine;

namespace PhotoModeActive
{
    [BepInPlugin(pluginGuild, pluginName, pluginVersion)]
    [BepInProcess(gameName)]
    public class Plugin : BasePlugin
    {
        private const string gameName = "MiSideFull.exe";

        private const string pluginGuild = "juzlus.miside.bettermovement";
        private const string pluginName = "Better Movement";
        private const string pluginVersion = "1.0.0";

        private ConfigEntry<bool> infinityJumpsConfig;
        public static bool infinityJumps = false;

        private ConfigEntry<string> infinityJumpsKeyConfig;
        public static KeyCode infinityJumpsKey = KeyCode.F8;

        private ConfigEntry<float> jumpForceConfig;
        public static float jumpForce = 50f;

        public static ManualLogSource logSource;

        public override void Load()
        {
            logSource = Log;
            logSource.LogInfo("Plugin " + pluginGuild + " is loaded!");

            Harmony harmony = new Harmony(pluginGuild);
            harmony.PatchAll(typeof(PlayerMovePath));

            infinityJumpsConfig = Config.Bind("General", "InfinityJumps", false, "Infinite jumps enabled at startup.");
            infinityJumps = infinityJumpsConfig.Value;

            infinityJumpsKeyConfig = Config.Bind("General", "InfinityJumpsKey", "F8", "Key bind to toggle the infinite jumps feature.");
            infinityJumpsKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), infinityJumpsKeyConfig.Value);

            jumpForceConfig = Config.Bind("General", "JumpForce", 50f, "Sets the force applied when jumping.");
            jumpForce = jumpForceConfig.Value;

            logSource.LogMessage("Infinity Jumps : " + infinityJumps + ", Jump Force : " + jumpForce);
            logSource.LogMessage("Infinity Jumps Key -> " + infinityJumpsKey);
        }
    }
}
