using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private Manager m;
    private AudioManager audioM;

    private void Start() {
        m = GameObject.Find("Manager").GetComponent<Manager>();
        audioM = GameObject.Find("ManagerAudio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        m.gatherItem();
        Destroy(transform.gameObject);
        audioM.Play("pling");
    }
}
