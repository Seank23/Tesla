using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public GameObject canvas;
    public GameObject background;
    public GameObject progressBar;
    public Text loadingText;

    private int loadProgress = 0;

	void Start ()
    {
        canvas.SetActive(false);
        background.SetActive(false);
        progressBar.SetActive(false);
        loadingText.enabled = false;
	}

    public void StartLoadingScreen(string name)
    {
        StartCoroutine(DisplayLoadingScreen(name));
    }
	
    IEnumerator DisplayLoadingScreen(string level)
    {
        canvas.SetActive(true);
        background.SetActive(true);
        progressBar.SetActive(true);
        loadingText.enabled = true;

        progressBar.transform.localScale = new Vector3(loadProgress, progressBar.transform.localScale.y, progressBar.transform.localScale.z);
        loadingText.text = "Loading Progress: " + loadProgress + "%";

        AsyncOperation async = SceneManager.LoadSceneAsync(level);
        while (!async.isDone)
        {
            loadProgress = Mathf.FloorToInt(async.progress * 100);
            progressBar.transform.localScale = new Vector3(async.progress, progressBar.transform.localScale.y, progressBar.transform.localScale.z);
            loadingText.text = "Loading Progress: " + loadProgress + "%";
            yield return null;
        }
    }
}
