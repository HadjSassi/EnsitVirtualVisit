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
    }

    public void LinkClick()
    {
        Application.OpenURL("https://www.google.com");
    }

    public void Resume()
    {
        print("Resume");
        GameObject canvas = GameObject.FindWithTag(PlayerInteractions.CanvasOfEchap);
        canvas.SetActive(false);
        MouseCursorLock.Instance.Apply();
    }
    
}
