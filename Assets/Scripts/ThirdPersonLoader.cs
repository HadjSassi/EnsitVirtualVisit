using System;
using System.Collections;
using ReadyPlayerMe.Core;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ReadyPlayerMe.Samples.QuickStart
{
    public class ThirdPersonLoader : MonoBehaviour
    {
        //todo mahdi here you will search in the database the avatar name from the avatarUrl

        private string playerCanvaTag = "PlayerCanva";

        private readonly Vector3 avatarPositionOffset = new Vector3(0, 0, -1.07f);

        [SerializeField] [Tooltip("RPM avatar URL or shortcode to load")]
        private string avatarUrl;

        private GameObject avatar;
        private AvatarObjectLoader avatarObjectLoader;

        [SerializeField] [Tooltip("Animator to use on loaded avatar")]
        private RuntimeAnimatorController animatorController;

        [SerializeField] [Tooltip("If true it will try to load avatar from avatarUrl on start")]
        private bool loadOnStart = true;

        [SerializeField]
        [Tooltip("Preview avatar to display until avatar loads. Will be destroyed after new avatar is loaded")]
        private GameObject previewAvatar;

        public event Action OnLoadComplete;

        [Obsolete("Obsolete")]
        private void Start()
        {
            avatarObjectLoader = new AvatarObjectLoader();
            avatarObjectLoader.OnCompleted += OnLoadCompleted;
            avatarObjectLoader.OnFailed += OnLoadFailed;

            if (previewAvatar != null)
            {
                SetupAvatar(previewAvatar);
            }

            if (loadOnStart)
            {
                LoadAvatar(avatarUrl);
            }
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
        private void OnLoadFailed(object sender, FailureEventArgs args)
        {
            OnLoadComplete?.Invoke();
        }

        [Obsolete("Obsolete")]
        private void OnLoadCompleted(object sender, CompletionEventArgs args)
        {
            if (previewAvatar != null)
            {
                Destroy(previewAvatar);
                previewAvatar = null;
            }

            SetupAvatar(args.Avatar);
            OnLoadComplete?.Invoke();
        }

        [Obsolete("Obsolete")]
        private void SetupAvatar(GameObject targetAvatar)
        {
            if (avatar != null)
            {
                Destroy(avatar);
            }

            avatar = targetAvatar;
            avatar.gameObject.tag = "Player";
            // Re-parent and reset transforms
            avatar.transform.SetParent(transform); // Make the avatar a child of this object
            avatar.transform.SetAsFirstSibling(); // Set the avatar as the first child
            // avatar.transform.parent = transform;
            avatar.transform.localPosition = avatarPositionOffset;
            avatar.transform.localRotation = Quaternion.Euler(0, -180, 0);

            Animator avatarAnimator = avatar.GetComponent<Animator>();
            if (avatarAnimator != null)
            {
                // Set the runtime animator controller
                avatarAnimator.runtimeAnimatorController = animatorController;
            }

            // Find PlayerCanva object by tag
            GameObject playerCanvaObject = GameObject.FindGameObjectWithTag(playerCanvaTag);
            if (playerCanvaObject != null)
            {
                // Get Player Image and Player Name child objects
                Transform playerImage = playerCanvaObject.transform.Find("Player Image");
                Transform playerName = playerCanvaObject.transform.Find("Player Name");

                // Modify source image of Player Image
                if (playerImage != null)
                {
                    Image imageComponent = playerImage.GetComponent<Image>();
                    if (imageComponent != null)
                    {
                        StartCoroutine(LoadImageFromUrl(avatarUrl, imageComponent));
                    }
                    else
                    {
                        Debug.LogWarning("Image component not found on Player Image object.");
                    }
                }
                else
                {
                    Debug.LogWarning("Player Image object not found in PlayerCanva.");
                }
                if (playerName != null)
                {
                    Text playerNameText = playerName.GetComponent<Text>();
                    if (playerNameText != null)
                    {
                        // Extract the avatar ID from the URL
                        string avatarId = ExtractAvatarId(avatarUrl);
                        // Search for the avatar name in the database using the avatar ID
                        
                        StartCoroutine(WaitForAvatarResponse(avatarId, playerNameText));
                    }
                    else
                    {
                        Debug.LogWarning("Text component not found on Player Name object.");
                    }
                }
            }
            else
            {
                Debug.LogWarning("PlayerCanva object not found in the scene.");
            }

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
                    playerNameText.text = avatar != null ? avatar.avatarName : "Unknown"; // Ensure avatar is not null before accessing its properties
                }
                else
                {
                    Debug.LogWarning("playerNameText is null!");
                }
            });
        }
        public void LoadAvatar(string url)
        {
            //remove any leading or trailing spaces
            avatarUrl = url.Trim(' ');
            avatarObjectLoader.LoadAvatar(avatarUrl);
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
}