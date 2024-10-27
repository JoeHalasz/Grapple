using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreColliderChecker : MonoBehaviour
{

    [SerializeField]
    public AudioClip addPointSound;

    Death dead;
    private void Start()
    {
        dead = GetComponent<Death>();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !collision.gameObject.GetComponent<Death>().isDead)
        {
            ScoreManager.instance.AddPoint();

            SoundFxManager.instance.PlaySFXClip(addPointSound, transform);
            // Debug.Log("NO LONGER COLLIDING");
        }
    }
}
