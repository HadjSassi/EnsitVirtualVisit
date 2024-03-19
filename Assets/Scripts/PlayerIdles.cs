using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdles : MonoBehaviour
{
    private static readonly int IdleNumber = Animator.StringToHash("IdleNumber");
    private static readonly int WaitingTime = Animator.StringToHash("waitingTime");
    private static readonly int IdleDone = Animator.StringToHash("Done");
    private static readonly int exitTime = Animator.StringToHash("exitTime");

    private GameObject avatar;

    private Animator animator;
    
    [SerializeField] private int idleNumber = 0;

    [SerializeField] private float waitingTime = 60f; //1 minute


    // Start is called before the first frame update
    void Awake()
    {
        avatar = transform.GetChild(0).gameObject;
        animator = avatar.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if any movement keys are pressed
        // todo for checking what needs to be pressed
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow) ||
            Input.GetKey(KeyCode.LeftArrow))
        {
            animator.SetTrigger(exitTime);
            // If any movement key is pressed, immediately return to the first state
            animator.SetInteger(IdleNumber, 0);
            waitingTime = 60f; // Reset waiting time to 1 minute (60 seconds)
        }
        else
        {
            // If no movement keys are pressed, wait for the specified time and then set a new random idle animation
            waitingTime -= Time.deltaTime;
            animator.SetFloat(WaitingTime, waitingTime);

            if (animator.GetBool(IdleDone))
            {
                animator.SetBool(IdleDone, false);
                // Reset waiting time
                waitingTime = 60f; // Reset waiting time to 1 minute (60 seconds)
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
    }
}