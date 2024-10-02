using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{ Rigidbody2D body;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))    
        {
            body.AddForce(new Vector2(1, 0));
        }
        else if (Input.GetKey(KeyCode.A))
        {
            body.AddForce(new Vector2(-1, 0));
        }
        Debug.Log(body.position);
    }
}
    