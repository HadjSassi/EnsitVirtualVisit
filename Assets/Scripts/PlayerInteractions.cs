using System;
using System.Collections;
using BackEnd.Model;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Avatar = BackEnd.Model.Avatar;

public class PlayerInteractions : MonoBehaviour
{
    public static readonly string CanvasOfInteractable = "CanvaI";
    private GameObject canvaInteractable;
    public static bool isInInformationCollider;
    private Text nameOfInteractableText;
    private Text descriptionOfInteractableText;
    private Text typeOfInteractableText;
    private Image imageOfInteractableText;
    private Button linkOfInteractable;
    private Affiche currentAffiche;
    private Stand currentStand;
    private Avatar currentAvatar;
    void Start()
    {
        canvaInteractable = GameObject.FindWithTag(CanvasOfInteractable);
        if (canvaInteractable != null)
        {
            Transform imageOfInteractableTransform = canvaInteractable.transform.Find("ImageOfInteractable");
            if (imageOfInteractableTransform != null)
            {
                imageOfInteractableText = imageOfInteractableTransform.GetComponent<Image>();
                if (imageOfInteractableText == null)
                {
                    Debug.LogError("imageOfInteractableText component not found in the children of CanvaI.");
                }
            }
            else
            {
                Debug.LogError("Child named imageOfInteractableText not found under CanvaI.");
            }

            Transform typeOfInteractableTransform = canvaInteractable.transform.Find("TypeOfInteractable");
            if (typeOfInteractableTransform != null)
            {
                typeOfInteractableText = typeOfInteractableTransform.GetComponent<Text>();
                if (typeOfInteractableText == null)
                {
                    Debug.LogError("typeOfInteractableText component not found in the children of CanvaI.");
                }
            }
            else
            {
                Debug.LogError("Child named typeOfInteractableText not found under CanvaI.");
            }

            Transform nameOfInteractableTransform = canvaInteractable.transform.Find("NameOfInteractable");
            if (nameOfInteractableTransform != null)
            {
                nameOfInteractableText = nameOfInteractableTransform.GetComponent<Text>();
                if (nameOfInteractableText == null)
                {
                    Debug.LogError("Text component not found in the children of CanvaI.");
                }
            }
            else
            {
                Debug.LogError("Child named NameOfInteractable not found under CanvaI.");
            }

            Transform linkInteractableTransform = canvaInteractable.transform.Find("LinkToInteractable");
            if (linkInteractableTransform != null)
            {
                linkOfInteractable = linkInteractableTransform.GetComponent<Button>();
                if (linkOfInteractable == null)
                {
                    Debug.LogError("linkOfInteractable component not found in the children of CanvaI.");
                }
            }
            else
            {
                Debug.LogError("Child named linkOfInteractable not found under CanvaI.");
            }

            Transform descriptionOfInteractableTransform =
                canvaInteractable.transform.Find("DescriptionOfInteractable");
            if (descriptionOfInteractableTransform != null)
            {
                descriptionOfInteractableText = descriptionOfInteractableTransform.GetComponent<Text>();
                if (descriptionOfInteractableText == null)
                {
                    Debug.LogError("descriptionOfInteractableText component not found in the children of CanvaI.");
                }
            }
            else
            {
                Debug.LogError("Child named descriptionOfInteractableText not found under CanvaI.");
            }

            canvaInteractable.SetActive(false);
        }

        isInInformationCollider = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.I))
        {
            if (isInInformationCollider && canvaInteractable != null)
            {
                canvaInteractable.SetActive(!canvaInteractable.activeSelf);

                // Update cursor settings based on canvas activity
                if (canvaInteractable.activeSelf)
                {
                    MouseCursorLock.Instance.SetCursorVisible(true);
                }
                else
                {
                    MouseCursorLock.Instance.Apply();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Information"))
        {
            isInInformationCollider = true;

            CurrentCanvaScript currentCanvaScript = other.GetComponentInChildren<CurrentCanvaScript>();
            if (currentCanvaScript != null)
            {
                if (currentCanvaScript.typeObject == 1)
                {
                    currentAffiche = currentCanvaScript.CurrentAffiche;
                    if (currentAffiche == null)
                    {
                        Debug.LogError("Current Affiche not set in the CurrentAfficheScript.");
                    }
                    else
                    {
                        nameOfInteractableText.text = currentAffiche.titre;
                        descriptionOfInteractableText.text = currentAffiche.description;
                        typeOfInteractableText.text = currentAffiche.sujet;
                        linkOfInteractable.interactable = true;
                        linkOfInteractable.onClick.AddListener(() => OpenLink(currentAffiche.lien));
                        // Load image from local path
                        /*Texture2D texture = LoadTextureFromFile(currentAffiche.image);
                        if (texture != null)
                        {
                            // Convert texture to sprite and assign it to the Image component
                            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                                Vector2.zero);
                            imageOfInteractableText.sprite = sprite;
                        }
                        else
                        {
                            Debug.LogError("Failed to load image from path: " + currentAffiche.image);
                        }*/
                        Action<Sprite> getAfficheCouvertureCallback = (downloadedSprite) =>
                        {
                            if (downloadedSprite != null)
                            {
                                imageOfInteractableText.sprite = downloadedSprite;
                            }
                            else
                            {
                                Debug.LogError("Failed to get affiche image.");
                            }
                        };
                        StartCoroutine(Main.Instance.Web.GetAfficheImage(currentAffiche.image, getAfficheCouvertureCallback));

                    }
                }
                if (currentCanvaScript.typeObject == 2)
                {
                    currentStand = currentCanvaScript.CurrentStand;
                    if (currentStand == null)
                    {
                        Debug.LogError("Current Stand not set in the CurrentCanvaScript.");
                    }
                    else
                    {
                        nameOfInteractableText.text = currentStand.nom;
                        descriptionOfInteractableText.text = currentStand.description;
                        typeOfInteractableText.text = currentStand.sujet;
                        linkOfInteractable.interactable = true;
                        linkOfInteractable.onClick.AddListener(() => OpenLink(currentStand.lien));
                        // Load image from local path
                        Action<Sprite> getStandCouvertureCallback = (downloadedSprite) =>
                        {
                            if (downloadedSprite != null)
                            {
                                imageOfInteractableText.sprite = downloadedSprite;
                            }
                            else
                            {
                                Debug.LogError("Failed to get stand image.");
                            }
                        };
                        StartCoroutine(Main.Instance.Web.GetStandImage(currentStand.image, getStandCouvertureCallback));
                        
                    }
                }
                if (currentCanvaScript.typeObject == 3)
                {
                    currentAvatar = currentCanvaScript.CurrentAvatar;
                    if (currentAvatar == null)
                    {
                        Debug.LogError("Current Avatar not set in the currentAvatar.");
                    }
                    else
                    {
                        nameOfInteractableText.text = currentAvatar.avatarName;
                        descriptionOfInteractableText.text = currentAvatar.description;
                        typeOfInteractableText.text = "Avatar";
                        // linkOfInteractable.onClick.AddListener(() => OpenLink(currentAvatar.lien));
                        // Load image from local path
                        linkOfInteractable.interactable = false;
                        Texture2D texture = new Texture2D(1, 1); 
                        imageOfInteractableText.sprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), Vector2.zero);
                        StartCoroutine(FetchImageFromWeb(ExtractAvatarImage(currentAvatar.url)));
                    }
                }
            }
            else
            {
                Debug.LogError("CurrentAfficheScript component not found on the Information object.");
            }
        }
    }

    string ExtractAvatarImage(string url)
    {
        string modifiedUrl = "https://models.readyplayer.me/" + url + ".png";
        return modifiedUrl;
    }
    
    private Texture2D LoadTextureFromFile(string filePath)
    {
        byte[] fileData = System.IO.File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(fileData)) // LoadImage() returns true if the loading is successful
        {
            return texture;
        }
        else
        {
            Debug.LogError("Failed to load texture from file: " + filePath);
            return null;
        }
    }

    private void OpenLink(string url)
    {
        Application.OpenURL(url);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Information"))
        {
            if (canvaInteractable.activeSelf)
            {
                MouseCursorLock.Instance.Apply();
                canvaInteractable.SetActive(false);
            }

            isInInformationCollider = false;
            currentAffiche = null;
        }
    }
    
    private IEnumerator FetchImageFromWeb(string url)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error fetching image: " + www.error);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                imageOfInteractableText.sprite = sprite;
            }
        }
    }
}