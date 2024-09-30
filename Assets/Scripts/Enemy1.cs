using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    private Manager m;
    private AudioManager audioM;
    private GameObject grid;
    private Vector3 velocity;
    private GameObject player;

    public float moveSpeed = 0.5f;
    public bool isFollowing = false;

    private void Start() {
        m = GameObject.Find("Manager").GetComponent<Manager>();
        audioM = GameObject.Find("ManagerAudio").GetComponent<AudioManager>();
        grid = GameObject.Find("floor");
        player = GameObject.Find("Rin2_");
        velocity = new Vector3(1,0,1);
    }

    private void Update() {
        if (isFollowing) {
            chase();
        }
        move();
       
    }

    private void OnTriggerEnter(Collider other)
    {
        m.enemy1Hit();
        Debug.Log("Enemy Hit");
        audioM.Play("caught");
        
    }

    private void move() {
        Vector3 newPosition = transform.position + moveSpeed * velocity;
        if ((newPosition.x < 0) || (newPosition.x > grid.transform.position.x)) {
            velocity.x *= -1;
        }
        if ((newPosition.z < 0) || (newPosition.z > grid.transform.position.z)) {
            velocity.z *= -1;
        }
        transform.position = newPosition;
    }

    private void chase() {
        velocity = player.transform.position - transform.position;
        velocity.Normalize();
    }

    void OnDestroy()
    {
        if (audioM.IsPlaying("caught")) {
            audioM.Stop("caught");
        }
    }
}
