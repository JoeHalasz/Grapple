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
    private Vector2 grappleAnimationTarget;
    // add a list of masks to accept
    private Vector2 hitPoint;
    private Vector2 currentAnimationHitPoint;
    private SpringJoint2D joint;
    private bool needShortenGrapple = false;
    private bool needLengthenGrapple = false;
    private float speedPerFrame = 1f/60f;

    public LayerMask canGrappleTo;

    private GameObject player;

    private Grapple grapple;

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
        player = GameObject.Find("Player");
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
        // if (grappling)
        // {
        //     if (needShortenGrapple)
        //     {
        //         // check minDist
        //         if (joint.distance - (grappleLengthChangeSpeed * speedPerFrame) < grappleMinDist)
        //         {
        //             joint.distance = grappleMinDist;
        //         }
        //         else
        //         {
        //             joint.distance -= (grappleLengthChangeSpeed * speedPerFrame);
        //         }
        //         needShortenGrapple = false;
        //     }
        //     if (needLengthenGrapple)
        //     {
        //         // check maxDist
        //         if (joint.distance + (grappleLengthChangeSpeed * speedPerFrame) > grappleMaxDist)
        //         {
        //             joint.distance = grappleMaxDist;
        //         }
        //         else
        //         {
        //             joint.distance += (grappleLengthChangeSpeed * speedPerFrame);
        //         }
        //         needLengthenGrapple = false;
        //     }
        // }
    }

    void LateUpdate()
    {
        if (grapple != null)
        {
            grapple.updatePosition(player.transform.position);
        }
    }

    void startGrapple()
    {
        grappling = true;
        // ray from the player to the mouse
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, mousePos - (Vector2)transform.position, grappleMaxDist, canGrappleTo);
        if (hit.collider != null)
        {
            hitPoint = hit.point;
            // make a grapple
            grapple = new Grapple(transform.position, hitPoint);
        }
        else
        {
            grappling = false;
        }
    }

    void endGrapple()
    {
        grappling = false;
        grapple.DestroyGrapple();
        grapple = null;
    }

    private class Grapple
    {
        GrappleNode head;
        double lengthBetweenNodes = 1;

        public Grapple(Vector2 startPosition, Vector2 endPosition)
        {
            double length = Vector2.Distance(startPosition, endPosition);
            // split the length into the number of nodes needed
            int numNodes = (int)(length / lengthBetweenNodes);
            numNodes = 20;
            double lengthPerNode = length / (numNodes+1);
            GrappleNode currentNode = new GrappleNode(startPosition, lengthPerNode, 0);
            head = currentNode;
            for (int i = 1; i < numNodes+1; i++)
            {
                Vector2 nextPosition = Vector2.Lerp(startPosition, endPosition, (float)(i / numNodes));
                GrappleNode nextNode = new GrappleNode(nextPosition, lengthPerNode, i);
                currentNode.setForwardNode(nextNode);
                currentNode.drawLine();
                currentNode.createSpringJoint(lengthPerNode);
                Debug.Log(currentNode.node.transform.name + " connected to " + nextNode.node.transform.name);

                currentNode.node.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

                currentNode = nextNode;
            }
            // make the last node not able to move
            currentNode.node.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

            // make the parent of the head the player
            GameObject player = GameObject.Find("Player");
            head.node.transform.parent = player.transform;
            // draw a line from the player to the current node
            LineRenderer lineRenderer = player.GetComponent<LineRenderer>();
            if (lineRenderer == null)
                lineRenderer = player.AddComponent<LineRenderer>();
            lineRenderer.SetPosition(0, player.transform.position);
            lineRenderer.SetPosition(1, head.node.transform.position);
            lineRenderer.startWidth = .1f;
            // create a spring joint between the player and the current node
            SpringJoint2D joint = player.GetComponent<SpringJoint2D>();
            if (joint == null)
                joint = player.AddComponent<SpringJoint2D>();
            joint.connectedBody = head.node.GetComponent<Rigidbody2D>();
            joint.distance = (float)lengthPerNode;
            Debug.Log("Head is " + head.node.transform.name);
        }

        public void DestroyGrapple()
        {
            head.DestroyGrappleNode();
        }

        public void updatePosition(Vector2 newPosition)
        {
            head.node.transform.position = newPosition;
            head.updatePosition();
        }

    }

    private class GrappleNode
    {
        public GameObject node;
        private GrappleNode forwardNode = null;
        private double length;
        public LineRenderer lineRenderer;
        
        // constructor
        public GrappleNode(Vector2 position, double length, int num)
        {
            this.node = new GameObject();
            node.name = "GrappleNode" + num;
            this.node.transform.position = position;
            this.length = length;
            // make sure node has a line render
            node.AddComponent<LineRenderer>();
            CircleCollider2D collider = node.AddComponent<CircleCollider2D>();
            collider.radius = (float)length/4;
            Rigidbody2D r = node.AddComponent<Rigidbody2D>();
            node.AddComponent<SpringJoint2D>();
            node.AddComponent<SpriteRenderer>();
            node.GetComponent<SpriteRenderer>().sprite = GameObject.Find("Circle").GetComponent<SpriteRenderer>().sprite;
            node.transform.localScale = new Vector2(.2f, .2f);
        }

        public void setForwardNode(GrappleNode forwardNode)
        {
            this.forwardNode = forwardNode;
        }

        // draw a line between this node and the forward node
        public void drawLine()
        {
            lineRenderer = node.GetComponent<LineRenderer>();
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, node.transform.position);
            // thickness
            lineRenderer.startWidth = .1f;
            lineRenderer.endWidth = .1f;
            lineRenderer.SetPosition(1, forwardNode.node.transform.position);
            
        }

        // create a spring joint between this node and the forward node
        public void createSpringJoint(double length)
        {
            if (forwardNode != null)
            {
                SpringJoint2D joint = node.GetComponent<SpringJoint2D>();
                joint.connectedBody = forwardNode.node.GetComponent<Rigidbody2D>();
                joint.enableCollision = false;
                joint.frequency = 1f;
                joint.dampingRatio = .5f;
                joint.distance = (float)length;
            }
        }

        public void updatePosition()
        {
            if (lineRenderer != null)
            {
                lineRenderer.SetPosition(0, node.transform.position);
                if (forwardNode != null)
                {
                    lineRenderer.SetPosition(1, forwardNode.node.transform.position);
                    forwardNode.updatePosition();
                }
            }
        }

        // fuck the GC ( have to do this so temp gets cleaned up )
        public void DestroyGrappleNode()
        {
            if (lineRenderer != null)
                Destroy(lineRenderer);
            if (node != null)
                Destroy(node);
            if (forwardNode != null)
                forwardNode.DestroyGrappleNode();
        }

    }

}
