using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public Text finalScore;
    GameController gameController;
    public GameObject HowToPLay;

    

    public void HowToPLayMenu()
    {

        HowToPLay.SetActive(true);
    }

    private void Awake()
    {
        //Time.timeScale = 1f;
        HowToPLay.SetActive(false);

    }
    private void Update()
    {
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        if (gameController.gameOver)
        {
            FinalScore();
        }

    }

    public void FinalScore()
    {
        //Time.timeScale = 0f;
        finalScore.text = ("Final Score: " + gameController.score);
    }
    public void MainMenu()

    {
        SceneManager.LoadScene(0);
    }

    public void HelpMenu()
    {
        SceneManager.LoadScene(2);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
