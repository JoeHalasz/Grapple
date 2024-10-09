using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    public float jumpForce;
    Rigidbody2D rb;
    public float maxVelocity;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(0, jumpForce);
        }
        if((rb.velocity.y) >= maxVelocity)
        {
            rb.velocity = new Vector2(0, maxVelocity);
        }
      //  Debug.Log(rb.velocityY);
    }
}
