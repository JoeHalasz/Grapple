using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeObjectJump : MonoBehaviour
{

    Rigidbody2D body;
    float addedTime = 0;
    // Start is called before the first frame update
    void Start()
    {
         body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        addedTime += Time.deltaTime;
        if(addedTime > 5)
        {
            body.AddForce(new Vector2(0, 500));
            addedTime = 0;
        }

        Debug.Log(body.position);

    }
}
