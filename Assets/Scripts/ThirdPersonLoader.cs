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
        //todo mahdi you need to get the image from the player image and change it by the same avatarUrl but replace the .glb by .png
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

        private void OnLoadFailed(object sender, FailureEventArgs args)
        {
            OnLoadComplete?.Invoke();
        }

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
            }
            else
            {
                Debug.LogWarning("PlayerCanva object not found in the scene.");
            }

            // var controller = GetComponent<ThirdPersonController>();
            // if (controller != null)
            // {
            //     controller.Setup(avatar, animatorController);
            // }
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