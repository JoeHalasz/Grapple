using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : MonoBehaviour
{

    [SerializeField]
    public float animationTime = 0.5f;
    [SerializeField]
    public int grappleMaxDist = 50;
    [SerializeField]
    public int grappleMinDist = 10;
    [SerializeField]
    public float grappleLengthChangeSpeed = .25f;
    private int skipFramesForLengthChange = 0;

    private GameObject hand; // the object that the grapple will shoot out of

    private bool grappling = false;
    private LineRenderer lr;
    private Vector2 grappleAnimationTarget;
    // add a list of masks to accept
    private Vector2 hitPoint;
    private Vector2 currentAnimationHitPoint;
    private HingeJoint2D joint;
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
        player = GameObject.Find("Player");
        hand = player.transform.Find("Hand").gameObject;
        if (player.transform.Find("Hand") == null)
        {
            Debug.LogError("Player object must have a child object named Hand");
        }
        
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
        if (Input.GetKey(KeyCode.W) && grappling && skipFramesForLengthChange == 0)
        {
            needShortenGrapple = true;
        }
        // if the player holds s, lengthen the grapple
        if (Input.GetKey(KeyCode.S) && grappling && skipFramesForLengthChange == 0)
        {
            needLengthenGrapple = true;
        }
    }

    void FixedUpdate()
    {
        if (grappling)
        {
            int changeBy = grappleLengthChangeSpeed < 1 ? 1 : (int)grappleLengthChangeSpeed;
            if (needShortenGrapple)
            {
                grapple.shortenLength(changeBy);
                needShortenGrapple = false;
                skipFramesForLengthChange = (int)(1 / grappleLengthChangeSpeed);
            }
            if (needLengthenGrapple)
            {
                grapple.addLength(changeBy);
                needLengthenGrapple = false;
                skipFramesForLengthChange = (int)(1 / grappleLengthChangeSpeed);
            }
        }
        if (skipFramesForLengthChange > 0)
        {
            skipFramesForLengthChange--;
        }
    }

    void LateUpdate()
    {
        if (grapple != null)
        {
            grapple.updateLRs();
        }
    }

    void startGrapple()
    {
        if (grapple == null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, mousePos - (Vector2)transform.position, grappleMaxDist, canGrappleTo);
            if (hit.collider != null)
            {
                hitPoint = hit.point;
                grappling = true;
                Vector2 grappleStartPos = hand.transform.position;
                grapple = new Grapple(grappleStartPos, hitPoint, (int)grappleMinDist, (int)grappleMaxDist);
            }
        }
    }

    void endGrapple()
    {
        grappling = false;
        if (grapple != null)
        {
            foreach (GameObject go in grapple.Nodes)
            {
                Destroy(go);
            }
            grapple.Nodes.Clear();
        }
        grapple = null;
    }


    private class Grapple
    {
        GameObject player;
        GameObject grappleGroup;

        double distBetweenPoints = 0.2;
        double nodeMass = .1;        
        public List<GameObject> Nodes = new List<GameObject>();

        int minLength;
        int maxLength;

        public Grapple(Vector2 startPos, Vector2 endPos, int minLength, int maxLength)
        {
            this.minLength = minLength;
            this.maxLength = maxLength;
            player = GameObject.Find("Player");
            grappleGroup = new GameObject();
            grappleGroup.transform.SetParent(player.transform);
            // calculate the points that the colliders, joints and LRs will start at
            float dist = Vector2.Distance(startPos, endPos);
            int numPoints = (int)(dist / distBetweenPoints);
            // get the sprite from the "Circle" game object
            GameObject lastNode = player.GetComponent<Rigidbody2D>().gameObject;
            for (int i = 1; i < numPoints+1; i++)
            {
                Vector2 pos = Vector2.Lerp(startPos, endPos, (float)i / numPoints);
                GameObject node = makeGrappleNode(pos, lastNode);
                node.transform.name = "Grapple" + i;
                Nodes.Add(node);
                lastNode = node;
            }
            makeNodeFinalNode(lastNode);
            Debug.Log("Grapple created with " + Nodes.Count + " nodes");
        }

        private GameObject makeGrappleNode(Vector2 pos, GameObject lastNode)
        {
            GameObject node = new GameObject();
            node.transform.position = pos;
            node.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            node.transform.SetParent(grappleGroup.transform);
            node.AddComponent<Rigidbody2D>();
            node.GetComponent<Rigidbody2D>().mass = (float)nodeMass;
            CircleCollider2D cc = node.AddComponent<CircleCollider2D>();
            cc.radius = (float)distBetweenPoints / 2.0f;
            node.AddComponent<HingeJoint2D>();
            node.GetComponent<HingeJoint2D>().connectedBody = lastNode.GetComponent<Rigidbody2D>();
            LineRenderer lr = node.AddComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.SetPosition(0, lastNode.transform.position);
            lr.SetPosition(1, node.transform.position);
            lr.startWidth = 0.1f;
            lr.endWidth = 0.1f;
            // make it black
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.startColor = Color.black;
            lr.endColor = Color.black;
            return node;
        }

        private void makeNodeFinalNode(GameObject node)
        {
            node.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            node.GetComponent<Rigidbody2D>().mass = 100000;
        }

        public void updateLRs()
        {
            // loop through all the nodes and update the line renderers
            for (int i = 0; i < Nodes.Count-1; i++)
            {
                LineRenderer lr = Nodes[i].GetComponent<LineRenderer>();
                lr.SetPosition(0, Nodes[i].transform.position);
                lr.SetPosition(1, Nodes[i + 1].transform.position);
            }
        }

        public void addLength(float amount)
        {
            for (int i = 0; i < (int)amount; i++)
            {
                if (Nodes.Count >= maxLength)
                {
                    return;
                }
                // break node 0 from node 1
                HingeJoint2D front = Nodes[0].GetComponent<HingeJoint2D>();
                Vector2 oldPos = Nodes[0].transform.position;
                Rigidbody2D oldConnection = front.connectedBody;
                HingeJoint2D next = Nodes[1].GetComponent<HingeJoint2D>();
                // get dist between node 0 and 1
                Vector2 diff = Nodes[0].transform.position - Nodes[1].transform.position;
                Vector2 newPos = new Vector2(Nodes[0].transform.position.x + diff.x/2.0f, Nodes[0].transform.position.y + diff.y/2.0f);
                // make a new node at the old position of node 0, connect it inbetween node 0 and 1, then put it in the list at index 1
                GameObject newNode = makeGrappleNode(newPos, Nodes[1]);
                newNode.transform.name = "GrappleAdded";
                Nodes.Insert(0, newNode);
                // connect node 0 to the new node
                front.connectedBody = newNode.GetComponent<Rigidbody2D>();
                newNode.GetComponent<HingeJoint2D>().connectedBody = oldConnection;
                // move the transform of the old body(player) by diff
                oldConnection.transform.position += new Vector3(diff.x, diff.y, 0)/2.0f;
                // move the grappleGroup by -diff
                grappleGroup.transform.position -= new Vector3(diff.x, diff.y, 0)/2.0f;
                // fix the LRs
                updateLRs();
            }
            Debug.Log("Grapple now has " + Nodes.Count + " nodes");
        }


        // returns if the grapple is still valid
        public void shortenLength(float amount)
        {
            for (int i = 0; i < (int)amount; i++)
            {
                if (Nodes.Count <= minLength)
                    break;
                // break node 0 from node 1
                HingeJoint2D front = Nodes[0].GetComponent<HingeJoint2D>();
                HingeJoint2D next = Nodes[1].GetComponent<HingeJoint2D>();
                Vector2 diff = Nodes[0].transform.position - Nodes[1].transform.position;
                // set the next's connected body to the front's connected body
                next.connectedBody = front.connectedBody;
                // remove the front node
                Destroy(Nodes[0]);
                Nodes.RemoveAt(0);
                // move the transform of the old body(player) by diff
                next.connectedBody.transform.position -= new Vector3(diff.x, diff.y, 0);
                Debug.Log(next.connectedBody.transform.name);
                // move the grappleGroup by -diff
                grappleGroup.transform.position += new Vector3(diff.x, diff.y, 0);
                // fix the LRs
                updateLRs();
            }
            Debug.Log("Grapple now has " + Nodes.Count + " nodes");
            return;
        }


        public void shortenLength(float amount)
        {
            // maybe just shorten the dist between nodes?
        }
    }
    

}
