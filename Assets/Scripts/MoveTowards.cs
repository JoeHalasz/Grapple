using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MoveTowards : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject targetToFollow;
    Vector2 targetPos;
    Vector2 currentPos;
    Vector2 offsetPos;
    float maxDistance;

    [SerializeField]
    float speed;

    void Start()
    {
        maxDistance = speed * Time.deltaTime;

        targetPos = targetToFollow.transform.position;
        currentPos = transform.position;
        offsetPos = targetPos + currentPos;
    }

    // Update is called once per frame
    void Update()
    {
        currentPos = transform.position;
        targetPos = targetToFollow.transform.position;
        transform.position = Vector2.MoveTowards(currentPos, targetPos + offsetPos, maxDistance);
    }
}
