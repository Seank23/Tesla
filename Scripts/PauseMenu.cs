using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    public GameObject myCanvas;
    public GameObject uiCanvas;
    private InGameManager game;
    private ControlSelector selector;
    private DialogueBox dialog;
    private LoadingScreen loading;

    public bool isPaused;
    public Button[] menuButtons = new Button[4];

    void Start()
    {
        game = FindObjectOfType<InGameManager>();
        selector = FindObjectOfType<ControlSelector>();
        dialog = FindObjectOfType<DialogueBox>();
        loading = FindObjectOfType<LoadingScreen>();
        myCanvas.SetActive(false);
        isPaused = false;
    }
	
	void Update ()
    {
        if (Input.GetButtonDown("Cancel") && !selector.selectorOpen && !dialog.dialogOpen)
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                ShowMenu();
            }
            else
            {
                Resume();
            }
        }

        if (dialog.dialogOpen)
        {
            menuButtons[0].enabled = false;
            menuButtons[1].enabled = false;
            menuButtons[2].enabled = false;
            menuButtons[3].enabled = false;
        }
        else
        {
            menuButtons[0].enabled = true;
            menuButtons[1].enabled = true;
            menuButtons[2].enabled = true;
            menuButtons[3].enabled = true;
        }
	}

    public void ShowMenu()
    {
        isPaused = true;
        myCanvas.SetActive(true);
        uiCanvas.SetActive(false);
        game.PauseGame(isPaused);
    }

    public void Resume()
    {
        isPaused = false;
        myCanvas.SetActive(false);
        uiCanvas.SetActive(true);
        game.PauseGame(isPaused);
        Audio.gameAudio.PlaySFX("btnClick1");
    }

    public void Options()
    {
        Audio.gameAudio.PlaySFX("btnClick1");
    }

    public void RestartClicked()
    {
        Audio.gameAudio.PlaySFX("btnClick1");
        dialog.ShowDialogue("Are you sure you want to restart?\nAny unsaved progress will be lost.", "No", "Yes");
        dialog.btnB.onClick.AddListener(() => { Restart(); dialog.DialogAudio(); });
    }

    public void QuitClicked()
    {
        Audio.gameAudio.PlaySFX("btnClick1");
        dialog.ShowDialogue("Are you sure you want to quit?\nAny unsaved progress will be lost.", "No", "Yes");
        dialog.btnB.onClick.AddListener(() => { Quit(); dialog.DialogAudio(); });
    }

    void Quit()
    {
        Resume();
        GameControl.control.Load(PlayerDataControl.data.playerName);
        loading.StartLoadingScreen("05_Level_Selector");
    }

    void Restart()
    {
        Resume();
        GameControl.control.Load(PlayerDataControl.data.playerName);
        loading.StartLoadingScreen(SceneManager.GetActiveScene().name);
    }
}
