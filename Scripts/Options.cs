using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class Options : MonoBehaviour
{
    public GameObject[] panels;
    public Dropdown resolution;
    public Dropdown antialiasing;
    public Toggle fullscreen;
    public Dictionary<string, int[]> values = new Dictionary<string, int[]>();

    private int width;
    private int height;
    private int aa;
    private bool isFullscreen;

	void Start ()
    {
        panels[0].SetActive(true);
        panels[1].SetActive(false);
        panels[2].SetActive(false);
        panels[3].SetActive(false);

        values.Add("Antialiasing", new int[] { 0, 2, 4, 8 });
        values.Add("Width", new int[] { 800, 1280, 1680, 1920 }); 
        values.Add("Height", new int[] { 600, 720, 1050, 1080 });

        resolution.onValueChanged.AddListener(delegate { ResChange(out width, out height); });
        antialiasing.onValueChanged.AddListener(delegate { DropdownChange(antialiasing, out aa); });
        fullscreen.onValueChanged.AddListener(delegate { ToggleChange(fullscreen, out isFullscreen); });

        SetValues();
    }

    void SetValues()
    {
        if(PlayerPrefs.HasKey("Antialiasing"))
        {
            aa = PlayerPrefs.GetInt("Antialiasing");
            int[] possibleAA = values["Antialiasing"];
            antialiasing.value = Array.IndexOf(possibleAA, aa);
        }

        if (PlayerPrefs.HasKey("Width"))
        {
            width = PlayerPrefs.GetInt("Width");
            int[] possibleWidths = values["Width"];
            resolution.value = Array.IndexOf(possibleWidths, width);
        }

        if (PlayerPrefs.HasKey("Fullscreen"))
        {
            if (PlayerPrefs.GetInt("Fullscreen") == 1)
                isFullscreen = true;
            else
                isFullscreen = false;

            fullscreen.isOn = isFullscreen;
        }
    }

    public void ActivatePanel(int val)
    {
        panels[0].SetActive(false);
        panels[1].SetActive(false);
        panels[2].SetActive(false);
        panels[3].SetActive(false);

        panels[val].SetActive(true);
    }

    public void Apply()
    {
        QualitySettings.antiAliasing = aa;
        Screen.SetResolution(width, height, isFullscreen);
        PlayerPrefs.SetInt("Antialiasing", aa);
        PlayerPrefs.SetInt("Width", width);
        PlayerPrefs.SetInt("Height", height);
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }

    private void ResChange(out int width, out int height)
    {
        int[] possibleWidths = values["Width"];
        int[] possibleHeights = values["Height"];
        width = possibleWidths[resolution.value];
        height = possibleHeights[resolution.value];
    }

    private void ToggleChange(Toggle target, out bool value)
    {
        if (target.isOn)
        {
            value = true;
        }
        else
        {
            value = false;
        }
    }

    private void DropdownChange(Dropdown target, out int value)
    {
        int[] possibleVals = values[target.name];
        value = possibleVals[target.value];
    }
}
