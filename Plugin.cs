using System.IO;
using System.Reflection;
using Admin.Admin;
using BepInEx;
using ExitGames.Client.Photon;
using FPSNametagsForZlothy.Patches;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace FPSNametagsForZlothy;

[BepInPlugin(Constants.PluginGuid, Constants.PluginName, Constants.PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    public static Transform firstPersonCameraTransform;
    public static Transform thirdPersonCameraTransform;

    public static TMP_FontAsset comicSans;
    
    private void Start()
    {
        HarmonyPatches.ApplyHarmonyPatches();

        Hashtable properties = new();
        properties.Add("FPS-Nametags for Zlothy", "Made by HanSolo1000Falcon");
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
        
        GorillaTagger.OnPlayerSpawned(OnGameInitialized);
    }

    private void OnGameInitialized()
    {
        firstPersonCameraTransform = GorillaTagger.Instance.mainCamera.transform;
        thirdPersonCameraTransform = GorillaTagger.Instance.thirdPersonCamera.transform.GetChild(0);

        Stream stream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream("FPSNametagsForZlothy.Resources.fpsnametagsforzlothy");
        AssetBundle bundle = AssetBundle.LoadFromStream(stream);
        stream.Close();
        
        comicSans = Instantiate(bundle.LoadAsset<TMP_FontAsset>("COMICBD SDF"));
        comicSans.material.shader = Shader.Find("TextMeshPro/Mobile/Distance Field");
        
        ModRegistry.RegisterMod("ZlothY Nametags");
    }
}
