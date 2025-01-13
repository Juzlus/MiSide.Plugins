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

        private const string pluginGuild = "juzlus.miside.photomodeactive";
        private const string pluginName = "PhotoMode Always Active";
        private const string pluginVersion = "1.0.0";

        private ConfigEntry<string> photoModeKeyConfig;
        public static KeyCode photoModeKey = KeyCode.V;

        public override void Load()
        {
            Log.LogInfo("Plugin " + pluginGuild + " is loaded!");

            Harmony harmony = new Harmony(pluginGuild);
            harmony.PatchAll(typeof(InterfaceFastMenuPatch));
            harmony.PatchAll(typeof(PhotoModeMainPath));
            harmony.PatchAll(typeof(GameControllerPath));

            photoModeKeyConfig = Config.Bind("General", "PhotoModeKey", "V", "Key for the quick activation of the photo mode.");
            photoModeKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), photoModeKeyConfig.Value);

            Log.LogMessage("Fast Photo Mode -> " + photoModeKey);
        }
    }
}
