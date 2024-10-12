using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpforce : MonoBehaviour
{
    Rigidbody2D body; 
    // Start is called before the first frame update
    void Start()
    {
      body =  GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            body.AddForce(new Vector2(0, 75));
        }

        Debug.Log(body.position);
    }
}
