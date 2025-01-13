using HarmonyLib;
using UnityEngine;

namespace PhotoModeActive.Patches
{
    [HarmonyPatch(typeof(PhotoModeMain))]
    internal class PhotoModeMainPath
    {
        [HarmonyPatch("StartComponent")]
        [HarmonyPrefix]
        static void StartComponentePrefix(PhotoModeMain __instance, GameController _scrgc)
        {
            __instance.enabled = false;

            GameObject consoleOpen = GameObject.Find("ConsoleOpen");
            if (consoleOpen)
            {
                Transform cameraSafeTransform = consoleOpen.transform.Find("CameraPlayer");
                if (cameraSafeTransform?.gameObject)
                    cameraSafeTransform.gameObject.SetActive(false);
            }
        }
    }
}
