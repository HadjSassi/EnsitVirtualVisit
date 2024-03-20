using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class PlayerInteractions : MonoBehaviour
{
    
    private static readonly int ApplyInteraction = Animator.StringToHash("ApplyInteraction");
    private static readonly string CanvasOfInteractable = "CanvaI";
    private GameObject canvas;
    
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindWithTag(CanvasOfInteractable); // Find the canvas object
        print(canvas);
        if (canvas != null)
        {
            canvas.SetActive(false); // Initially deactivate the canvas
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.tag == "Interaction")
        {
            Animator a = other.GetComponentInParent<Animator>();
            a.SetBool(ApplyInteraction,true);
            
        }
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Information"))
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.I))
            {
                if (canvas != null)
                {
                    canvas.SetActive(!canvas.activeSelf); // Toggle the canvas
                }
            }
        }

        if (other.CompareTag("Interaction"))
        {
            Animator animator = other.GetComponentInParent<Animator>();
            if (animator != null)
            {
                animator.SetBool(ApplyInteraction, true);
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interaction"))
        {
            Animator animator = other.GetComponentInParent<Animator>();
            if (animator != null)
            {
                animator.SetBool(ApplyInteraction, false);
            }
        }

        if (other.CompareTag("Information"))
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.I))
            {
                if (canvas != null)
                {
                    canvas.SetActive(false); // Deactivate the canvas
                }
            }
        }
    }
}
