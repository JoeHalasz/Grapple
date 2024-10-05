using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlipPlayer : MonoBehaviour
{
    KeyCode currentKey;
    KeyCode prevKey;
    float currentRot;
    // Start is called before the first frame update
    void Start()
    {
        currentKey = KeyCode.None;
       currentRot = transform.localRotation.y;
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
            currentRot += 180f;
            if( currentRot >= 360f)
            {
                currentRot = 0f;
            }
            transform.localRotation = new Quaternion(0, currentRot, 0, 0);
        }
        prevKey = currentKey;
    }
}
