using HarmonyLib;
using UnityEngine.SceneManagement;

namespace PhotoModeActive.Patches
{
    [HarmonyPatch(typeof(SceneStart))]
    internal class ScenePath
    {
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        public static void AwakePostfix(SceneStart __instance)
        {
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name == "SceneAihasto")
                SceneManager.LoadScene("SceneLoading");
        }
    }
}
