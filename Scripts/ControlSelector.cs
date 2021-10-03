using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ControlSelector : MonoBehaviour
{
    private InGameManager game;
    private PauseMenu pause;
    private Camera cam;
    private TimeControl timeControl;
    private UnityStandardAssets.ImageEffects.BlurOptimized blur;

    public GameObject myCanvas;
    public GameObject uiCanvas;
    public GameObject controlHolder;
    public bool selectorOpen;
    public GameObject[] controls = new GameObject[5];
    public GameObject controlNumber;
    private Dictionary<string, int> selectorControlDict = new Dictionary<string, int>();
    private List<GameObject> instantiatedButtons = new List<GameObject>();
    private List<GameObject> instantiatedNumbers = new List<GameObject>();

    void Start ()
    {
        game = FindObjectOfType<InGameManager>();
        pause = FindObjectOfType<PauseMenu>();
        cam = FindObjectOfType<Camera>();
        timeControl = FindObjectOfType<TimeControl>();
        blur = cam.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>();
        blur.enabled = false;
        myCanvas.SetActive(false);
        selectorOpen = false;

        selectorControlDict.Add("fast", 0);
        selectorControlDict.Add("pause", 0);
        selectorControlDict.Add("record", 0);
        selectorControlDict.Add("rewind", 0);
        selectorControlDict.Add("slow", 0);
    }
	
	void Update ()
    {
        if (Input.GetButtonDown("Control Selector") && !pause.isPaused && !timeControl.isTimeControlled)
        {
            selectorOpen = !selectorOpen;

            if (selectorOpen)
            {
                SelectorOpen();
            }

            if (!selectorOpen)
            {
                OnSelectorClosed();
            }
        }
	}

    void SelectorOpen()
    {
        game.inMenu = true;
        selectorOpen = true;
        myCanvas.SetActive(true);
        uiCanvas.SetActive(false);
        game.PauseGame(selectorOpen);
        blur.enabled = true;
        InstantiateControls();
    }

    public void OnSelectorClosed()
    {
        for (int i = 0; i < instantiatedButtons.Count; i++)
        {
            Destroy(instantiatedButtons[i]);
            Destroy(instantiatedNumbers[i]);

        }
        selectorOpen = false;
        myCanvas.SetActive(false);
        uiCanvas.SetActive(true);
        game.PauseGame(selectorOpen);
        blur.enabled = false;
        game.inMenu = false;
    }

    void InstantiateControls()
    {
        int numberOfIterations = 0;
        List<string> controlList = new List<string>();

        foreach (KeyValuePair<string, int> savedControls in PlayerDataControl.data.uiControlDict)
        {
            numberOfIterations += savedControls.Value;
            for (int i = 0; i < savedControls.Value; i++)
            {
                controlList.Add(savedControls.Key);
            }
        }

        selectorControlDict["fast"] = 0;
        selectorControlDict["pause"] = 0;
        selectorControlDict["record"] = 0;
        selectorControlDict["rewind"] = 0;
        selectorControlDict["slow"] = 0;

        for (int i = 0; i < numberOfIterations; i++)
        {
            foreach(GameObject control in controls)
            {
                if(control.name == controlList[i] + "Button")
                {
                    selectorControlDict[controlList[i]]++;

                    GameObject button = Instantiate(control, transform.position, Quaternion.identity) as GameObject;
                    button.name = control.name;
                    RectTransform buttonRect = button.GetComponent<RectTransform>();
                    buttonRect.SetParent(controlHolder.transform, false);

                    GameObject number = Instantiate(controlNumber, transform.position, Quaternion.identity) as GameObject;
                    number.name = controlList[i] + "Number";
                    RectTransform numberRect = number.GetComponent<RectTransform>();
                    numberRect.SetParent(button.transform, false);
                    numberRect.anchorMin = Vector2.zero;
                    numberRect.anchorMax = new Vector2(0.35f, 0.35f);

                    Text numberText = number.GetComponentInChildren<Text>();
                    numberText.text = " " + (PlayerDataControl.data.uiControlDict[controlList[i]]);

                    instantiatedButtons.Add(button);
                    instantiatedNumbers.Add(number);
                }
            }
        }
    }
}