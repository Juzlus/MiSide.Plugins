using HarmonyLib;

namespace PhotoModeActive.Patches
{
    [HarmonyPatch(typeof(Menu))]
    internal class MenuPath
    {
        private static bool isFirst = true;

        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        static void StartPostfix(Menu __instance)
        {
            if (isFirst && Plugin.autoContinue)
                try
                {
                    __instance.ButtonContinue();
                }
                catch
                {
                    __instance.ButtonNewGame();
                }
            isFirst = false;
        }

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        static void UpdatePostfix(Menu __instance)
        {
            if (!__instance.skiped && Plugin.skipStart)
                __instance.SkipStart();
        }
    }
}
