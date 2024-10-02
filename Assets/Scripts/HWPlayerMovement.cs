using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HWPlayerMovement : MonoBehaviour
{

    Rigidbody2D player;
    bool isColliding;

    [SerializeField]
    float xSpeed;
    [SerializeField]
    float ySpeed;
    [SerializeField]
    float maxVelocityX;
    [SerializeField]
    float maxVelocityY;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey(KeyCode.A) && isColliding)
        {
            player.velocity += new Vector2(-xSpeed, 0);
            if(player.velocity.x < -maxVelocityX)
            {
                player.velocity = new Vector2(-maxVelocityX, player.velocity.y);
            }

        }
        if (Input.GetKey(KeyCode.D) && isColliding)
        {
            player.velocity += new Vector2(xSpeed, 0);
            if (player.velocity.x > maxVelocityX)
            {
                player.velocity = new Vector2(maxVelocityX, player.velocity.y);
            }
        }
        if(Input.GetKeyDown(KeyCode.Space) && isColliding)
        {
            player.velocity += new Vector2(0, ySpeed);
            if (player.velocity.y > maxVelocityY)
            {
                player.velocity = new Vector2(player.velocity.x, maxVelocityY);
            }
        }
        Debug.Log(player.velocity);
    }

     void OnCollisionEnter2D(Collision2D collision)
    {
        isColliding = true;
        Debug.Log("Colliding");
    }

   void OnCollisionExit2D(Collision2D collision)
    {
        isColliding = false;
        Debug.Log("Not colliding");
    }
}
