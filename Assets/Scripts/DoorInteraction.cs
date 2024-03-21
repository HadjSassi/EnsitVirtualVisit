using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    private static readonly int ApplyInteraction = Animator.StringToHash("ApplyInteraction");
    private static readonly string PlayerTag = "PlayerHolder";
    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == PlayerTag)
        {
            animator.SetBool(ApplyInteraction,true);
        }
    }
     private void OnTriggerExit(Collider other)
    {
        if (other.tag == PlayerTag)
        {
            animator.SetBool(ApplyInteraction,false);
        }
    }
    
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
