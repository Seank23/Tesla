using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class Save : MonoBehaviour
{
    public Text saveNameText;
    public Text saveTable;
    public LoadingScreen loadingScreen;
    public Button cancel;
    public Button submitName;
    public InputField input;
    private DialogueBox dialog;
    private LevelManager levelManager;

    public string saveName;
    public List<string> listOfSaves = new List<string>();

    void Start()
    {
        dialog = FindObjectOfType<DialogueBox>();
        levelManager = FindObjectOfType<LevelManager>();
        float index = 0.1f;
        string[] saves = GetPreviousSaves().Split('\n');
        List<string> savesL = saves.ToList();
        savesL.RemoveAt(saves.Length - 1);
        saves = savesL.ToArray();

        foreach(string save in saves)
        {
            Text name = Instantiate(saveNameText, Vector3.zero, Quaternion.identity) as Text;
            name.name = save;
            if(save.Length > 0)
            {
                RectTransform rectTransform = name.GetComponent<RectTransform>();
                rectTransform.SetParent(saveTable.transform, false);
                rectTransform.anchorMax = new Vector2(0.5f, (1 - index));
                rectTransform.anchorMin = new Vector2(0.5f, (1 - index));
                name.text = save;
                index += 0.1f;
            }
            else
            {
                Destroy(name);
            }
        }
    }

    void Update()
    {
        if(dialog.dialogOpen)
        {
            cancel.enabled = false;
            submitName.enabled = false;
            input.enabled = false;
        }
        else
        {
            cancel.enabled = true;
            submitName.enabled = true;
            input.enabled = true;
        }
    }

    private string GetPreviousSaves()
    {
        string previousSaves = PlayerPrefs.GetString("SAVES");
        return previousSaves;
    }

    public void SubmitName(InputField input)
    {
        if (input.text.Length > 0)
        {
            saveName = input.text;
            input.text = "";
            if (listOfSaves.Contains(saveName) || GetPreviousSaves().Contains(saveName))
            {
                Audio.gameAudio.PlaySFX("unable");
                dialog.ShowDialogue("The save name '" + saveName + "' already exists", "Cancel", "OK");
                dialog.btnB.onClick.AddListener(() => { dialog.CloseDialog(); });
            }
            else
            {
                Audio.gameAudio.PlaySFX("btnClick1");
                dialog.ShowDialogue("Are you sure you wish to create this save?\nSave Name: " + saveName, "No", "Yes");
                dialog.btnB.onClick.AddListener(() => { CreateSave(input); });
            } 
        }
    }

    public void CreateSave(InputField input)
    {
        listOfSaves.Add(saveName);
        saveTable.text += listOfSaves[listOfSaves.Count - 1] + "\n";

        string temp = "";
        foreach (string save in listOfSaves)
        {
            temp += save + "\n";
        }
        PlayerPrefs.SetString("SAVES", GetPreviousSaves() + temp);

        PlayerDataControl.data.GetPlayerName(saveName);
        GameControl.control.Save(saveName, "data");
        loadingScreen.StartLoadingScreen("05_Level_Selector");
    }

    public void Cancel()
    {
        Audio.gameAudio.PlaySFX("btnClick2");
        levelManager.LoadLevel("01_Main_Menu");
    }
}
