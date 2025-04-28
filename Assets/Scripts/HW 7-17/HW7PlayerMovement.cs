using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HW7PlayerMovement : MonoBehaviour
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

    private bool isJumping;

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
            player.AddForce(Vector2.left * xSpeed, ForceMode2D.Impulse);
            // player.velocity += new Vector2(-xSpeed, 0);
            if (player.velocity.x < -maxVelocityX)
            {
                player.velocity = new Vector2(-maxVelocityX, player.velocity.y);
            }

        }
        if (Input.GetKey(KeyCode.D) && isColliding)
        {
            player.AddForce(Vector2.right * xSpeed, ForceMode2D.Impulse);
            // player.velocity += new Vector2(xSpeed, 0);
            if (player.velocity.x > maxVelocityX)
            {
                player.velocity = new Vector2(maxVelocityX, player.velocity.y);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && isColliding)
        {
            player.AddForce(Vector2.up * ySpeed, ForceMode2D.Impulse);
            isJumping = true;
            // player.velocity += new Vector2(0, ySpeed);
            if (player.velocity.y > maxVelocityY)
            {
                player.velocity = new Vector2(player.velocity.x, maxVelocityY);
                Debug.Log(player.velocity.y);
            }
        }
       // Debug.Log(player.velocity);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        isColliding = true;
        isJumping = false;
     //   Debug.Log("Colliding");
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isColliding = false;
      //  Debug.Log("Not colliding");
    }
}
