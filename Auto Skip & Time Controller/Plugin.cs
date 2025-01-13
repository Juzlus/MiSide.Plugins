using BepInEx;
using BepInEx.Configuration;
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

        private const string pluginGuild = "juzlus.miside.autoskip";
        private const string pluginName = "Auto Skip & Time Controller";
        private const string pluginVersion = "1.0.0";

        public static BepInEx.Logging.ManualLogSource logger;

        private ConfigEntry<bool> skipIntroConfig;
        public static bool skipIntro = true;

        private ConfigEntry<bool> skipStartConfig;
        public static bool skipStart = true;

        private ConfigEntry<bool> autoContinueConfig;
        public static bool autoContinue = false;

        private ConfigEntry<string> timeScaleUpKeyConfig;
        public static KeyCode timeScaleUpKey = KeyCode.KeypadPlus;

        private ConfigEntry<string> timeScaleDownKeyConfig;
        public static KeyCode timeScaleDownKey = KeyCode.KeypadMinus;

        private ConfigEntry<string> timeScaleResetKeyConfig;
        public static KeyCode timeScaleResetKey = KeyCode.KeypadEnter;

        public override void Load()
        {
            logger = Log;
            logger.LogInfo("Plugin " + pluginGuild + " is loaded!");

            Harmony harmony = new Harmony(pluginGuild);
            if (skipIntro)
                harmony.PatchAll(typeof(ScenePath));
            harmony.PatchAll(typeof(MenuPath));
            harmony.PatchAll(typeof(ConsoleCallPath));

            skipIntroConfig = Config.Bind("Auto Skip", "SkipIntro", true, "Automatically skip the intro when the game starts.");
            skipIntro = skipIntroConfig.Value;

            skipStartConfig = Config.Bind("Auto Skip", "SkipStart", true, "Automatically skip the start screen animation.");
            skipStart = skipStartConfig.Value;

            autoContinueConfig = Config.Bind("Auto Skip", "AutoContinue", false, "Automatically continue the game from menus (or new game if the 'Continue' button is not active).");
            autoContinue = autoContinueConfig.Value;

            timeScaleUpKeyConfig = Config.Bind("Time Controller", "TimeScaleUpKey", "KeypadPlus", "Key binding to increase the time scale of the game (+0.1f).");
            timeScaleUpKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), timeScaleUpKeyConfig.Value);

            timeScaleDownKeyConfig = Config.Bind("Time Controller", "TimeScaleDownKey", "KeypadMinus", "Key binding to decrease the time scale of the game (-0.1f).");
            timeScaleDownKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), timeScaleDownKeyConfig.Value);

            timeScaleResetKeyConfig = Config.Bind("Time Controller", "TimeScaleResetKey", "KeypadEnter", "Key binding to reset the time scale of the game (=1.0f).");
            timeScaleResetKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), timeScaleResetKeyConfig.Value);

            logger.LogMessage("Skip Intro : " + skipIntro + ", Skip Start : " + skipStart + ", Auto Continue : " + autoContinue);
            logger.LogMessage("Time Scale Up Key -> " + timeScaleUpKey);
            logger.LogMessage("Time Scale Down Key -> " + timeScaleDownKey);
            logger.LogMessage("Time Scale Reset Key -> " + timeScaleResetKey);
        }
    }
}
