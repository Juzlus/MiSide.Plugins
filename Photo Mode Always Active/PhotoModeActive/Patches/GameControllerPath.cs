using HarmonyLib;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PhotoModeActive.Patches
{
    [HarmonyPatch(typeof(GameController))]
    internal class GameControllerPath
    {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        static void UpdatePostfix(GameController __instance)
        {
            if (Input.GetKeyDown(Plugin.photoModeKey) && SceneManager.GetActiveScene().name != "Scene 18 - 2D")
            {
                GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>();
                GameObject[] players = gameObjects.Where(p => p.name == "Player").ToArray();
                GameObject[] otherPlayers = players?.Where(p => p.scene.name != "DontDestroyOnLoad").ToArray();

                foreach (GameObject otherPlayer in otherPlayers)
                    otherPlayer.name = "Player2";

                if (__instance.cameraPlayer)
                {
                    __instance.cameraPlayer.gameObject.active = true;
                    __instance.cameraPlayer.depth = __instance.photomode ? -1 : 1;
                }

                try
                {
                    if (__instance.photomode)
                    {
                        if (!GameObject.Find("Minigame MakeManeken(Clone)") && !GameObject.Find("TargetCamera") && !GameObject.Find("CanvasDisplay"))
                            __instance.ShowCursor(false);
                        __instance.photomodeObject.GetComponent<PhotoModeMain>().ExitPhotomode();
                        __instance.fastMenuObject.GetComponent<InterfaceFastMenu>().CloseMenu();
                    }
                    else
                    {
                        __instance.PhotomodeActive(true);
                    }
                }
                catch { }
            }
        }
    }
}
