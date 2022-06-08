using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    public Manager m;

    private void Update() {
        if (Input.GetKeyDown("space")) {
            m.nextLevel();
        }
    }

    public void NextLevel2()
    {
        m.nextLevel();
    }
}
