using TMPro;
using UnityEngine;

namespace FPSNametagsForZlothy.Tags;

public class Nametag : MonoBehaviour
{
    public GameObject firstPersonTag;
    public GameObject thirdPersonTag;

    private TextMeshPro firstPersonTagText;
    private TextMeshPro thirdPersonTagText;

    private NetPlayer player;

    public void UpdateColour(Color colour)
    {
        if (firstPersonTag == null || thirdPersonTag == null)
            CreateNametags();

        firstPersonTagText.color = colour;
        thirdPersonTagText.color = colour;
    }

    private void CreateNametags()
    {
        CreateNametag(ref firstPersonTag, ref firstPersonTagText, "FirstPersonTag", "FirstPersonOnly");
        CreateNametag(ref thirdPersonTag, ref thirdPersonTagText, "ThirdPersonTag", "MirrorOnly");
    }

    private void CreateNametag(ref GameObject tagObj, ref TextMeshPro tagText, string name, string layerName)
    {
        tagObj = new GameObject(name, typeof(Canvas), typeof(RectTransform));
        tagObj.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        tagObj.transform.SetParent(transform);
        tagObj.transform.localPosition = new Vector3(0f, 0.6f, 0f);

        tagObj.layer = LayerMask.NameToLayer(layerName);

        tagText = tagObj.AddComponent<TextMeshPro>();
        tagText.fontSize = 1.5f;
        tagText.alignment = TextAlignmentOptions.Center;
        tagText.font = Plugin.comicSans;
        tagText.font.material.shader = Shader.Find("TextMeshPro/Distance Field");
    }

    private void Update()
    {
        firstPersonTag.transform.LookAt(Plugin.firstPersonCameraTransform);
        thirdPersonTag.transform.LookAt(Plugin.thirdPersonCameraTransform);

        firstPersonTag.transform.Rotate(0f, 180f, 0f);
        thirdPersonTag.transform.Rotate(0f, 180f, 0f);

        firstPersonTagText.text = player.NickName;
        thirdPersonTagText.text = player.NickName;
    }

    private void OnDestroy()
    {
        Destroy(firstPersonTag);
        Destroy(thirdPersonTag);
    }


    private void Start() => player = GetComponent<VRRig>().OwningNetPlayer;
}