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
    private Vector2 hitPoint;
    private Vector2 currentAnimationHitPoint;
    private SpringJoint2D joint;
    private bool needShortenGrapple = false;
    private bool needLengthenGrapple = false;
    private float speedPerFrame = 1f/60f;

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
            Debug.Log("LineRenderer not found");
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
            if (needShortenGrapple)
            {
                // check minDist
                if (joint.distance - (grappleLengthChangeSpeed * speedPerFrame) < grappleMinDist)
                {
                    joint.distance = grappleMinDist;
                }
                else
                {
                    joint.distance -= (grappleLengthChangeSpeed * speedPerFrame);
                }
                needShortenGrapple = false;
            }
            if (needLengthenGrapple)
            {
                // check maxDist
                if (joint.distance + (grappleLengthChangeSpeed * speedPerFrame) > grappleMaxDist)
                {
                    joint.distance = grappleMaxDist;
                }
                else
                {
                    joint.distance += (grappleLengthChangeSpeed * speedPerFrame);
                }
                needLengthenGrapple = false;
            }
        }
    }

    void LateUpdate()
    {
        if (grappling)
        {
            drawGrapple();
        }
    }


    void startGrapple()
    {
        grappling = true;
        // ray from the player to the mouse
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, mousePos - (Vector2)transform.position, grappleMaxDist, canGrappleTo);
        Debug.Log(mousePos);
        Debug.Log(hit.point);
        if (hit.collider != null)
        {
            hitPoint = hit.point;
            joint = gameObject.AddComponent<SpringJoint2D>();
            joint.enableCollision = true;

            joint.connectedAnchor = hitPoint;
            
            joint.autoConfigureDistance = true;

            joint.frequency = 1f;
            joint.dampingRatio = .5f;
            
        }
    }

    void endGrapple()
    {
        grappling = false;
        Destroy(joint);
        lr.enabled = false;
    }

    void drawGrapple()
    {
        lr.enabled = true;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, hitPoint);
    }


}
