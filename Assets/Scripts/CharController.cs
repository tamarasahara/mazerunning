using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor.Animations;

public class CharController : MonoBehaviour
{
    private Animator animator;
    public AudioManager audioM;

    public bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            if ((Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Vertical") > 0))
            {
                animator.SetBool("isWalking", true);
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    animator.SetBool("isRunning", true);
                    if (!audioM.IsPlaying("running"))
                    {
                        audioM.Play("running");
                    }
                }
                else
                {
                    animator.SetBool("isRunning", false);
                    if (audioM.IsPlaying("running"))
                    {
                        audioM.Stop("running");
                    }
                    if (!audioM.IsPlaying("walking"))
                    {
                        audioM.Play("walking");
                    }

                }
            }
            else
            {
                animator.SetBool("isWalking", false);
                if (audioM.IsPlaying("walking"))
                {
                    audioM.Stop("walking");
                }
                if (audioM.IsPlaying("running"))
                {
                    audioM.Stop("running");
                }
            }
        }
    }
}
