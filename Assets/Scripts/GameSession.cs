using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField ] int playerLives = 3;
    [SerializeField] int score;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;

    void Awake()
    {
        int numGameSession = FindObjectsOfType<GameSession>().Length;
        if(numGameSession > 1){
            Destroy(gameObject);
        }else{
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start(){
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();
    }
    public void AddToScore(int pointsToAdd){
        score +=pointsToAdd;
        scoreText.text = score.ToString();
    }
    public void ProcessPlayerDeath(){
        if(playerLives > 1){
            TakeLive();
        }
        else{
            ResetGameSession();
        }
    }

    void TakeLive()
    {
        playerLives--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = playerLives.ToString();
    }

    void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
