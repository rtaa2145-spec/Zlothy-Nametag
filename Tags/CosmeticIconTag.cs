using FPSNametagsForZlothy.Tags;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class CosmeticIconTag : MonoBehaviour
{
    private Shader UIShader;
    private VRRig rig;
    private bool iconsCreated;

    private readonly Dictionary<string, string> specialCosmetics = new Dictionary<string, string>
    {
        { "LBAAD.", "FPSNametagsForZlothy.Resources.Admin.png" },
        { "LBAAK.", "FPSNametagsForZlothy.Resources.Stick.png" },
        { "LBADE.", "FPSNametagsForZlothy.Resources.Fingerpainter.png" },
        { "LBANI.", "FPSNametagsForZlothy.Resources.AACreator.png" },
        { "LBAGS.", "FPSNametagsForZlothy.Resources.Illustrator.png" },
        { "LMAPY.", "FPSNametagsForZlothy.Resources.Forestguide.png" }
    };

    private readonly Dictionary<string, Texture2D> cosmeticTextures = new();
    private readonly List<GameObject> fpIcons = new();
    private readonly List<GameObject> tpIcons = new();

    private void Awake()
    {
        UIShader = Shader.Find("UI/Default");
        LoadCosmeticTextures();
    }

    private void Update()
    {
        if (iconsCreated) return;

        if (rig == null)
            rig = GetComponent<VRRig>();

        var nametag = GetComponent<Nametag>();
        if (nametag != null &&
            nametag.firstPersonTag != null &&
            nametag.thirdPersonTag != null &&
            !string.IsNullOrEmpty(rig.concatStringOfCosmeticsAllowed))
        {
            CreateCosmeticIcons();
            iconsCreated = true;
        }
    }

    private void LoadCosmeticTextures()
    {
        foreach (var kvp in specialCosmetics)
        {
            Texture2D tex = LoadEmbeddedImage(kvp.Value);
            if (tex != null)
                cosmeticTextures[kvp.Key] = tex;
        }
    }

    private Texture2D LoadEmbeddedImage(string resourcePath)
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath);
        if (stream == null)
        {
            Debug.LogError($"[CosmeticIconTag] Resource '{resourcePath}' not found.");
            return null;
        }

        byte[] imageData = new byte[stream.Length];
        stream.Read(imageData, 0, imageData.Length);

        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageData);
        return texture;
    }

    private void CreateCosmeticIcons()
    {
        fpIcons.Clear();
        tpIcons.Clear();

        List<string> foundCosmetics = new List<string>();
        foreach (var kvp in specialCosmetics)
        {
            if (rig.concatStringOfCosmeticsAllowed.Contains(kvp.Key))
                foundCosmetics.Add(kvp.Key);
        }

        var fpTag = GetComponent<Nametag>().firstPersonTag;
        if (fpTag != null)
            CreateIconsForTag(fpTag, fpIcons, foundCosmetics);

        var tpTag = GetComponent<Nametag>().thirdPersonTag;
        if (tpTag != null)
            CreateIconsForTag(tpTag, tpIcons, foundCosmetics);
    }

    private void CreateIconsForTag(GameObject parent, List<GameObject> iconList, List<string> cosmeticKeys)
    {
        float spacing = 0.25f;
        float startOffset = -((cosmeticKeys.Count - 1) * spacing) / 2f;

        for (int i = 0; i < cosmeticKeys.Count; i++)
        {
            if (!cosmeticTextures.TryGetValue(cosmeticKeys[i], out Texture2D tex))
                continue;

            GameObject iconObj = GameObject.CreatePrimitive(PrimitiveType.Quad);
            Destroy(iconObj.GetComponent<Collider>());

            iconObj.transform.SetParent(parent.transform);
            iconObj.transform.localPosition = new Vector3(startOffset + (i * spacing), 0.3f, 0f);
            iconObj.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            iconObj.transform.localRotation = Quaternion.identity;
            iconObj.layer = parent.layer;

            Renderer renderer = iconObj.GetComponent<Renderer>();
            renderer.material = new Material(UIShader);
            renderer.material.mainTexture = tex;

            iconList.Add(iconObj);


        }
    }
}
