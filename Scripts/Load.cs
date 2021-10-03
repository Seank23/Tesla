using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class Load : MonoBehaviour
{
    public Toggle saveToggle;
    public ToggleGroup group;
    public GameObject loadTable;
    public LoadingScreen loadingScreen;
    public Button load;
    public Button cancel;
    public Button delete;
    private List<Toggle> toggles;
    private DialogueBox dialog;
    private LevelManager levelManager;

    private string GetPreviousSaves()
    {
        string previousSaves = PlayerPrefs.GetString("SAVES");
        return previousSaves;
    }

    void Start()
    {
        dialog = FindObjectOfType<DialogueBox>();
        levelManager = FindObjectOfType<LevelManager>();
        group = loadTable.GetComponent<ToggleGroup>();
        toggles = new List<Toggle>();

        float index = 0.1f;
        string[] saves = GetPreviousSaves().Split('\n');
        List<string> savesL = saves.ToList();
        savesL.RemoveAt(saves.Length - 1);
        saves = savesL.ToArray();

        foreach(string save in saves)
        {
            Toggle toggle = Instantiate(saveToggle, Vector3.zero, Quaternion.identity) as Toggle;
            toggle.name = save;
            if(toggle.name.Length > 0)
            {
                toggle.group = group;
                RectTransform rectTransform = toggle.GetComponent<RectTransform>();
                rectTransform.SetParent(loadTable.transform, false);
                rectTransform.anchorMax = new Vector2(0.1f, (1 - index));
                rectTransform.anchorMin = new Vector2(0.1f, (1 - index));
                Text toggleText = toggle.GetComponentInChildren<Text>();
                toggleText.text = save;
                toggles.Add(toggle);
                index += 0.1f;
            }
            else
            {
                Destroy(toggle);
            }
        }
    }

    void Update()
    {
        if(dialog.dialogOpen)
        {
            load.enabled = false;
            cancel.enabled = false;
            delete.enabled = false;
        }
        else
        {
            load.enabled = true;
            cancel.enabled = true;
            delete.enabled = true;
        }
    }

    public void LoadClicked()
    {
        foreach (Toggle toggle in toggles)
        {
            if(toggle.isOn)
            {
                Audio.gameAudio.PlaySFX("btnClick1");
                dialog.ShowDialogue("Are you sure you want to load save '" + toggle.name + "'?", "No", "Yes");
                dialog.btnB.onClick.AddListener(() => { LoadSave(); });
            }
        }
    }

    public void DeleteClicked()
    {
        foreach (Toggle toggle in toggles)
        {
            if(toggle.isOn)
            {
                Audio.gameAudio.PlaySFX("btnClick1");
                dialog.ShowDialogue("Are you sure you want to delete this save?", "No", "Yes");
                dialog.btnB.onClick.AddListener(() => { DeleteSave(); });
            }
        }
    }

    void LoadSave()
    {
        foreach (Toggle toggle in toggles)
        {
            if (toggle.isOn && toggle.name != "")
            {
                Debug.Log(toggle.name);
                GameControl.control.Load(toggle.name);
                loadingScreen.StartLoadingScreen("05_Level_Selector");
            }
        }
    }

    void DeleteSave()
    {
        foreach (Toggle toggle in toggles)
        {
            if (toggle.isOn && toggle.name != "")
            {
                GameControl.control.DeleteSave(toggle.name);
                string newSaves = PlayerPrefs.GetString("SAVES").Replace(toggle.name, null);
                PlayerPrefs.SetString("SAVES", newSaves);
                loadingScreen.StartLoadingScreen("01_Main_Menu");
            }
        }
    }

    public void Cancel()
    {
        Audio.gameAudio.PlaySFX("btnClick2");
        levelManager.LoadLevel("01_Main_Menu");
    }
}
