using System.Reflection;
using HarmonyLib;

namespace ZlothYNametag.Patches;

public class HarmonyPatches
{
    private const  string  instanceId = Constants.PluginGuid;
    private static Harmony harmonyInstance;

    private static bool isPatched;

    internal static void ApplyHarmonyPatches()
    {
        if (!isPatched)
        {
            if (harmonyInstance == null)
                harmonyInstance = new Harmony(instanceId);

            harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
            isPatched = true;
        }
    }

    internal static void RemoveHarmonyPatches()
    {
        if (harmonyInstance != null && isPatched)
        {
            harmonyInstance.UnpatchSelf();
            isPatched = false;
        }
    }
}