using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
    private LevelManager levelManager;

    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        Music.gameMusic.PlayMusic("menuMusic2");
    }

    public void ButtonClicked(string buttonName)
    {
        if (buttonName == "Save")
        {
            Audio.gameAudio.PlaySFX("btnClick1");
            levelManager.LoadLevel("03_Save_Game");
        }

        if (buttonName == "Load")
        {
            Audio.gameAudio.PlaySFX("btnClick1");
            levelManager.LoadLevel("04_Load_Game");
        }

        if (buttonName == "Options")
        {
            Audio.gameAudio.PlaySFX("btnClick1");
            levelManager.LoadLevel("02_Options_Menu");
        }

        if (buttonName == "Quit")
        {
            Audio.gameAudio.PlaySFX("btnClick1");
            levelManager.QuitRequest();
        }
    }
}
