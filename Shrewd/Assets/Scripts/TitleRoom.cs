using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleRoom : MonoBehaviour
{
    private bool unfading = true;
    private bool loading = false;

    void Start()
    {
        Application.targetFrameRate = 60;
        StartCoroutine("UnFade");
    }

    // Update is called once per frame
    void Update()
    {
        // Do not allow control while scene transition effect is still ongoing
        if(!unfading && !loading)
        {
            // Start game
            if(Input.GetKeyDown(KeyCode.Space))
            {
                loading = true;
                StartCoroutine("Fade");
            }

            // Exit game
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }

    // Invoked when title scene is loaded.
    // Smoothly translates background color from black to white.
    private IEnumerator UnFade()
    {
        while (Camera.main.backgroundColor.r < 0.95f)
        {
            Camera.main.backgroundColor = Color.Lerp(Color.white, Camera.main.backgroundColor, 0.95f);
            yield return null;
        }
        unfading = false;
    }

    // Invoked when player decides to start a new game.
    // Smoothly translates background coor from white to black.
    private IEnumerator Fade()
    {
        while(Camera.main.backgroundColor.r > 0.01f)
        {
            Camera.main.backgroundColor = Color.Lerp(Color.black, Camera.main.backgroundColor, 0.95f);
            yield return null;
        }
        SceneManager.LoadScene("Gameplay");
    }
}
