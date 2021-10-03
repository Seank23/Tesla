using UnityEngine;
using Chronos;
using System.Collections;

public class FinishPortal : BaseBehaviour 
{
    public GameObject finishMesh;
    private InGameManager game;
    private Rigidbody rb;

	void Start () 
    {
        game = FindObjectOfType<InGameManager>();
        rb = GetComponent<Rigidbody>();
        finishMesh.SetActive(false);
	}
	
	void Update () 
    {
        if (time.timeScale > 0 && finishMesh.activeSelf)
        {
            transform.Rotate(0, 30 * time.deltaTime, 0, Space.World);
        }

        if (Physics.Raycast(transform.position, Vector3.down, 0.5f))
        {
            rb.isKinematic = true;
        }
    }

    public void Unlock()
    {
        finishMesh.SetActive(true);
        StartCoroutine(Fade(1f, 12f));
    }

    IEnumerator Fade(float aValue, float aTime)
    {
        Color[] colors = new Color[3];
        colors[0] = GetComponentInChildren<Renderer>().materials[0].color;
        colors[1] = GetComponentInChildren<Renderer>().materials[1].color;
        colors[2] = GetComponentInChildren<Renderer>().materials[2].color;
        
        for (float t = 0.0f; t < 1.0f; t += time.deltaTime / aTime)
        {
            colors[0].a = Mathf.Lerp(colors[0].a, aValue, t);
            colors[1].a = Mathf.Lerp(colors[1].a, aValue, t);
            colors[2].a = Mathf.Lerp(colors[2].a, aValue, t);
            GetComponentInChildren<Renderer>().materials[0].color = colors[0];
            GetComponentInChildren<Renderer>().materials[1].color = colors[1];
            GetComponentInChildren<Renderer>().materials[2].color = colors[2];
            yield return null;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player" && finishMesh.activeSelf)
        {
            StartCoroutine(game.LevelComplete());
        }
    }
}
