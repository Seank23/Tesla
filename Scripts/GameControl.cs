using UnityEngine;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameControl : MonoBehaviour
{
    public static GameControl control;
	
	void Awake ()
    {
	    if(control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if(control != this)
        {
            Destroy(gameObject);
        }
	}

    public void Save(string saveName, string condition)
    {
        if (condition == "data")
        {
            BinaryFormatter binary = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/" + saveName + ".dat");

            PlayerData data = new PlayerData();
            // Gets current game data from PlayerDataControl
            data.playerName = PlayerDataControl.data.playerName;
            data.gold = PlayerDataControl.data.gold;
            data.controlSlots = PlayerDataControl.data.controlSlots;
            data.resetSlots = PlayerDataControl.data.resetSlots;
            data.trashSlots = PlayerDataControl.data.trashSlots;
            data.resets = PlayerDataControl.data.resets;
            data.rewinds = PlayerDataControl.data.controlDict["rewind"];
            data.slows = PlayerDataControl.data.controlDict["slow"];
            data.fasts = PlayerDataControl.data.controlDict["fast"];
            data.records = PlayerDataControl.data.controlDict["record"];
            data.pauses = PlayerDataControl.data.controlDict["pause"];
            data.completedLevels = PlayerDataControl.data.completedLevels;
            data.levelTimes = PlayerDataControl.data.levelTimes;

            binary.Serialize(file, data);
            file.Close();
        }
        else if(condition == "awards")
        {
            BinaryFormatter binary = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/" + saveName + "_Awards.dat");
            PlayerData data = new PlayerData();
            data.unlockedAwards = PlayerDataControl.data.awardsUnlocked;
            binary.Serialize(file, data);
            file.Close();
        }
    }

    public void Load(string fileName)
    {
        if (File.Exists(Application.persistentDataPath + "/" + fileName + ".dat"))
        {
            BinaryFormatter binary = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + fileName + ".dat", FileMode.Open);
            PlayerData data = (PlayerData)binary.Deserialize(file);
            file.Close();
            // Gets saved data from file, sets to current game data
            PlayerDataControl.data.playerName = data.playerName;
            PlayerDataControl.data.gold = data.gold;
            PlayerDataControl.data.controlSlots = data.controlSlots;
            PlayerDataControl.data.resetSlots = data.resetSlots;
            PlayerDataControl.data.trashSlots = data.trashSlots;
            PlayerDataControl.data.resets = data.resets;
            PlayerDataControl.data.controlDict["rewind"] = data.rewinds;
            PlayerDataControl.data.controlDict["slow"] = data.slows;
            PlayerDataControl.data.controlDict["fast"] = data.fasts;
            PlayerDataControl.data.controlDict["record"] = data.records;
            PlayerDataControl.data.controlDict["pause"] = data.pauses;
            PlayerDataControl.data.completedLevels = data.completedLevels;
            PlayerDataControl.data.levelTimes = data.levelTimes;
        }
        if (File.Exists(Application.persistentDataPath + "/" + fileName + "_Awards.dat"))
        {
            BinaryFormatter binary = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + fileName + "_Awards.dat", FileMode.Open);
            PlayerData data = (PlayerData)binary.Deserialize(file);
            file.Close();
            PlayerDataControl.data.awardsUnlocked = data.unlockedAwards;
        }
    }

    public void DeleteSave(string fileName)
    {
        if (File.Exists(Application.persistentDataPath + "/" + fileName + ".dat"))
        {
            File.Delete(Application.persistentDataPath + "/" + fileName + ".dat");
        }
        if (File.Exists(Application.persistentDataPath + "/" + fileName + "_Awards.dat"))
        {
            File.Delete(Application.persistentDataPath + "/" + fileName + "_Awards.dat");
        }
    }
}

[Serializable]
class PlayerData
{
    public string playerName;
    public int gold;
    public int controlSlots;
    public int resetSlots;
    public int trashSlots;
    public int resets;
    public int rewinds;
    public int slows;
    public int fasts;
    public int records;
    public int pauses;
    public bool[] completedLevels;
    public List<string> unlockedAwards;
    public Dictionary<string, List<float>> levelTimes;
}

