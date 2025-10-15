using System.IO;
using System.Reflection;
using BepInEx;
using ExitGames.Client.Photon;
using Photon.Pun;
using TMPro;
using UnityEngine;
using ZlothYNametag.Patches;

namespace ZlothYNametag;

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
        properties.Add("FPS-Nametags for Zlothy",
                $"Made by HanSolo1000Falcon & ZlothY - Version {Constants.PluginVersion}");

        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);

        GorillaTagger.OnPlayerSpawned(OnGameInitialized);
    }

    private void OnGameInitialized()
    {
        firstPersonCameraTransform = GorillaTagger.Instance.mainCamera.transform;
        thirdPersonCameraTransform = GorillaTagger.Instance.thirdPersonCamera.transform.GetChild(0);

        Stream stream = Assembly.GetExecutingAssembly()
                                .GetManifestResourceStream("ZlothYNametag.Resources.fpsnametagsforzlothy");

        AssetBundle bundle = AssetBundle.LoadFromStream(stream);
        stream.Close();

        comicSans                 = Instantiate(bundle.LoadAsset<TMP_FontAsset>("COMICBD SDF"));
        comicSans.material.shader = Shader.Find("TextMeshPro/Mobile/Distance Field");
    }
}