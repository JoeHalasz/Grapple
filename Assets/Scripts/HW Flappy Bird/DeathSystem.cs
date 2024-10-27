using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death1 : MonoBehaviour
{
    [SerializeField]
    public AudioClip deathNoise;

    bool firstCheck;

    Death dead;
    GameObject inGameScoreText;
    GameObject deathMenu;

   public TextMeshProUGUI deathText;
    public TextMeshProUGUI scoreText;

    private void Start()
    {
        dead = GameObject.Find("Player").GetComponent<Death>();
        inGameScoreText = GameObject.Find("GameScoreText");
        deathMenu = GameObject.Find("DeathMenu");



        deathMenu.SetActive(false);
        inGameScoreText.SetActive(true);

        firstCheck = true;

    }

    // Update is called once per frame
    void Update()
    {
        if(firstCheck && dead.isDead)
        {

            SoundFxManager.instance.PlaySFXClip(deathNoise, transform);

            GetComponent<StopMovement>().FreezePos();
            GameObject.Find("PipeManager").GetComponent<SpawnPipe>().willSpawn = false;

            deathText.text = scoreText.text;

            inGameScoreText.SetActive(false);
            deathMenu.SetActive(true);

            firstCheck = false;
            
            Debug.Log("Is working");
        }
        
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
