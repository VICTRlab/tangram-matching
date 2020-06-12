using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_selection : MonoBehaviour {

    bool flip = true;
    void Update()
    {
        int nbTouches = Input.touchCount;
        Debug.Log(nbTouches);

        if (nbTouches > 0)
        {
            if (flip)
            {
                gameObject.GetComponent<Renderer>().material.color = Color.blue;
                flip = false;
            }
            else
            {
                gameObject.GetComponent<Renderer>().material.color = Color.gray;
                flip = true;
            }
        }
    }
}
