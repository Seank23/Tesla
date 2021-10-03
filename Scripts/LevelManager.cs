using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    private DialogueBox dialog;

    void Start()
    {
        dialog = FindObjectOfType<DialogueBox>();
    }

	// Loads specified level (string name)
	public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
	}
	
	//Quits game
	public void QuitRequest()
    {
        dialog.ShowDialogue("Are you sure you want to quit the game?", "No", "Yes");
        dialog.btnB.onClick.AddListener(() => { Quit(); dialog.DialogAudio(); });
	}

    void Quit()
    {
        print("Level Manager: QuitRequest called");
        dialog.CloseDialog();
        Application.Quit();
    }
}