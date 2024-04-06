using BackEnd.Model;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractions : MonoBehaviour
{
    public static readonly string CanvasOfInteractable = "CanvaI";
    public static readonly string CanvasOfEchap = "CanvaEchap";
    private GameObject canvaInteractable, canvaEchap;
    public static bool isInInformationCollider;
    private Text nameOfInteractableText; 
    private Text descriptionOfInteractableText;
    private Text typeOfInteractableText;
    private Image imageOfInteractableText;
    private Button linkOfInteractable;
    private Affiche currentAffiche;
    private string AfficheType;

    void Start()
    {
        canvaInteractable = GameObject.FindWithTag(CanvasOfInteractable);
        canvaEchap = GameObject.FindWithTag(CanvasOfEchap);
        if (canvaInteractable != null && canvaEchap != null)
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
            Transform descriptionOfInteractableTransform = canvaInteractable.transform.Find("DescriptionOfInteractable");
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
            canvaEchap.SetActive(false);
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            canvaEchap.SetActive(!canvaEchap.activeSelf);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Information"))
        {
            isInInformationCollider = true;

            CurrentAfficheScript currentAfficheScript = other.GetComponentInChildren<CurrentAfficheScript>();
            if (currentAfficheScript != null)
            {
                currentAffiche = currentAfficheScript.CurrentAffiche;
                AfficheType = currentAfficheScript.typeAffiche;
                if (currentAffiche == null)
                {
                    Debug.LogError("Current Affiche not set in the CurrentAfficheScript.");
                }
                else
                {
                    nameOfInteractableText.text = currentAffiche.titre;
                    descriptionOfInteractableText.text = currentAffiche.description;
                    typeOfInteractableText.text = AfficheType ;
                    linkOfInteractable.onClick.AddListener(() => OpenLink(currentAffiche.lien));
                    // Load image from local path
                    Texture2D texture = LoadTextureFromFile(currentAffiche.image);
                    if (texture != null)
                    {
                        // Convert texture to sprite and assign it to the Image component
                        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                        imageOfInteractableText.sprite = sprite;
                    }
                    else
                    {
                        Debug.LogError("Failed to load image from path: " + currentAffiche.image);
                    }
                }
            }
            else
            {
                Debug.LogError("CurrentAfficheScript component not found on the Information object.");
            }
        }
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
}