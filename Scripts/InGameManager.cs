using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Chronos;
using System.Collections;
using System.Linq;

public class InGameManager : BaseBehaviour
{
    public LoadingScreen loadingScreen;
    public GameObject[] crystals = new GameObject[7];
    private LevelManager level;
    private UIControl ui;
    private TimeControl timeControl;
    private FinishPortal finish;
    private Sweeper sweeper;

    public List<string> monsters;
    public bool inMenu = false;
    public int timeLimit;
    public int playerLives;
    public int goldWorth = 20;
    public int crystalIndex = 0;
    public int controlIndex = 0;
    public bool gameOver = false;
    public bool rewinding = false;
    public string[] crystalPickedUp = new string[3];
    public int crystalsReleased;
    private bool initLastControls = false;
    private Dictionary<string, int> crystalDict = new Dictionary<string, int>();

    void Start ()
    {
        ui = FindObjectOfType<UIControl>();
        level = FindObjectOfType<LevelManager>();
        timeControl = FindObjectOfType<TimeControl>();
        finish = FindObjectOfType<FinishPortal>();
        sweeper = FindObjectOfType<Sweeper>();
        playerLives = PlayerDataControl.data.resets;

        foreach (GameObject item in crystals)
        {
            if (item.name != "goldCrystal")
            {
                string name = item.name.Substring(0, item.name.Length - 7);
                crystalDict.Add(name, 0);
            }
        }

        InitialControlInstances("start");
        Music.gameMusic.PlayMusic("levelMusic2");
        PlayerDataControl.data.currentLevel = SceneManager.GetActiveScene().name;
    }
	
	void Update ()
    {
        if (time.time >= timeLimit)
        {
            AwardControl.UnlockAward("Outatime");
            GameOver();
        }
	}

    public void InitialControlInstances(string calledFrom)
    {
        int numberOfIterations = 0;
        List<string> controlList = new List<string>();

        print("Controls Updated");
        initLastControls = true;
        if (calledFrom == "start")
        {
            foreach (KeyValuePair<string, int> savedControls in PlayerDataControl.data.controlDict)
            {
                numberOfIterations += savedControls.Value;
                for (int i = 0; i < savedControls.Value; i++)
                {
                    controlList.Add(savedControls.Key);
                }
            }
        }
        else if(calledFrom == "controlSelector")
        {
            foreach (KeyValuePair<string, int> savedControls in PlayerDataControl.data.uiControlDict)
            {
                numberOfIterations += savedControls.Value;
                for (int i = 0; i < savedControls.Value; i++)
                {
                    controlList.Add(savedControls.Key);
                }
            }
        }

        PlayerDataControl.data.ClearUIControlDict();

        for (int i = 0; i < numberOfIterations; i++)
        {
            ControlCollected(controlList[i], true);
        }

        PlayerDataControl.data.uiControlDict = PlayerDataControl.data.controlDict;
        initLastControls = false;
    }

    public void CrystalInstances()
    {
        int numberOfIterations = 0;
        List<string> crystalList = new List<string>();

        foreach(KeyValuePair<string, int> crystal in crystalDict)
        {
            numberOfIterations += crystal.Value;
            for(int i = 0; i < crystal.Value; i++)
            {
                crystalList.Add(crystal.Key);
            }
        }

        foreach (GameObject item in crystals)
        {
            if (item.name != "goldCrystal")
            {
                string name = item.name.Substring(0, item.name.Length - 7);
                crystalDict[name] = 0;
            }
        }

        crystalIndex = 0;

        for (int i = 0; i < numberOfIterations; i++)
        {
            RespawnCrystal(crystalList[i]);
        }
    }

    void RespawnCrystal(string crystalName)
    {
        crystalDict[crystalName]++;
        crystalName += "Crystal";
        ui.DisplayCrystalUI(crystalName);
        crystalIndex++;
        TrackCrystalCount();
    }

    public void CrystalCollected(string crystalName, float[] crystalPosition)
    {
        if (crystalIndex != 3)
        {
            if (crystalName == "goldCrystal")
            {
                time.Do
                (
                    false,
                    delegate()
                    {
                        Audio.gameAudio.PlaySFX("crystal");
                        PlayerDataControl.data.currentLevelGold += goldWorth;
                        return goldWorth;
                    },
                    delegate(int worth)
                    {
                        PlayerDataControl.data.currentLevelGold -= worth;
                    }
                );
            }
            else
            {
                ui.DisplayCrystalUI(crystalName);
                time.Do
                (
                    false,
                    delegate ()
                    {
                        string name = crystalName.Substring(0, crystalName.Length - 7);
                        crystalDict[name]++;
                        crystalIndex++;
                        return name;
                    },
                    delegate (string name)
                    {
                        crystalDict[name]--;
                    }
                );
            }

            TrackCrystalCount();
        }
    }

    IEnumerator InstantiateCrystals(Vector3 pos)
    {
        yield return null;
        Clock clock = Timekeeper.instance.Clock("Time Crystals");

        int[] rndCrystals = new int[crystalsReleased];
        for(int i = 0; i < crystalsReleased; i++)
        {
            rndCrystals[i] = Random.Range(0, crystals.Length);
        }

        int index = 0;
        foreach(GameObject crystal in crystals)
        {
            if(rndCrystals.Contains(index))
            {
                time.Do
                (
                    false,
                    delegate ()
                    {
                        int rndx = Random.Range(-300, 300);
                        int rndz = Random.Range(-300, 300);
                        GameObject clone = Instantiate(crystal, pos, Quaternion.identity) as GameObject;
                        clone.name = crystal.name;
                        Rigidbody rb = clone.GetComponent<Rigidbody>();
                        rb.AddForce(new Vector3(rndx * clock.localTimeScale, 275, rndz * clock.localTimeScale));
                        return clone;
                    },
                    delegate (GameObject clone)
                    {
                        Destroy(clone);
                    }
                );
            }
            index++;
        }
    }

    public void TrackCrystalCount()
    {
        if (crystalIndex == 3)
        {
            if (crystalPickedUp[0] == crystalPickedUp[1] && crystalPickedUp[1] == crystalPickedUp[2])
            {
                if(crystalPickedUp[0] == "resetCrystal")
                {
                    if(playerLives < PlayerDataControl.data.resetSlots)
                    {
                        time.Do
                        (
                            false,
                            delegate ()
                            {
                                playerLives++;
                                ui.DestroyResets();
                                ui.InstantiateResets("refresh");
                                return playerLives;
                            },
                            delegate (int lives)
                            {
                                lives--;
                                playerLives = lives;
                            }
                        );
                    }
                    Audio.gameAudio.PlaySFX("crystal");
                }
                else
                {
                    ControlCollected(crystalPickedUp[0], false);
                }
            }
            else
            {
                Audio.gameAudio.PlaySFX("wrongCombo");
            }

            time.Do
            (
                false,
                delegate ()
                {
                    List<int> previousValues = new List<int>();
                    foreach (KeyValuePair<string, int> crystal in crystalDict)
                    {
                        previousValues.Add(crystal.Value);
                    }

                    crystalDict["pause"] = 0;
                    crystalDict["fast"] = 0;
                    crystalDict["record"] = 0;
                    crystalDict["rewind"] = 0;
                    crystalDict["slow"] = 0;
                    crystalDict["reset"] = 0;

                    return previousValues;
                },
                delegate (List<int> values)
                {
                    crystalDict["pause"] = values[0];
                    crystalDict["fast"] = values[1];
                    crystalDict["record"] = values[2];
                    crystalDict["rewind"] = values[3];
                    crystalDict["slow"] = values[4];
                    crystalDict["reset"] = values[5];
                }
            );
            StartCoroutine(ui.DestroyCrystalSprites());
        }
        else
        {
            Audio.gameAudio.PlaySFX("crystal");
        }
    }

    public void ControlCollected(string controlName, bool rebuild)
    {
        if (!rebuild)
        {
            if (initLastControls == false)
            {
                controlName = controlName.Substring(0, controlName.Length - 7);
                Audio.gameAudio.PlaySFX("control");
            }

            if (controlIndex == PlayerDataControl.data.controlSlots)
            {
                time.Do
                (
                    false,
                    delegate ()
                    {
                        string controlReplaced = ui.collectedControls[0].name;
                        PlayerDataControl.data.uiControlDict[controlReplaced]--;
                        ui.DestroyControlSprite();

                        return controlReplaced;
                    },
                    delegate (string control)
                    {
                        print(control);
                        PlayerDataControl.data.uiControlDict[control]++;
                    }
                );
            }

            time.Do
            (
                false,
                delegate ()
                {
                    ui.DisplayControlUI(controlName);
                    PlayerDataControl.data.uiControlDict[controlName]++;

                    if (controlIndex != PlayerDataControl.data.controlSlots)
                        controlIndex++;

                    return controlName;
                },
                delegate (string control)
                {
                    PlayerDataControl.data.uiControlDict[control]--;

                    if (controlIndex != PlayerDataControl.data.controlSlots)
                        controlIndex--;

                    PlayerDataControl.data.DebugControl();
                }
            );
        }
        else
        {
            if (initLastControls == false)
            {
                controlName = controlName.Substring(0, controlName.Length - 7);
                Audio.gameAudio.PlaySFX("control");
            }

            if (controlIndex == PlayerDataControl.data.controlSlots)
            {
                PlayerDataControl.data.uiControlDict[ui.collectedControls[0].name]--;
                ui.DestroyControlSprite();
            }

            ui.DisplayControlUI(controlName);
            PlayerDataControl.data.uiControlDict[controlName]++;

            if (controlIndex != PlayerDataControl.data.controlSlots)
                controlIndex++;
        }
    }

    public void PauseGame(bool state)
    {
        Clock clock = Timekeeper.instance.Clock("Root");

        if(state == true)
        {
            inMenu = true;
            clock.localTimeScale = 0f;
        }
        else
        {
            inMenu = false;
            clock.localTimeScale = 1f;
        }
    }

    public void PlayerLostLife()
    {
        playerLives--;

        if(playerLives < 0)
        {
            gameOver = true;
            GameOver();
        }

        if (!gameOver)
        {
            timeControl.Rewind("died");
        }
    }

    public void RespawnObjectOccurrence(GameObject passedObject, GameObject[] objects, Vector3 instancePos)
    {
        time.Do
        (
            false,
            delegate()
            {
                string crystalName = passedObject.name;
                Destroy(passedObject);
                return crystalName;
            },
            delegate(string objectName)
            {
                foreach(GameObject item in objects)
                {
                    if(item.name == objectName)
                    {
                        Instantiate(item, instancePos, Quaternion.identity);
                    }
                }
            }
        );
    }

    private void GameOver()
    {
        Music.gameMusic.PlayMusic("");
        level.LoadLevel("11_Game_Over");
    }

    public void DisableGameObject(GameObject passedObject)
    {
        time.Do
        (
            false,
            delegate ()
            {
                passedObject.SetActive(false);
                return passedObject;
            },
            delegate (GameObject gameObject)
            {
                passedObject.SetActive(true);
            }
        );
    }

    public void MonsterKilled(GameObject monster)
    {
        print("Monster killed");
        ui.EditMonsterSprite(monster.name);
        monsters.Remove(monster.name);
        StartCoroutine(ui.DisplayMonstersLeft());
        StartCoroutine(InstantiateCrystals(monster.transform.position));
        Destroy(monster);

        if (monsters.Count == 0)
        {
            finish.Unlock();
        }
    }

    public IEnumerator LevelComplete()
    {
        StartCoroutine(ui.FadeOut(5));
        Music.gameMusic.PlayMusic("");
        PlayerDataControl.data.collectedItems = sweeper.itemsCollectedNames;
        PlayerDataControl.data.levelTimes[PlayerDataControl.data.currentLevel].Add(time.time);
        PlayerDataControl.data.resets = playerLives;
        yield return new WaitForSeconds(1);
        level.LoadLevel("12_Level_Complete");
    }
}
