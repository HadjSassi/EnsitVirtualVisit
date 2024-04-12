using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarIdles : MonoBehaviour
{
    private static readonly int IdleNumber = Animator.StringToHash("IdleNumber");
    private static readonly int WaitingTime = Animator.StringToHash("waitingTime");
    private static readonly int IdleDone = Animator.StringToHash("Done");
    private static readonly int exitTime = Animator.StringToHash("exitTime");

    private float waitingTime; // Current waiting time

    private Animator animator;

    [SerializeField] private int idleNumber = 0;

    [SerializeField] private float minWaitingTime = 20f; // Minimum waiting time
    [SerializeField] private float maxWaitingTime = 120f; // Maximum waiting time



    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        waitingTime = Random.Range(minWaitingTime, maxWaitingTime); // Initialize waiting time randomly

    }

    // Update is called once per frame
    void Update()
    {
        if (animator != null)
        {
            // Check if any movement keys are pressed

            // If no movement keys are pressed, wait for the specified time and then set a new random idle animation
            waitingTime -= Time.deltaTime;
            animator.SetFloat(WaitingTime, waitingTime);

            if (animator.GetBool(IdleDone))
            {
                animator.SetBool(IdleDone, false);
                // Reset waiting time
                waitingTime = Random.Range(minWaitingTime, maxWaitingTime); // Initialize waiting time randomly
            }
            else
            {
                if (waitingTime <= 0)
                {
                    // Generate a random number between 1 and 6 for the idle animation
                    idleNumber = Random.Range(1, 7);
                    // Set the idle animation
                    animator.SetInteger(IdleNumber, idleNumber);
                }
            }
        }
        else
        {
            animator = GetComponent<Animator>();
        }
    }
}