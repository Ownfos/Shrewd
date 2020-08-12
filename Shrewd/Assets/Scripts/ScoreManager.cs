using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Records current score and update corresponding UI's text content
public class ScoreManager : MonoBehaviour
{
    private int score = 0;
    private Text scoreText;

    void Start()
    {
        scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>();
        scoreText.text = $"{score}";
    }

    public void IncreaseScore(int score)
    {
        this.score += score;
        scoreText.text = $"{this.score}";
    }

    // Invoked when the player is dead (referenced as player object's Destructible component's callback).
    // Save current score to info object to pass the value to the next scene (ScoreRoom).
    public void MoveToScoreScene()
    {
        GameObject.FindGameObjectWithTag("InfoObject").GetComponent<InfoObject>().score = score;

        // Deactivate other background color manipulating component
        Destroy(Camera.main.GetComponent<BackgroundColorController>());

        // Since PlayerController will change the background color to red and lower the timescale when it gets hit,
        // we need to revert the color and the timescale to a normal state.
        // Note that the fading effect used in here translates background color from black to white.
        Camera.main.backgroundColor = Color.black;
        Time.timeScale = 1.0f;
        StartCoroutine("Fade");
    }

    // Smoothly change the background color from black to white,
    // then load the score viewing scene.
    private IEnumerator Fade()
    {
        while (Camera.main.backgroundColor.r < 0.99f)
        {
            Camera.main.backgroundColor = Color.Lerp(Color.white, Camera.main.backgroundColor, 0.95f);
            yield return null;
        }
        SceneManager.LoadScene("ScoreRoom");
    }
}
