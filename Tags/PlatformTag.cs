using FPSNametagsForZlothy.Tags;
using FPSNametagsForZlothy;
using System.Collections;
using TMPro;
using UnityEngine;


    public class PlatformTag : MonoBehaviour
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
            tagObj.transform.localPosition = new Vector3(0f, -0.1f, 0f);

            tagObj.layer = LayerMask.NameToLayer(layerName);

            tagText = tagObj.AddComponent<TextMeshPro>();
            tagText.fontSize = 0.8f;
            tagText.alignment = TextAlignmentOptions.Center;
            tagText.font = Plugin.comicSans;
        }

        private void Update()
        {
            if (rig == null)
                rig = GetComponent<VRRig>();

            string platform = GetPlatform(rig);
            Color tagColour = Color.white;

            switch (platform)
            {
                case "STEAM":
                    tagColour = Color.magenta;
                    break;
                case "QUEST?":
                    tagColour = Color.cyan;
                    break;
                case "PC":
                    tagColour = Color.yellow;
                    break;
                default:
                    tagColour = Color.cyan;
                    break;
            }

            firstPersonTagText.text = platform;
            thirdPersonTagText.text = platform;

            firstPersonTagText.color = tagColour;
            thirdPersonTagText.color = tagColour;
        }

        private void OnDestroy()
        {
            Destroy(firstPersonTag);
            Destroy(thirdPersonTag);
        }

        private void Start() => StartCoroutine(DelayedStart());

        private static string GetPlatform(VRRig rig)
        {
            string concatStringOfCosmeticsAllowed = rig.concatStringOfCosmeticsAllowed;

            if (concatStringOfCosmeticsAllowed.Contains("S. FIRST LOGIN"))
                return "STEAM";
            else if (concatStringOfCosmeticsAllowed.Contains("FIRST LOGIN") || rig.Creator.GetPlayerRef().CustomProperties.Count >= 2)
                return "PC";

            return "QUEST";
        }
    }

