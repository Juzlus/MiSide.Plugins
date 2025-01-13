using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PhotoModeActive
{
    [HarmonyPatch(typeof(InterfaceFastMenu))]
    internal class InterfaceFastMenuPatch
    {
        [HarmonyPatch("StartComponent")]
        [HarmonyPostfix]
        static void StartComponentPostfix(InterfaceFastMenu __instance)
        {
            if (__instance != null)
            {
                if (__instance.buttonPhotomode != null && SceneManager.GetActiveScene().name != "Scene 18 - 2D")
                    __instance.buttonPhotomode.interactable = true;
            }
        }

        [HarmonyPatch("CloseMenu")]
        [HarmonyPostfix]
        static void CloseMenuPostfix()
        {
            if (SceneManager.GetActiveScene().name != "Scene 18 - 2D")
            {
                GameController gameController = GameObject.FindAnyObjectByType<GameController>();
                if (!GameObject.Find("Minigame MakeManeken(Clone)") && !GameObject.Find("TargetCamera") && !GameObject.Find("CanvasDisplay"))
                    gameController.ShowCursor(false);
                gameController.cameraPlayer.gameObject.active = true;
                gameController.cameraPlayer.depth = gameController.photomode ? 1 : -1;
            }
        }
    }
}
