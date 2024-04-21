using System;
using System.Collections;
using ReadyPlayerMe.Core;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;
using Avatar = BackEnd.Model.Avatar;

public class AvatarsScript : MonoBehaviour
{
    private List<Avatar> npcAvatarsList = new List<Avatar>();
    private List<Avatar> simpleAvatarsList = new List<Avatar>();
    private bool loadOnStart = true;
    private string npcAvatarTag = "npcAvatar";
    private AvatarObjectLoader[] npcAvatarObjectLoaders;
    [SerializeField] private RuntimeAnimatorController animatorController;
    [SerializeField] private GameObject[] previewNpcAvatars;
    private TransformData[] previewNpcTransforms; // Array to store transform data
    [SerializeField] private GameObject[] NpcAvatars;
    [SerializeField] private GameObject positionXPrefab; // Assign the prefab in the Unity Editor

    public event Action OnLoadComplete;
    [Serializable]
    public struct TransformData
    {
        public Vector3 position;
        public Quaternion rotation;
        public Collider collider;
    }
    [Obsolete("Obsolete")]
    private void Start()
    {
        StartCoroutine(Main.Instance.Web.GetAvatarsLists(OnAvatarsListsReceived));
        
    }

    [Obsolete("Obsolete")]
    void OnAvatarsListsReceived(List<Avatar> npcAvatars, List<Avatar> playerAvatars)
    {
        // Assuming NPC avatars are included in the npcAvatars list returned from the server
        npcAvatarsList = npcAvatars;
        previewNpcAvatars = GameObject.FindGameObjectsWithTag(npcAvatarTag);
        if (previewNpcAvatars == null || previewNpcAvatars.Length == 0)
        {
            Debug.LogError("No GameObjects with tag 'npcAvatar' found.");
            return;
        }
        npcAvatarObjectLoaders = new AvatarObjectLoader[previewNpcAvatars.Length];
        NpcAvatars = new GameObject[previewNpcAvatars.Length];
        previewNpcTransforms = new TransformData[previewNpcAvatars.Length]; 
        for (int i = 0; i < previewNpcAvatars.Length; i++)
        {
            int currentIndex = i;
            previewNpcTransforms[currentIndex].position = previewNpcAvatars[i].transform.position;
            previewNpcTransforms[currentIndex].rotation = previewNpcAvatars[i].transform.rotation;
            previewNpcTransforms[currentIndex].collider = previewNpcAvatars[i].GetComponent<Collider>();

            npcAvatarObjectLoaders[currentIndex] = new AvatarObjectLoader();
            npcAvatarObjectLoaders[currentIndex].OnCompleted += (sender, args) => OnLoadCompleted(sender, args, currentIndex);
            npcAvatarObjectLoaders[currentIndex].OnFailed += OnLoadFailed;

            if (previewNpcAvatars[currentIndex] != null)
            {
                SetupAvatar(previewNpcAvatars[currentIndex], currentIndex);
            }

            if (loadOnStart)
            {
                LoadAvatar(currentIndex);
            }
            
        }
    }

    private void OnLoadFailed(object sender, FailureEventArgs args)
    {
        OnLoadComplete?.Invoke();
    }

    [Obsolete("Obsolete")]
    private void OnLoadCompleted(object sender, CompletionEventArgs args, int index)
    {
        // Check if the index is within the bounds of the array
        if (index >= 0 && index < previewNpcAvatars.Length)
        {
            if (previewNpcAvatars[index] != null)
            {
                Destroy(previewNpcAvatars[index]);
                previewNpcAvatars[index] = null;
            }

            SetupAvatar(args.Avatar, index);
        }
        else
        {
            Debug.LogError("Index is outside the bounds of the previewNpcAvatars array.");
            return; // Exit the method if the index is invalid
        }

        OnLoadComplete?.Invoke();
    }


    [Obsolete("Obsolete")]
    private void SetupAvatar(GameObject targetAvatar, int index)
    {
        if (NpcAvatars[index] != null)
        {
            Destroy(NpcAvatars[index]);
        }

        NpcAvatars[index] = targetAvatar;
        NpcAvatars[index].transform.SetParent(transform);
        NpcAvatars[index].transform.SetAsFirstSibling();
        NpcAvatars[index].transform.position = previewNpcTransforms[index].position;
        NpcAvatars[index].transform.rotation = previewNpcTransforms[index].rotation;
        
        if (positionXPrefab != null)
        {
            // Instantiate PositionX prefab and set it as a child of NpcAvatars[index]
            GameObject positionX = Instantiate(positionXPrefab, NpcAvatars[index].transform);
            positionX.transform.localPosition = new Vector3(0.00300000003f, 1.72599995f, 0.00100000005f);
            positionX.transform.localRotation = Quaternion.identity;
            positionX.transform.localScale = new Vector3(0.0868599862f, 0.112799995f, 0.0665499792f);
            positionX.name = "Position"; // Ensure the name is set to PositionX
        }
        else
        {
            Debug.LogWarning("PositionX prefab not found in Resources folder.");
        }
        
        Collider originalCollider = previewNpcTransforms[index].collider;
        Collider newCollider = null;

        if (originalCollider is BoxCollider)
        {
            BoxCollider originalBoxCollider = (BoxCollider)originalCollider;
            BoxCollider newBoxCollider = NpcAvatars[index].AddComponent<BoxCollider>();
            newBoxCollider.center = originalBoxCollider.center;
            newBoxCollider.size = originalBoxCollider.size;
            newCollider = newBoxCollider;
        }
        else if (originalCollider is SphereCollider)
        {
            SphereCollider originalSphereCollider = (SphereCollider)originalCollider;
            SphereCollider newSphereCollider = NpcAvatars[index].AddComponent<SphereCollider>();
            newSphereCollider.center = originalSphereCollider.center;
            newSphereCollider.radius = originalSphereCollider.radius;
            newCollider = newSphereCollider;
        }
        else if (originalCollider is CapsuleCollider)
        {
            CapsuleCollider originalCapsuleCollider = (CapsuleCollider)originalCollider;
            CapsuleCollider newCapsuleCollider = NpcAvatars[index].AddComponent<CapsuleCollider>();
            newCapsuleCollider.center = originalCapsuleCollider.center;
            newCapsuleCollider.radius = originalCapsuleCollider.radius;
            newCapsuleCollider.height = originalCapsuleCollider.height;
            newCapsuleCollider.direction = originalCapsuleCollider.direction;
            newCollider = newCapsuleCollider;
        }
        else
        {
            Debug.LogError("Unsupported collider type detected.");
            return;
        }

        
        Animator avatarAnimator = NpcAvatars[index].GetComponent<Animator>();
        if (avatarAnimator != null)
        {
            // Set the runtime animator controller
            avatarAnimator.runtimeAnimatorController = animatorController;
            // Add PlayerIdles script to the Animator component
            AvatarIdles AvatarIdles = avatarAnimator.gameObject.GetComponent<AvatarIdles>();
            if (AvatarIdles == null)
            {
                AvatarIdles = avatarAnimator.gameObject.AddComponent<AvatarIdles>();
            }
        }
        
        Transform positionXTransform = NpcAvatars[index].transform.Find("Position/Interaction");
    
        if (positionXTransform != null)
        {
            // Get the CurrentCanvaScript component
            CurrentCanvaScript currentCanvaScript = positionXTransform.GetComponent<CurrentCanvaScript>();

            if (currentCanvaScript != null)
            {
                currentCanvaScript.typeObject = 3;
                currentCanvaScript.CurrentAvatar = npcAvatarsList[index];
                currentCanvaScript.typeAffiche = npcAvatarsList[index].avatarName;
            }
            else
            {
                Debug.LogWarning("CurrentCanvaScript component not found on Position/Interaction object.");
            }
        }
        else
        {
            Debug.LogWarning("Position/Interaction object not found.");
        }
    }

    public void LoadAvatar(int index)
    {
        
        // Get the avatar URL at the random index
        Avatar obj = npcAvatarsList[index];
        string avatarUrl = obj.url;
        
        // Load the avatar using the obtained URL
        npcAvatarObjectLoaders[index].LoadAvatar(avatarUrl);
        
    }

    string ExtractAvatarId(string url)
    {
        // Split the URL by '/'
        string[] parts = url.Split('/');
        // Get the last part
        string lastPart = parts[parts.Length - 1];
        // Remove the '.glb' extension
        string id = lastPart.Replace(".glb", "");
        return id;
    }


    [Obsolete("Obsolete")]
    IEnumerator WaitForAvatarResponse(string avatarId, Text playerNameText)
    {
        // Call GetAvatar method and wait for the response
        yield return Main.Instance.Web.GetAvatar(avatarId, avatar =>
        {
            // Check if playerNameText is not null before accessing it
            if (playerNameText != null)
            {
                playerNameText.text =
                    avatar != null
                        ? avatar.avatarName
                        : "Unknown"; // Ensure avatar is not null before accessing its properties
            }
            else
            {
                Debug.LogWarning("playerNameText is null!");
            }
        });
    }


    private IEnumerator LoadImageFromUrl(string url, Image imageComponent)
    {
        url = url.Replace(".glb", ".png");
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError ||
                www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Failed to load image from URL: {www.error}");
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                imageComponent.sprite = sprite;
            }
        }
    }
}