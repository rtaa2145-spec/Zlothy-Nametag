using HarmonyLib;
using UnityEngine;
using ZlothYNametag.Tags;

namespace ZlothYNametag.Patches;

[HarmonyPatch(typeof(VRRig), nameof(VRRig.SetColor))]
public class SetColourPatch
{
    private static void Postfix(VRRig __instance, Color color)
    {
        if (__instance.isLocal)
            return;
        
        __instance.GetOrAddComponent<Nametag>(out Nametag nametag);
        nametag.UpdateColour(color);
        
        __instance.GetOrAddComponent<FPSTag>(out _);
        __instance.GetOrAddComponent<PlatformTag>(out _);
        __instance.GetOrAddComponent<CosmeticIconTag>(out _);
    }
}