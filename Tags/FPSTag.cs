using System.Collections;
using TMPro;
using UnityEngine;

namespace FPSNametagsForZlothy.Tags;

public class FPSTag : MonoBehaviour
{
    private GameObject firstPersonTag;
    private GameObject thirdPersonTag;

    private TextMeshPro firstPersonTagText;
    private TextMeshPro thirdPersonTagText;

    private VRRig rig;

    private IEnumerator DelayedStart()
    {
        while (GetComponent<Nametag>() == null)
            yield return null;

        CreateNametags();
    }

    private void CreateNametags()
    {
        CreateNametag(ref firstPersonTag, ref firstPersonTagText, "FirstPersonTag", "FirstPersonOnly", false);
        CreateNametag(ref thirdPersonTag, ref thirdPersonTagText, "ThirdPersonTag", "MirrorOnly", true);
    }

    private void CreateNametag(ref GameObject tagObj, ref TextMeshPro tagText, string name, string layerName, bool isThirdPerson)
    {
        tagObj = new GameObject(name);
        tagObj.transform.SetParent(isThirdPerson ? GetComponent<Nametag>().thirdPersonTag.transform : GetComponent<Nametag>().firstPersonTag.transform);
        tagObj.transform.localPosition = new Vector3(0f, 0.15f, 0f);

        tagObj.layer = LayerMask.NameToLayer(layerName);

        tagText = tagObj.AddComponent<TextMeshPro>();
        tagText.fontSize = 1f;
        tagText.alignment = TextAlignmentOptions.Center;
        tagText.font = Plugin.comicSans;
        tagText.font.material.shader = Shader.Find("TextMeshPro/Distance Field");
    }

    private static readonly Color Orange = new Color(1f, 0.5f, 0f);

    private void Update()
    {
        if (rig == null)
            rig = GetComponent<VRRig>();

        int fps = rig.fps;

        Color tagColour = Color.green;


        if (fps < 49)
        {
            tagColour = Color.red;
        }
        else if (fps < 59)
        {
            tagColour = Orange;
        }
        else if (fps < 89)
        {
            tagColour = Color.yellow;
        }
        else if (fps >= 120 && fps < 255)
        {
            tagColour = Color.blue;
        }
        else
        {
            tagColour = Color.green;
        }

        firstPersonTagText.text = fps.ToString();
        thirdPersonTagText.text = fps.ToString();

        firstPersonTagText.color = tagColour;
        thirdPersonTagText.color = tagColour;
    }

    private void OnDestroy()
    {
        Destroy(firstPersonTag);
        Destroy(thirdPersonTag);
    }

    private void Start() => StartCoroutine(DelayedStart());
}