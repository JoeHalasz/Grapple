using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orbFollow : MonoBehaviour
{
    GameObject player;
    int speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        Vector2 orbMove = player.transform.position + new Vector3(-1, 1, 0);
        transform.position = Vector2.MoveTowards(transform.position, orbMove, step);
    }
}
