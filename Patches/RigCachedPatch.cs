using FPSNametagsForZlothy.Tags;
using HarmonyLib;
using UnityEngine;

namespace FPSNametagsForZlothy.Patches;

[HarmonyPatch(typeof(VRRigCache), nameof(VRRigCache.RemoveRigFromGorillaParent))]
public class RigCachedPatch
{
    private static void Postfix(NetPlayer player, VRRig vrrig)
    {
        Object.Destroy(vrrig.GetComponent<FPSTag>());
        Object.Destroy(vrrig.GetComponent<PlatformTag>());
        Object.Destroy(vrrig.GetComponent<Nametag>());
        Object.Destroy(vrrig.GetComponent<CosmeticIconTag>());
    }
}