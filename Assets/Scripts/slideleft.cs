using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slideleft : MonoBehaviour
{
    Rigidbody2D tube;
    // Start is called before the first frame update
    void Start()
    {
        tube = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        tube.velocity=(new Vector2(-5, 0));
    }
}
