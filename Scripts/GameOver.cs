using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOver : MonoBehaviour
{
    public Text textContinue;

    private LoadingScreen load;

	void Start ()
    {
        load = FindObjectOfType<LoadingScreen>();
        textContinue.enabled = false;

        StartCoroutine(ShowText());
	}
	
	void Update ()
    {
	    if(Input.GetButtonDown("Submit") && textContinue.enabled == true)
        {
            load.StartLoadingScreen("05_Level_Selector");
        }
	}

    IEnumerator ShowText()
    {
        yield return new WaitForSeconds(1);
        textContinue.enabled = true;
        StartCoroutine(FadeText(0.8f));
    }

    private IEnumerator FadeText(float time)
    {
        Color fade = textContinue.color;

        while (!Input.GetButtonDown("Submit"))
        {
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
            {
                fade.a = Mathf.Lerp(fade.a, 0.05f, t);
                textContinue.color = fade;
                yield return null;
            }

            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
            {
                fade.a = Mathf.Lerp(fade.a, 1f, t);
                textContinue.color = fade;
                yield return null;
            }
        }
    }
}
