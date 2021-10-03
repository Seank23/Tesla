using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class StatsScreen : MonoBehaviour 
{
    public Text time;
    public Text goldCollected;
    public GameObject itemsPanel;
    public Text totalGold;
    public Text itemText;
    public Text continueToMenu;
    public Image fadeScreen;
    private LevelManager level;

    public int itemsGold;
    private string displayedTime;
    private float levelTime;
    private bool displayFinished = false;

	void Start () 
    {
        level = FindObjectOfType<LevelManager>();
        continueToMenu.enabled = false;

        levelTime = PlayerDataControl.data.levelTimes[PlayerDataControl.data.currentLevel][PlayerDataControl.data.levelTimes[PlayerDataControl.data.currentLevel].Count - 1];
        int minutes = Mathf.FloorToInt(levelTime / 60);
        double seconds = Math.Round(levelTime % 60, 3);
        if(seconds >= 0 && seconds < 10)
        {
            displayedTime = "Time: 0" + minutes + ":0" + seconds;
        }
        else
        {
            displayedTime = "Time: 0" + minutes + ":" + seconds;
        }

        StartCoroutine(DisplayData());
	}
	
	void Update () 
    {
        if (Input.GetButtonDown("Submit") && displayFinished)
        {
            if (PlayerDataControl.data.playerName.Length > 0)
            {
                PlayerDataControl.data.gold += (itemsGold + PlayerDataControl.data.currentLevelGold);
                GameControl.control.Save(PlayerDataControl.data.playerName, "data");
            }

            PlayerDataControl.data.currentLevelGold = 0;
            PlayerDataControl.data.currentLevel = "";
            PlayerDataControl.data.collectedItems.Clear();

            StartCoroutine(FadeOut(2));
        }
	}

    IEnumerator DisplayData()
    {
        yield return new WaitForSeconds(1);
        time.text = displayedTime;

        yield return new WaitForSeconds(1);

        goldCollected.text = "Gold Collected: 0G";
        int displayedGold = 0;
        while(displayedGold < PlayerDataControl.data.currentLevelGold)
        {
            yield return new WaitForSeconds(0.02f);
            displayedGold++;
            goldCollected.text = "Gold Collected: " + displayedGold + "G";
        }

        float anchorMaxY = 1f;
        float anchorMinY = 0.9f;
        foreach(string item in PlayerDataControl.data.collectedItems)
        {
            yield return new WaitForSeconds(0.5f);

            Text itemCollected = Instantiate(itemText, transform.position, Quaternion.identity) as Text;
            itemCollected.text = item + "                            " + PlayerDataControl.data.trashValues[item] + "G";
            RectTransform itemRect = itemCollected.GetComponent<RectTransform>();
            itemRect.SetParent(itemsPanel.transform, false);
            itemRect.anchorMax = new Vector2(1, anchorMaxY);
            itemRect.anchorMin = new Vector2(0, anchorMinY);
            itemsGold += PlayerDataControl.data.trashValues[item];
            anchorMaxY -= 0.1f;
            anchorMinY -= 0.1f;
        }

        yield return new WaitForSeconds(0.5f);

        totalGold.text = "Total Gold: " + (PlayerDataControl.data.currentLevelGold + itemsGold).ToString() + "G";

        yield return new WaitForSeconds(1);

        continueToMenu.enabled = true;
        displayFinished = true;
        StartCoroutine(FadeText(0.8f));
    }

    private IEnumerator FadeOut(float time)
    {
        Color fade = fadeScreen.color;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
        {
            fade.a = Mathf.Lerp(fade.a, 1, t);
            fadeScreen.color = fade;
            yield return null;
        }
        level.LoadLevel("05_Level_Selector");
    }

    private IEnumerator FadeText(float time)
    {
        Color fade = continueToMenu.color;

        while (!Input.GetButtonDown("Submit"))
        {
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
            {
                fade.a = Mathf.Lerp(fade.a, 0.05f, t);
                continueToMenu.color = fade;
                yield return null;
            }

            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
            {
                fade.a = Mathf.Lerp(fade.a, 1f, t);
                continueToMenu.color = fade;
                yield return null;
            }
        }
    }
}
