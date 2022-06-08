using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallGrower : MonoBehaviour
{
    public GameObject labyrinth;

    private GameObject[] walls;

    // Start is called before the first frame update
    void Start()
    {
        walls = GetComponentsInChildren<GameObject>();
        Debug.Log(walls.Length);
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        foreach(GameObject wall in walls)
        {
            Debug.Log(wall.name.ToString());
            //wall.transform.localScale = new Vector3((float)wall.transform.localScale[0] * 0.99f, (float)wall.transform.localScale[1], (float)wall.transform.localScale[2]);
        }
        */
    }
}
