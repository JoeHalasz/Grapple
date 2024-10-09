using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlipPlayer : MonoBehaviour
{
    KeyCode currentKey;
    KeyCode prevKey;
    // Start is called before the first frame update
    void Start()
    {
        currentKey = KeyCode.None;
        prevKey = KeyCode.D;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            currentKey = KeyCode.A;
            Flip();
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            currentKey = KeyCode.D;
            Flip();
        }
    }

    void Flip()
    {
        if( currentKey != prevKey)
        {
            transform.Rotate(0, 180, 0);
        }
        prevKey = currentKey;
    }
}
