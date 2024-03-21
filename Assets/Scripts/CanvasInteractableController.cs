using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasInteractableController : MonoBehaviour
{
    public void CrossClick()
    {
        GameObject canvas = GameObject.FindWithTag(PlayerInteractions.CanvasOfInteractable);
        canvas.SetActive(false);
        MouseCursorLock.Instance.Apply();
        PlayerInteractions.isInInformationCollider = false;
    }

    public void LinkClick()
    {
        Application.OpenURL("https://www.google.com");
    }
    
}
