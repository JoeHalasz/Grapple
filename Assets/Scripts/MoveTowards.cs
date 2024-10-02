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
    float distance;
    float accelRate;

    [SerializeField]
    float speed;

    void Start()
    {
        

        targetPos = targetToFollow.transform.position;
        currentPos = transform.position;
        offsetPos = targetPos + currentPos;
    }

    // Update is called once per frame
    void Update()
    {

       
        currentPos = transform.position;
        targetPos = targetToFollow.transform.position;
        distance = Vector2.Distance(currentPos, targetPos);
        accelRate = speed * distance * Time.deltaTime;
        transform.position = Vector2.MoveTowards(currentPos, targetPos + offsetPos, accelRate);
    }
}
