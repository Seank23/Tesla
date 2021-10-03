using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraSmooth : MonoBehaviour
{
    public GameObject start;
    public GameObject finish;
    public Image panel;

    private float transitionDuration = 10f;
	
	void Start ()
    {
        StartCoroutine(Fade(0));
        StartCoroutine(Transition());
	}

    IEnumerator Transition()
    {
        float t = 0.0f;
        while (t < 1.0f)
        {
            if(t > 0.7f && t < 0.72f)
            {
                StartCoroutine(Fade(1));
            }
            t += Time.deltaTime * (Time.timeScale / transitionDuration);
            transform.position = Vector3.Lerp(start.transform.position, finish.transform.position, t);
            yield return 0;
        }
    }

    IEnumerator Fade(float target)
    {
        Color fadePanel = panel.color;
        float t = 0.0f;
        while(t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / 100);
            fadePanel.a = Mathf.Lerp(fadePanel.a, target, t);
            panel.color = fadePanel;
            yield return 0;
        }
    }
}
