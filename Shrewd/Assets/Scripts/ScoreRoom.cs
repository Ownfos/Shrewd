using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreRoom : MonoBehaviour
{
    void Start()
    {
        var score = GameObject.FindGameObjectWithTag("InfoObject").GetComponent<InfoObject>().score;
        GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>().text = $"{score}";
        StartCoroutine("ReturnToTitle");
    }

    // Invoked when ScoreRoom scene is loaded.
    // show the score for 3 seconds and then start fading.
    // Load TitleRoom scene when fading is done.
    private IEnumerator ReturnToTitle()
    {
        yield return new WaitForSeconds(3f);

        while (Camera.main.backgroundColor.r > 0.01f)
        {
            Camera.main.backgroundColor = Color.Lerp(Color.black, Camera.main.backgroundColor, 0.95f);
            yield return null;
        }
        SceneManager.LoadScene("TitleRoom");
    }
}
