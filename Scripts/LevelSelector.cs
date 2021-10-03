using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class LevelSelector : MonoBehaviour
{
    private LoadingScreen loading;
    private LevelManager levelManager;

    public Text welcome;
    public Text[] stats;
    public Text timeText;
    public GameObject levelTimesHolder;
    public int timesNumberDisplayed;

	void Start ()
    {
        loading = FindObjectOfType<LoadingScreen>();
        levelManager = FindObjectOfType<LevelManager>();
        DisplayMenuStats();
        Music.gameMusic.PlayMusic("menuMusic1");
	}

    void DisplayMenuStats()
    {
        welcome.text = "Welcome back, " + PlayerDataControl.data.playerName;

        stats[0].text = PlayerDataControl.data.gold.ToString();
        stats[1].text = PlayerDataControl.data.resets.ToString();
        stats[2].text = PlayerDataControl.data.controlDict["pause"].ToString();
        stats[3].text = PlayerDataControl.data.controlDict["fast"].ToString();
        stats[4].text = PlayerDataControl.data.controlDict["record"].ToString();
        stats[5].text = PlayerDataControl.data.controlDict["rewind"].ToString();
        stats[6].text = PlayerDataControl.data.controlDict["slow"].ToString();
    }

    public void PlayLevel(string levelToLoad)
    {
        Audio.gameAudio.PlaySFX("btnClick1");
        loading.StartLoadingScreen(levelToLoad);
    }

    public void Cancel()
    {
        Audio.gameAudio.PlaySFX("btnClick2");
        levelManager.LoadLevel("01_Main_Menu");
    }

    public void DisplayLevelTimes(string levelName)
    {
        List<float> times = PlayerDataControl.data.levelTimes[levelName];
        times.Sort();
        int index = 0;

        foreach (float time in times)
        {
            if (index < timesNumberDisplayed)
            {
                Text currentTime = Instantiate(timeText, transform.position, Quaternion.identity) as Text;
                RectTransform timeRect = currentTime.GetComponent<RectTransform>();
                timeRect.SetParent(levelTimesHolder.transform, false);

                int minutes = Mathf.FloorToInt(time / 60);
                double seconds = Math.Round(time % 60, 3);
                if (seconds >= 0 && seconds < 10)
                {
                    currentTime.text = "0" + minutes + ":0" + seconds;
                }
                else
                {
                    currentTime.text = "0" + minutes + ":" + seconds;
                }
                index++;
            }
        }

        if (index < timesNumberDisplayed)
        {
            for (int i = 0; i < timesNumberDisplayed - index; i++)
            {
                Text currentTime = Instantiate(timeText, transform.position, Quaternion.identity) as Text;
                RectTransform timeRect = currentTime.GetComponent<RectTransform>();
                timeRect.SetParent(levelTimesHolder.transform, false);
            }
        }
    }

    public void ClearTimes()
    {
        foreach (Transform time in levelTimesHolder.transform)
            Destroy(time.gameObject);
    }
}
