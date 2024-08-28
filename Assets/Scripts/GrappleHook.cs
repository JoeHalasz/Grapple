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

    private GameObject hand; // the object that the grapple will shoot out of

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
                grapple = new Grapple(grappleStartPos, hitPoint);
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

        double distBetweenPoints = 0.15;
        double nodeMass = .35;
        
        public List<GameObject> Nodes = new List<GameObject>();

        public Grapple(Vector2 startPos, Vector2 endPos)
        {
            player = GameObject.Find("Player");
            // calculate the points that the colliders, joints and LRs will start at
            float dist = Vector2.Distance(startPos, endPos);
            int numPoints = (int)(dist / distBetweenPoints);
            // get the sprite from the "Circle" game object
            Sprite sprite = GameObject.Find("Circle").GetComponent<SpriteRenderer>().sprite;
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
            node.transform.SetParent(player.transform);
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
    }
    

}
