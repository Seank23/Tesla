using UnityEngine;
using System.Collections.Generic;

public class PlayerDataControl : MonoBehaviour
{
    public static PlayerDataControl data;

    public string playerName;
    public int gold = 0;
    public int resets = 2;
    public int controlSlots = 3;
    public int resetSlots = 3;
    public int trashSlots = 3;
    public int sweeperType = 0;
    public bool[] completedLevels = new bool[16];
    public List<string> awardsUnlocked = new List<string>();
    public Dictionary<string, List<float>> levelTimes = new Dictionary<string, List<float>>();
    public Dictionary<string, int> controlDict = new Dictionary<string, int>();
    public Dictionary<string, int> uiControlDict = new Dictionary<string, int>();
    public Award[] awards;

    public string currentLevel;
    public int currentLevelGold;
    public List<string> collectedItems = new List<string>();
    public Dictionary<string, int> trashValues = new Dictionary<string, int>();

    void Awake()
    {
        if (data == null)
        {
            DontDestroyOnLoad(gameObject);
            data = this;
        }
        else if (data != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        controlDict.Add("fast", 0);
        controlDict.Add("pause", 0);
        controlDict.Add("record", 0);
        controlDict.Add("rewind", 0);
        controlDict.Add("slow", 0);

        uiControlDict.Add("fast", 0);
        uiControlDict.Add("pause", 0);
        uiControlDict.Add("record", 0);
        uiControlDict.Add("rewind", 0);
        uiControlDict.Add("slow", 0);

        trashValues.Add("Bench", 20);
        trashValues.Add("Bike", 40);
        trashValues.Add("Bin", 15);
        trashValues.Add("Dumpster", 60);
        trashValues.Add("Log", 10);
        trashValues.Add("Plant", 25);

        levelTimes.Add("06_Game_Rookie", new List<float>());
    }

    public void GetPlayerName(string player)
    {
        playerName = player;
    }

    public void ClearUIControlDict()
    {
        uiControlDict["fast"] = 0;
        uiControlDict["pause"] = 0;
        uiControlDict["record"] = 0;
        uiControlDict["rewind"] = 0;
        uiControlDict["slow"] = 0;
    }

	public void DebugControl()
	{
		string temp = "";
		foreach(var control in uiControlDict)
		{
			temp += control + "\n";
		}

		Debug.Log(temp);
	}
}
