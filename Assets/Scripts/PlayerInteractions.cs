using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    private static readonly string CanvasOfInteractable = "CanvaI";
    private GameObject canvas;
    private bool isInInformationCollider;
    
    void Start()
    {
        canvas = GameObject.FindWithTag(CanvasOfInteractable);
        if (canvas != null)
        {
            canvas.SetActive(false);
        }

        isInInformationCollider = false;
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.I))
        {
            if (isInInformationCollider && canvas != null)
            {
                canvas.SetActive(!canvas.activeSelf);

                // Update cursor settings based on canvas activity
                if (canvas.activeSelf)
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

    private void OnTriggerStay(Collider other)
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
            isInInformationCollider = false;
            canvas.SetActive(false);
            MouseCursorLock.Instance.Apply();
        }
    }
}