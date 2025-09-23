using FPSNametagsForZlothy.Tags;
using GorillaNetworking;
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
        { "LMAPY.", "FPSNametagsForZlothy.Resources.Forestguide.png" },

        //stinky person placeholder
        { "HANSOLO", "FPSNametagsForZlothy.Resources.gouda.png" },
        //stinky above not below

        // cool people placeholders
        { "ZLOTHY", "FPSNametagsForZlothy.Resources.ZlothYLogoPurpleBoarder.png" },
        { "GRAZE", "FPSNametagsForZlothy.Resources.graze.png" },
        { "ARIEL", "FPSNametagsForZlothy.Resources.ariel.png" },
        { "AXO", "FPSNametagsForZlothy.Resources.axo.png" },
        { "DEV", "FPSNametagsForZlothy.Resources.dev.png" },

        // Cheater icon (only detects cheats that set custom props like ShibaGT Genesis)
        { "CHEATER", "FPSNametagsForZlothy.Resources.cheater.png" },

        //Pirate/CosmetX user icon
        { "PIRATE", "FPSNametagsForZlothy.Resources.pirate.png" }
    };

    private readonly Dictionary<string, Texture2D> cosmeticTextures = new();
    private readonly List<GameObject> fpIcons = new();
    private readonly List<GameObject> tpIcons = new();

    private readonly HashSet<string> zlothyPlayerIds = new HashSet<string>
    {
        "B5F9797560165521",
        "24EA3CB4A0106203",
        "376C2C7C27C0D613",
        "96A75B23C8BBB4C9"
    };

    private readonly HashSet<string> axoPlayerIds = new HashSet<string>
    {
        "5D5B4978C1300B24",
        "8E25CAA731003004"
    };

    private string hanSoloId = "A48744B93D9A3596";
    private string grazeId = "42D7D32651E93866";
    private string arielId = "C41A1A9055417A27";
    private string devId = "E354E818871BD1D8";

    private readonly HashSet<string> cheaterProps = new HashSet<string>
    {
        "ObsidianMC",
        "genesis",
        "elux",
        "VioletFreeUser",
        "VioletPaidUser",
        "Hidden Menu",
        "void",
        "6XpyykmrCthKhFeUfkYGxv7xnXpoe2",
        "cronos",
        "ORBIT",
        "Violet On Top",
        "Vivid"
    };

    private void Awake()
    {
        UIShader = Shader.Find("UI/Default");
        LoadCosmeticTextures();
    }

    private void Update()
    {
        if (rig == null)
            rig = GetComponent<VRRig>();

        var nametag = GetComponent<Nametag>();
        if (nametag != null &&
            nametag.firstPersonTag != null &&
            nametag.thirdPersonTag != null &&
            !string.IsNullOrEmpty(rig.concatStringOfCosmeticsAllowed))
        {
            CreateCosmeticIcons();
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
        foreach (var icon in fpIcons)
            if (icon != null)
                Destroy(icon);
        foreach (var icon in tpIcons)
            if (icon != null)
                Destroy(icon);

        fpIcons.Clear();
        tpIcons.Clear();

        List<string> foundCosmetics = new List<string>();

        //Owners Check
        if (zlothyPlayerIds.Contains(rig.creator.UserId))
        {
            foundCosmetics.Add("ZLOTHY");
        }
        else if (hanSoloId == rig.creator.UserId)
        {
            foundCosmetics.Add("HANSOLO");
        }
        else if (grazeId == rig.creator.UserId)
        {
            foundCosmetics.Add("GRAZE");
        }
        else if (arielId == rig.creator.UserId)
        {
            foundCosmetics.Add("ARIEL");
        }
        else if (axoPlayerIds.Contains(rig.creator.UserId))
        {
            foundCosmetics.Add("AXO");
        }
        else if (devId == rig.creator.UserId)
        {
            foundCosmetics.Add("DEV");
        }

        //Cheater Check
        if (rig.creator != null && rig.creator.GetPlayerRef().CustomProperties != null)
        {
            foreach (var prop in rig.creator.GetPlayerRef().CustomProperties)
            {
                string key = prop.Key?.ToString();
                string value = prop.Value?.ToString();

                if ((key != null && cheaterProps.Contains(key)) ||
                    (value != null && cheaterProps.Contains(value)))
                {
                    foundCosmetics.Add("CHEATER");
                    break;
                }
            }
        }

        //Pirate/CosmetX check
        CosmeticsController.CosmeticSet cosmeticSet = rig.cosmeticSet;
        foreach (CosmeticsController.CosmeticItem cosmetic in cosmeticSet.items)
        {
            if (!cosmetic.isNullItem && !rig.concatStringOfCosmeticsAllowed.Contains(cosmetic.itemName))
            {
                foundCosmetics.Add("PIRATE");
                break;
            }
        }

        //Rare Cosmetic Check
        foreach (var kvp in specialCosmetics)
        {
            //Ignore the other stuff
            if (kvp.Key == "ZLOTHY" || kvp.Key == "HANSOLO" || kvp.Key == "GRAZE" || kvp.Key == "ARIEL" ||
                kvp.Key == "AXO" || kvp.Key == "DEV" || kvp.Key == "CHEATER" || kvp.Key == "PIRATE")
                continue;

            if (rig.concatStringOfCosmeticsAllowed.Contains(kvp.Key))
            {
                foundCosmetics.Add(kvp.Key);
            }
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
            iconObj.transform.localPosition = new Vector3(startOffset + (i * spacing), 0.31f, 0f);
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