using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : MonoBehaviour
{

    [SerializeField]
    public float animationTime = 0.5f;
    [SerializeField]
    public float grappleMaxDist = 25f;
    [SerializeField]
    public float grappleMinDist = 5f;
    [SerializeField]
    public float grappleLengthChangeSpeed = .25f;
    private bool grappling = false;
    private LineRenderer lr;
    private bool animating = false;
    private Vector2 grappleAnimationTarget;
    private bool endingGrapple = false;
    // add a list of masks to accept
    private Vector2 currentAnimationHitPoint;
    private SpringJoint2D joint;
    private bool needShortenGrapple = false;
    private bool needLengthenGrapple = false;
    private float speedPerFrame = 1f/60f;
    GameObject grappledTo;
    private float speedLowTime = 0;

    public LayerMask canGrappleTo;

    public bool isGrappling()
    {
        return grappling;
    }

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        if (lr == null) 
        {
            lr = gameObject.AddComponent<LineRenderer>();
            lr.startWidth = 0.1f;
            lr.endWidth = 0.1f;
            // make it white
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.startColor = Color.white;
        }
        lr.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startGrapple();
        }
        if (Input.GetMouseButtonDown(1))
        {
            endGrapple();
        }
        // if the player holds w, shorten the grapple
        if (Input.GetKey(KeyCode.W) && grappling)
        {
            needShortenGrapple = true;
        }
        // if the player holds s, lengthen the grapple
        if (Input.GetKey(KeyCode.S) && grappling)
        {
            needLengthenGrapple = true;
        }
    }

    void FixedUpdate()
    {
        if (grappling)
        {
            checkCollision();
            checkShouldBreak();
            checkLengthChange();
        }
        else
        {
            speedLowTime = 0;
        }
    }

    void LateUpdate()
    {
        if (grappling)
        {
            drawGrapple();
        }
    }

    void checkShouldBreak()
    {
        // if the player's x velocity has been under .5f for 1 second and the angle between the player and the grapple pos is above 30 and below 150, break the grapple
        if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) < .5f)
        {
            speedLowTime += Time.deltaTime;
        }
        float angle = Vector2.Angle(transform.position, grappledTo.transform.position);
        // draw a line at that angle from the player
        if (speedLowTime > .1f)
        {
            if (angle < 60)
            {
                breakGrapple();
                speedLowTime = 0;
            }
        }
    }

    void checkCollision()
    {
        float leewayAmount = 0.3f;
        Vector2 hitPoint = grappledTo.transform.position;
        // if we draw a ray from the grapple to the connected object and it hits something, then we hit something
        RaycastHit2D hit = Physics2D.Raycast(transform.position, hitPoint - (Vector2)transform.position, Vector2.Distance(hitPoint, (Vector2)transform.position) - leewayAmount, canGrappleTo); // doing 95% of the way to the connected object for some leeway
        Vector2 leeway = (hitPoint - (Vector2)transform.position).normalized * leewayAmount;
        Debug.DrawRay(transform.position, (hitPoint - (Vector2)transform.position) - leeway, Color.red, 1);
        if (hit.collider != null && hit.collider.gameObject != grappledTo)
        {
            Debug.Log("Snap!");
            breakGrapple();
        }
    }

    void checkLengthChange()
    {
        if (needShortenGrapple)
        {
            // check minDist
            if (joint.distance - (grappleLengthChangeSpeed * speedPerFrame * grappleLengthChangeSpeed) < grappleMinDist)
            {
                joint.distance = grappleMinDist;
            }
            else
            {
                joint.distance -= (grappleLengthChangeSpeed * speedPerFrame * grappleLengthChangeSpeed);
            }
            needShortenGrapple = false;
        }
        if (needLengthenGrapple)
        {
            // check maxDist
            if (joint.distance + (grappleLengthChangeSpeed * speedPerFrame * grappleLengthChangeSpeed) > grappleMaxDist)
            {
                joint.distance = grappleMaxDist;
            }
            else
            {
                joint.distance += (grappleLengthChangeSpeed * speedPerFrame * grappleLengthChangeSpeed);
            }
            needLengthenGrapple = false;
        }
    }

    void startGrapple()
    {
        if (!isGrappling())
        {
            // ray from the player to the mouse
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, mousePos - (Vector2)transform.position, grappleMaxDist, canGrappleTo);
            if (hit.distance <= grappleMaxDist && hit.collider != null)
            {
                grappling = true;
                joint = gameObject.AddComponent<SpringJoint2D>();
                joint.enableCollision = true;

                // make a gameObject that is the same size as the line render for collisions
                grappledTo = new GameObject();
                // make its position the middle of the line
                grappledTo.transform.position = hit.point;    
                // make the grappledTo's parent the thing we hit
                grappledTo.transform.SetParent(hit.collider.gameObject.transform);
                // give it a rigidbody2D
                Rigidbody2D rb = grappledTo.AddComponent<Rigidbody2D>();
                // make it kinematic
                rb.isKinematic = true;
            
                joint.connectedBody = rb;
                joint.autoConfigureDistance = true;

                joint.frequency = 1;
                joint.dampingRatio = .5f;
            }
        }
    }

    public void breakGrapple()
    {
        endGrapple();
    }

    void endGrapple()
    {
        grappling = false;
        Destroy(joint);
        Destroy(grappledTo);
        lr.enabled = false;
    }

    void drawGrapple()
    {
        lr.enabled = true;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, grappledTo.transform.position);
    }


}