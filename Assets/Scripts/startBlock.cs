using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startBlock : MonoBehaviour
{
    public Manager m;
    public AudioManager audioM;
    public float goDownValue = 0.015f;

    private void OnTriggerEnter(Collider other)
    {
        audioM.Play("Enter");
        m.startTimer();
        transform.position = new Vector3(transform.position[0], transform.position[1] - goDownValue, transform.position[2]);
    }

    private void OnTriggerExit(Collider other)
    {
        transform.position = new Vector3(transform.position[0], transform.position[1] + goDownValue, transform.position[2]);
    }
}
