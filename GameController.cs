using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject[] hazards;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    public AudioSource winMusic;
    public AudioSource loseMusic;

    public Text ScoreText;
    public Text RestartText;
    public Text GameOverText;
    public Text winText;
   

    private bool gameOver;
    private bool restart;
    private int score;

    private float timer;
    private bool canCount = true;
    private bool doOnce = false;
    [SerializeField] private Text uitext;
    [SerializeField] private float mainTimer;

    void Start ()
    {
        timer = mainTimer;
        gameOver = false;
        restart = false;
        RestartText.text = "";
        GameOverText.text = "";
        winText.text = "";
        score = 0;
        UpdateScore();
        StartCoroutine (SpawnWaves());

    }

   void Update()
    {
        if (timer >= 0.0f && canCount)
        {
            timer -= Time.deltaTime;
            uitext.text = timer.ToString("F");
        }
        else if (timer <= 0.0f && !doOnce)
        {
            canCount = false;
            doOnce = true;
            uitext.text = "0.00";
            timer = 0.0f;
            gameOver = true;

        }

        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                SceneManager.LoadScene("Munnis_Alison_DIG3480_Challenge3");
            }
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();

        }
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        { 
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds (waveWait);

            if (gameOver)
            { 
                //GetComponent<AudioSource>().Stop();
                //loseMusic.Play();
                RestartText.text = "Press 'N' for new game";
                restart = true;
                GameOver();
                break;

            }
           
        }

    }

    public void ResetBtn()
    {
        timer = mainTimer;
        canCount = true;
        doOnce = false;
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    public void GameOver ()
    {
        GameOverText.text = "GAME CREATED BY ALISON MUNNIS";
        gameOver = true;
        GetComponent<AudioSource>().Stop();
        loseMusic.Play();

        if (score >= 150)
        {
            loseMusic.Stop();
            winMusic.Play();
        }
    }

    void UpdateScore()
    {
        ScoreText.text = "Points: " + score;
        if (score >= 150)
        {
            winText.text = "You win! GAME CREATED BY ALISON MUNNIS ";
            //GetComponent<AudioSource>().Stop();
            //loseMusic.Stop();
            //winMusic.Play();
            gameOver = true;
            restart = true;
        }
    }

}
