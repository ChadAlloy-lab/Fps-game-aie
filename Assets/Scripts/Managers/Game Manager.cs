using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public HighScores highScores;

    public TextMeshProUGUI messageText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public Image crosshair;
    public GameObject highScorePanel;
    public TextMeshProUGUI highScoresText;

    public Button newGameButton;
    public Button highScoreButton;

    public TargetHealth[] targets;
    public CoinPickup[] coins;
    public GameObject player;
    public Camera worldCamera;

    public float startTimerAmount = 3;
    private float startTimer;

    public float targetActivateTimerAmount = 1;
    private float targetActivateTimer;

    public float coinActivateTimerAmount = 1;
    private float coinActivateTimer;

  


    public float gameTimerAmount = 60;
    private float gameTimer;

    private int score = 0;

    public enum GameState
    { 
        Start,
        Playing,
        GameOver
    };

    public GameState gameState;
    public GameState State { get { return gameState; } }


    private void Awake()
    {
        gameState = GameState.GameOver;
    }

    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;

        player.SetActive(false);
        worldCamera.gameObject.SetActive(true);
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].GameManager = this;
            targets[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < coins.Length; i++)
        {
            coins[i].GameManager = this;
            coins[i].gameObject.SetActive(false);
        }
        startTimer = startTimerAmount;
        messageText.text = "Press Enter To Start";
        timerText.text = "";
        scoreText.text = "";

        highScorePanel.gameObject.SetActive(false);
        newGameButton.gameObject.SetActive(true);
        highScoreButton.gameObject.SetActive(true);
        crosshair.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }

        switch (gameState)
        {
            case GameState.Start:
                GameStateStart();
                break;

            case GameState.Playing:
                GameStatePlaying();
                break;

            case GameState.GameOver:
                GameStateGameOver();
                break;
        }
    }

    private void ActivateRandomTarget()
    {
        int randomIndex = Random.Range(0, targets.Length);
        targets[randomIndex].gameObject.SetActive(true);
    }
    private void ActivateRandomCoin()
    {
        int randomIndex = Random.Range(0, coins.Length);
        coins[randomIndex].gameObject.SetActive(true);
    }

    public void AddScore(int points)
    {
        score += points;
        scoreText.text = "Score: " + score;
    }

    public void OnNewGame()
    {
        gameState = GameState.Start;
        highScorePanel.gameObject.SetActive(false);
        newGameButton.gameObject.SetActive(false);
        highScoreButton.gameObject.SetActive(false);
    }

    public void OnHighScores()
    {
        messageText.text = "";
        highScoreButton.gameObject.SetActive(false);
        highScorePanel.gameObject.SetActive(true);


        string text = "";
        for (int i = 0; i < highScores.scores.Length; i++)
        {
            text += highScores.scores[i] + "\n";
        }
        highScoresText.text = text;
    }

    private void GameStateStart()
    {
        startTimer -= Time.deltaTime;
        messageText.text = "Get Ready " + (int)(startTimer + 1);

        

        if (startTimer < 0)
        {
            Cursor.lockState = CursorLockMode.Locked;
            messageText.text = "";

            gameState = GameState.Playing;
            gameTimer = gameTimerAmount;
            startTimer = startTimerAmount;
            score = 0; 

            highScorePanel.gameObject.SetActive(false);
            highScoreButton.gameObject.SetActive(false);
            newGameButton.gameObject.SetActive(false);
            crosshair.gameObject.SetActive(true);


            player.SetActive(true);
            worldCamera.gameObject.SetActive(false);
           
        }
    }

    private void GameStatePlaying()
    {
        gameTimer -= Time.deltaTime;
        int seconds = Mathf.RoundToInt(gameTimer);
        timerText.text = string.Format("Time: {0:D2}:{1:D2}", (seconds / 60), (seconds % 60));


        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log(highScores);
            OnHighScores();
            highScorePanel.gameObject.SetActive(true);
            crosshair.gameObject.SetActive(false);
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            highScorePanel.gameObject.SetActive(false);
            crosshair.gameObject.SetActive(true);
        }

        if (gameTimer <= 0)
        {
            Cursor.lockState= CursorLockMode.Confined;
            messageText.text = ("Game Over. Score: " + score);
            gameState = GameState.GameOver;
            player.SetActive(false);
            worldCamera.gameObject.SetActive(true);
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < coins.Length; i++)
            {
                coins[i].gameObject.SetActive(false);
            }
            highScores.AddScore(score);
            highScores.SaveScoresToFile();
            timerText.text = "";
            newGameButton.gameObject.SetActive(true);
            highScoreButton.gameObject.SetActive(true);
            crosshair.gameObject.SetActive(false);
        }

        targetActivateTimer -= Time.deltaTime;
        if (targetActivateTimer <= 0)
        {
            ActivateRandomTarget();
            targetActivateTimer = targetActivateTimerAmount;
        }

        coinActivateTimer -= Time.deltaTime;
        if (coinActivateTimer <= 0)
        {
            ActivateRandomCoin();
            coinActivateTimer = coinActivateTimerAmount;
        }

    }
    private void GameStateGameOver()
    {
        if(Input.GetKeyUp(KeyCode.Return))
        {
            gameState = GameState.Start;
            timerText.text = "";
            scoreText.text = "";
            highScorePanel.gameObject.SetActive(false);
            newGameButton.gameObject.SetActive(false);
            highScoreButton.gameObject.SetActive(false);
        }
    }
}
