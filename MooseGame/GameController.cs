using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public float startDelay;
    public float enemyDelay;
    public float waveDelay;
    public float difficultyScaling;
    public int enemiesInWave;
    public GameObject[] enemyPrefabs;    
    public Transform[] lanes;

    private float TimeScaling = 3;

    public float score;
    public Text scoreText;

    //Use Bool, if moose hit, then game over screen
    public bool gameOver = false;
    public GameObject panel;

    private void Awake()
    {
        gameOver = false;
    }
    void Start()
    {

        Application.targetFrameRate = 60;

        gameOver = false;
        panel.SetActive(false);
        StartCoroutine(SpawnWaves());
        StartCoroutine(Threat());
        score = 0; // score is intially set to 0
        UpdateScore(); // Calls UpdateScore function to set Score with the new value of 0
    }

    private void Update()
    {
        GameOverDisplayPanel();
    }


    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startDelay);
        while (true) //spawn a wave
        {
            for (int i = 0; i < enemiesInWave; i++) //Added threat level to this so it spawns more units per wave
            {
                Vector3 spawnPosition = lanes[Random.Range(0, lanes.Length)].position; //Place at which they spawn x is a range, y is set.
                Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPosition, Quaternion.identity); //Randomize Enemy
                //Randomize position
                yield return new WaitForSeconds(enemyDelay);
            }            
            yield return new WaitForSeconds(waveDelay);
        }
    }

    IEnumerator Threat()
    {
        while (enemyDelay > .1)
        {
            yield return new WaitForSeconds(TimeScaling);
            enemyDelay = enemyDelay - difficultyScaling;


            
        }
        

    }

    public void AddPoints(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }
    private void UpdateScore() // Function used to call code when needed elsewhere in program
    {
        scoreText.text = ("Score: " + score); // Defines what is display on screen using text function
    }
    public void GameOver()
    {
        Application.Quit();
    }
    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }
    public void SwitchGameOver()
    {
        gameOver = !gameOver;
    }
    public void GameOverDisplayPanel()
    {
        if (gameOver)
            panel.SetActive(true);
    }


}
