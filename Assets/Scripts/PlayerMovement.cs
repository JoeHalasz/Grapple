using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// get the inputs in Update, move the player in FixedUpdate
// this is done so that the inputs feel quick and responsive, but the player moves at the same speed no matter the framerate
public class PlayerMovement : MonoBehaviour
{

    Rigidbody2D player;
    [SerializeField][Range(10, 200)]
    public float speed;
    [SerializeField][Range(1, 20)]
    public float jumpStrength;

    [SerializeField][Range(1, 20)]
    public float maxRunningSpeed;

    private float currentXInput = 0;
    private bool jumped = false;

    // save a double for (1f/60f) to avoid recalculating it every frame
    private float speedPerFrame = 1f/60f;

    // grapplehook script
    private GrappleHook grappleHookScript;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        grappleHookScript = GetComponent<GrappleHook>();
    }

    // Update is called once per frame
    void Update() 
    {
        // if d key is pressed, set the currentXInput to 1
        currentXInput = 0;
        if (Input.GetKey(KeyCode.D))
        {
            currentXInput = 1f;
        }
        // if a key is pressed, set the currentXInput to -1
        if (Input.GetKey(KeyCode.A))
        {
            currentXInput = -1f;
        }
        if (!jumped && isGrounded())
            jumped = Input.GetKeyDown(KeyCode.Space) ? true : false;
    }

    // FixedUpdate is called once per physics frame ( 60 time a second right now )
    void FixedUpdate()
    {
        if (isGrounded()) 
        {
            // move the player using acceleration, and if the player is grounded, slow them down
            if (currentXInput != 0)
                player.velocity += new Vector2(currentXInput * speed * speedPerFrame, 0);
            else 
            {
                if (player.velocity.x > 0)
                {
                    player.velocity -= new Vector2(speed * speedPerFrame * 2f, 0);
                    if (player.velocity.x < 0)
                        player.velocity = new Vector2(0, player.velocity.y);
                }
                if (player.velocity.x < 0)
                {
                    player.velocity += new Vector2(speed * speedPerFrame * 2f, 0);
                    if (player.velocity.x > 0)
                        player.velocity = new Vector2(0, player.velocity.y);
                }
            }
            // accelerate towards the max speed if we are over
            if (player.velocity.x > maxRunningSpeed) 
            {
                player.velocity -= new Vector2(speed * speedPerFrame * 2, 0);
            }
            if (player.velocity.x < -maxRunningSpeed) 
            {
                player.velocity += new Vector2(speed * speedPerFrame * 2, 0);
            }

        }
        else // floating or grappling physics
        {
            // slowly accelerate the player in the air if grappling
            //if (grappleHookScript.isGrappling())
                player.velocity += new Vector2(currentXInput * speed * speedPerFrame *.2f, 0);
        }
        if (jumped) 
        {
            player.velocity = new Vector2(player.velocity.x, jumpStrength);
            jumped = false;
        }
    }

    bool isGrounded() 
    {
        RaycastHit2D[] hit;
        // raycast that doesnt hit the player 
        hit = Physics2D.RaycastAll(transform.position, Vector2.down, 1.1f);
        //draw it in the scene view
        Debug.DrawRay(transform.position, Vector2.down * 1.1f, Color.red);
        

        foreach (RaycastHit2D h in hit) {
            if (h.collider != null && h.collider.tag != "Player")
            {
                return true;
            }
        }
        return false;

    }
}
