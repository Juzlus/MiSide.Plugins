using HarmonyLib;
using System;
using System.Linq;
using UnityEngine;

namespace PhotoModeActive.Patches
{
    [HarmonyPatch(typeof(PlayerMove))]
    internal class PlayerMovePath
    {
        private static GameObject plane;
        private static bool isReset = true;
        private static float cooldownJump = 3f;
        private static float cooldownTimer;

        private static string planeName = "JumpGround";

        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        static void AwakePostfix(PlayerMove __instance)
        {
            plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            plane.transform.parent = __instance.transform;
            plane.name = planeName;
            plane.transform.localPosition = new Vector3(0f, 0f, -0.05f);
            plane.transform.localScale = new Vector3(0.04f, 5f, 0.04f);

            plane.AddComponent<ObjectMaterial>();

            BoxCollider boxCollider = plane.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;

            Rigidbody rb = plane.AddComponent<Rigidbody>();
            rb.isKinematic = true;

            MeshRenderer meshRenderer = plane.GetComponent<MeshRenderer>();
            meshRenderer.castShadows = false;

            GameObject.Destroy(plane.GetComponent<MeshFilter>());
            GameObject.Destroy(plane.GetComponent<MeshRenderer>());
            GameObject.Destroy(plane.GetComponent<MeshCollider>());
        }

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        static void UpdatePostfix(PlayerMove __instance)
        {
            __instance.canRun = true;
            __instance.canSit = true;

            Collider isGrounded = null;
            if (plane)
            {
                Collider[] hitColliders = Physics.OverlapBox(plane.transform.position, new Vector3(0.1f, 0.1f, 0.1f));
                isGrounded = hitColliders.FirstOrDefault(c => c.gameObject.name != planeName && c.gameObject.name != "Player" && c.gameObject.layer == 0);
            }

            if (Input.GetKeyDown(Plugin.infinityJumpsKey))
            {
                Plugin.logSource.LogInfo("Infinity jumps changed : " + Plugin.infinityJumps + " -> " + !Plugin.infinityJumps);
                Plugin.infinityJumps = !Plugin.infinityJumps;
            }

            if (isReset)
            {
                if (plane)
                    plane.SetActive(false);
                if (isGrounded && cooldownTimer <= 0f)
                    isReset = false;
                cooldownTimer -= Time.deltaTime;
            } else
            {
                if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || Plugin.infinityJumps))
                    __instance.rb.AddForce(Vector3.up * Plugin.jumpForce, ForceMode.Impulse);

                if (plane)
                    if (!isGrounded)
                        plane.SetActive(true);
                    else
                        plane.SetActive(false);
            }
        }

        [HarmonyPatch("TeleportPlayer")]
        [HarmonyPostfix]
        [HarmonyPatch(new Type[] { typeof(Vector3), typeof(float), typeof(float) })]
        static void TeleportPlayerPostfix(Vector3 _position, float _rotation, float _rotationHead)
        {
            isReset = true;
            cooldownTimer = cooldownJump;
        }
    }
}
