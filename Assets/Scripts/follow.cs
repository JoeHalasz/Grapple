using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class follow : MonoBehaviour
{
    GameObject player;
    Rigidbody2D target;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
    }

    // Update is called once per frame
    void Update()
    {
        float step = Vector2.Distance (transform.position, player.transform.position) * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position +new Vector3(-1,1.5f), step);
    }
}
