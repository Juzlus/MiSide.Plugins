using HarmonyLib;
using System;
using UnityEngine;

namespace PhotoModeActive.Patches
{
    [HarmonyPatch(typeof(ConsoleCall))]
    internal class ConsoleCallPath
    {
        private static float timeScale = 1.0f;
        private static float maxTimeScale = 10f;

        private static float cooldown = 0.2f;
        private static float cooldownTimer = 0f;

        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        static void UpdatePrefix()
        {
            if (cooldownTimer > 0f)
                cooldownTimer -= Time.unscaledDeltaTime;

            if (cooldownTimer <= 0f)
            {
                if (Input.GetKey(Plugin.timeScaleUpKey) && timeScale < maxTimeScale)
                    ChangeTimeScale(0.1f);
                else if (Input.GetKey(Plugin.timeScaleDownKey) && timeScale >= 0.1f)
                    ChangeTimeScale(-0.1f);
                else if (Input.GetKey(Plugin.timeScaleResetKey))
                    ChangeTimeScale(1f);
            }
        }

        static void ChangeTimeScale(float x)
        {
            cooldownTimer = cooldown;
            timeScale = (float)Math.Round(x == 1f ? 1 : (x + timeScale), 1);
            Plugin.logger.LogInfo("Timescale changed : " + Time.timeScale + " -> " + timeScale);
            Time.timeScale = timeScale;
        }
    }
}
