using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChaser : MonoBehaviour
{
    public Enemy1 enemy1;
    public float chaseTime = 100;

    private float chaseTimeLeft;
    private AudioManager audioM;
    private bool hasPlayedFirstCaughtSound = false;


    // Start is called before the first frame update
    void Start()
    {
        //audioM = GameObject.Find("ManagerAudio").GetComponent<AudioManager>();
        chaseTimeLeft = chaseTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy1.isFollowing) {
            chaseTimeLeft = chaseTimeLeft - 0.01f;
        }
        if (chaseTimeLeft <= 0) {
            enemy1.isFollowing = false;
            chaseTimeLeft = chaseTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        enemy1.isFollowing = true;
        if (!hasPlayedFirstCaughtSound) {
            //audioM.Play("chasingPlayer");
            //hasPlayedFirstCaughtSound = true;
        }
        else {
            //audioM.Play("chasingPlayer2");
        }
    }

    void OnDestroy()
    {
        if (audioM.IsPlaying("chasingPlayer")) {
            //audioM.Stop("chasingPlayer");
        }
        if (audioM.IsPlaying("chasingPlayer2")) {
            //audioM.Stop("chasingPlayer2");
        }
    }
}
