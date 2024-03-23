using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    public static readonly string CanvasOfInteractable = "CanvaI";
    public static readonly string CanvasOfEchap = "CanvaEchap";
    private GameObject canvaInteractable, canvaEchap;
    public static bool isInInformationCollider;

    void Start()
    {
        canvaInteractable = GameObject.FindWithTag(CanvasOfInteractable);
        canvaEchap = GameObject.FindWithTag(CanvasOfEchap);
        if (canvaInteractable != null && canvaEchap != null)
        {
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
        }
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
        }
    }
}