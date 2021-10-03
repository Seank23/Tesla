using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIControl : BaseBehaviour
{
    public Image[] crystalSprites = new Image[6];
    public Image[] collectedControlSprites = new Image[5];
    public Image controlNumber;
    public Image crystalHolder;
    public Image controlHolder;
    public GameObject greenBar;
    public Image[] instantiatedSprites = new Image[3];
    public GameObject[] instantiatedBars = new GameObject[3];
    public Image resetSlot;
    public GameObject resetHolder;
    public Image resetControl;
    public GameObject trashSlotHolder;
    public Image trashSlot;
    public Image[] trashSprites;
    public Image currentItem;
    public Image fadeScreen;
    public Image[] monsterSprites;
    public GameObject monsterHolder;
    public Image tick;
    public Text timePast;
    public GameObject timeBar;
    public Text monstersLeft;
    private InGameManager game;
    private Sweeper sweeper;

    private float[] spriteAnchorMinX = { 0.1f, 0.42f, 0.74f };
    private float[] spriteAnchorMaxX = { 0.27f, 0.59f, 0.91f };
    private float[] barAnchorMinX = { 0.026f, 0.346f, 0.68f };
    private float[] barAnchorMaxX = { 0.34f, 0.66f, 0.974f };
    private float[] resetAnchorMinX = { 0.04f, 0.245f, 0.44f, 0.635f, 0.82f };
    private float[] resetAnchorMaxX = { 0.165f, 0.36f, 0.565f, 0.76f, 0.955f };
    private int resetIndex;
    public List<Image> collectedControls = new List<Image>();
    private List<Image> collectedNumbers = new List<Image>();
    private List<Image> trashSlots = new List<Image>();
    private List<Image> trashCollected = new List<Image>();
    private List<Image> resets = new List<Image>();
    private List<Image> monstersInLevel = new List<Image>();
    private Image controlToBeDestroyed;

    void Start ()
    {
        game = FindObjectOfType<InGameManager>();
        sweeper = FindObjectOfType<Sweeper>();

        trashSlots.Add(currentItem);

        CreateResetSlots();
        InstantiateResets("init");
        CreateTrashSlots();
        InstantiateMonsterSprites();
        monstersLeft.GetComponent<Text>().enabled = true;
    }

    void Update()
    {
        string minutes = Mathf.FloorToInt(time.time / 60).ToString();
        string seconds = Math.Round(time.time % 60, 3).ToString();
        if((time.time % 60) >= 0 && (time.time % 60) <= 10)
        {
            seconds = "0" + seconds;
        }
        timePast.text = "Time: " + minutes + ":" + seconds;

        if (time.time >= game.timeLimit - 10)
        {
            if (Mathf.FloorToInt(time.time) % 2 == 0)
            {
                timePast.color = Color.red;
            }
            else if (Mathf.FloorToInt(time.time) % 2 != 0)
            {
                timePast.color = Color.white;
            }
        }

        timeBar.GetComponent<RectTransform>().anchorMax = new Vector2(1 - (time.time / game.timeLimit), 1);
    }

    void CreateResetSlots()
    {
        float lastMaxX = 0f;
        float lastMinX = -0.2f;
        for (int i = 0; i < PlayerDataControl.data.resetSlots; i++)
        {
            lastMaxX += 0.2f;
            lastMinX += 0.2f;
            Image slot = Instantiate(resetSlot, transform.position, Quaternion.identity) as Image;
            RectTransform slotRect = slot.GetComponent<RectTransform>();
            slotRect.SetParent(resetHolder.transform, false);
            slotRect.anchorMax = new Vector2(lastMaxX, slotRect.anchorMax.y);
            slotRect.anchorMin = new Vector2(lastMinX, slotRect.anchorMin.y);
        }
    }

    public void InstantiateResets(string state)
    {
        if (state == "init")
        {
            for (int i = 0; i < PlayerDataControl.data.resets; i++)
            {

                Image reset = Instantiate(resetControl, transform.position, Quaternion.identity) as Image;
                RectTransform resetRect = reset.GetComponent<RectTransform>();
                resetRect.SetParent(resetHolder.transform, false);
                resetRect.anchorMax = new Vector2(resetAnchorMaxX[resetIndex], resetRect.anchorMax.y);
                resetRect.anchorMin = new Vector2(resetAnchorMinX[resetIndex], resetRect.anchorMin.y);
                resets.Add(reset);
                resetIndex++;
            }
        }
        else if(state == "refresh")
        {
            for (int i = 0; i < game.playerLives; i++)
            {

                Image reset = Instantiate(resetControl, transform.position, Quaternion.identity) as Image;
                RectTransform resetRect = reset.GetComponent<RectTransform>();
                resetRect.SetParent(resetHolder.transform, false);
                resetRect.anchorMax = new Vector2(resetAnchorMaxX[resetIndex], resetRect.anchorMax.y);
                resetRect.anchorMin = new Vector2(resetAnchorMinX[resetIndex], resetRect.anchorMin.y);
                resets.Add(reset);
                resetIndex++;
            }
        }
    }

    void CreateTrashSlots()
    {
        float lastMinY = -0.2f;
        float lastMaxY = 0f;
        for(int i = 0; i < (PlayerDataControl.data.trashSlots - 1); i++)
        {
            lastMinY += 0.2f;
            lastMaxY += 0.2f;
            Image slot = Instantiate(trashSlot, transform.position, Quaternion.identity) as Image;
            slot.name = "Slot_" + i;
            RectTransform slotRect = slot.GetComponent<RectTransform>();
            slotRect.SetParent(trashSlotHolder.transform, false);
            slotRect.anchorMax = new Vector2(1f, lastMaxY);
            slotRect.anchorMin = new Vector2(0f, lastMinY);
            trashSlots.Add(slot);
        }
    }

    void InstantiateMonsterSprites()
    {
        float lastMinX = -0.2f;
        float lastMaxX = 0f;
        float lastMinY = 0.5f;
        float lastMaxY = 1f;
        for(int i = 0; i < game.monsters.Count; i++)
        {
            foreach(Image sprite in monsterSprites)
            {
                if(sprite.name == game.monsters[i])
                {
                    if (lastMaxX == 1f)
                    {
                        lastMinX = -0.2f;
                        lastMaxX = 0f;
                        lastMinY = 0f;
                        lastMaxY = 0.5f;
                    }
                    lastMinX += 0.2f;
                    lastMaxX += 0.2f;
                    Image monster = Instantiate(sprite, transform.position, Quaternion.identity) as Image;
                    monster.name = game.monsters[i];
                    RectTransform spriteRect = monster.GetComponent<RectTransform>();
                    spriteRect.SetParent(monsterHolder.transform, false);
                    spriteRect.anchorMax = new Vector2(lastMaxX, lastMaxY);
                    spriteRect.anchorMin = new Vector2(lastMinX, lastMinY);
                    monstersInLevel.Add(monster);
                }
            }
        }
    }

    public void DisplayCrystalUI(string crystalSprite)
    {
        foreach (Image sprite in crystalSprites)
        {
            if (sprite.name == crystalSprite + "_sprite")
            {
                game.crystalPickedUp[game.crystalIndex] = crystalSprite;
                Image crystal = Instantiate(sprite, transform.position, Quaternion.identity) as Image;
                crystal.name = crystalSprite + "_sprite";
                RectTransform rectTransform = crystal.GetComponent<RectTransform>();
                rectTransform.SetParent(crystalHolder.transform, false);
                rectTransform.anchorMin = new Vector2(spriteAnchorMinX[game.crystalIndex], rectTransform.anchorMin.y);
                rectTransform.anchorMax = new Vector2(spriteAnchorMaxX[game.crystalIndex], rectTransform.anchorMax.y);
                instantiatedSprites[game.crystalIndex] = crystal;

                GameObject bar = Instantiate(greenBar, transform.position, Quaternion.identity) as GameObject;
                RectTransform barRect = bar.GetComponent<RectTransform>();
                barRect.SetParent(crystalHolder.transform, false);
                barRect.anchorMin = new Vector2(barAnchorMinX[game.crystalIndex], barRect.anchorMin.y);
                barRect.anchorMax = new Vector2(barAnchorMaxX[game.crystalIndex], barRect.anchorMax.y);
                instantiatedBars[game.crystalIndex] = bar;
                break;
            }
        }
    }

    public void DestroyCrystalsFast()
    {
        for (int i = 0; i < 3; i++)
        {
            Destroy(instantiatedSprites[i]);
            Destroy(instantiatedBars[i]);
        }
        game.crystalIndex = 0;
    }

    public IEnumerator DestroyCrystalSprites()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 3; i++)
        {
            Destroy(instantiatedSprites[i]);
            Destroy(instantiatedBars[i]);
        }
        game.crystalIndex = 0;
    }

    public void DisplayControlUI(string controlSprite)
    {
        foreach (Image sprite in collectedControlSprites)
        {
            if(sprite.name == controlSprite + "Control_Sprite")
            {
                if (PlayerDataControl.data.uiControlDict[controlSprite] == 0)
                {
                    Image control = Instantiate(sprite, transform.position, Quaternion.identity) as Image;
                    control.name = controlSprite;
                    RectTransform controlRect = control.GetComponent<RectTransform>();
                    controlRect.SetParent(controlHolder.transform, false);

                    collectedControls.Add(control);

                    Image number = Instantiate(controlNumber, transform.position, Quaternion.identity) as Image;
                    number.name = controlSprite;
                    RectTransform numberRect = number.GetComponent<RectTransform>();
                    numberRect.SetParent(control.transform, false);
                    numberRect.anchorMin = Vector2.zero;
                    numberRect.anchorMax = new Vector2(0.35f, 0.35f);

                    collectedNumbers.Add(number);

                    Text numberText = number.GetComponentInChildren<Text>();
                    numberText.text = " " + (PlayerDataControl.data.uiControlDict[controlSprite] + 1);
                }
                else
                {
                    Text numberText1 = collectedControls.Find(obj => obj.name == controlSprite).GetComponentInChildren<Text>();
                    numberText1.text = " " + (PlayerDataControl.data.uiControlDict[controlSprite] + 1);
                }

                break;
            }
        }
    }

    public void DestroyControlSprite()
    {
        controlToBeDestroyed = collectedControls[0];
        Image numberToBeDestroyed = collectedNumbers[0];

        if (PlayerDataControl.data.uiControlDict[controlToBeDestroyed.name] == 0)
        {
            Destroy(controlToBeDestroyed);
            Destroy(numberToBeDestroyed);
            Destroy(numberToBeDestroyed.GetComponentInChildren<Text>());
            collectedControls.RemoveAt(0);
            collectedNumbers.RemoveAt(0);
        }
        else
        {
            foreach (Image myControl in collectedControls)
            {
                if (myControl.name == controlToBeDestroyed.name)
                {
                    Image updatedNumber = collectedNumbers.Find(obj => obj.name == myControl.name);
                    Text updatedText = updatedNumber.GetComponentInChildren<Text>();
                    updatedText.text = " " + PlayerDataControl.data.uiControlDict[myControl.name];
                }
            }
        }
    }

    public void ClearControlSprites()
    {
        for (int i = 0; i < collectedControls.Count; i++)
        {
            Destroy(collectedControls[i]);
            Destroy(collectedNumbers[i]);
            Destroy(collectedNumbers[i].GetComponentInChildren<Text>());
        }
        collectedControls.Clear();
        collectedNumbers.Clear();
    }

    public void DisplayTrashUI(string itemName)
    {
        foreach(Image sprite in trashSprites)
        {
            if(sprite.name == itemName + "_sprite")
            {
                Image item = Instantiate(sprite, transform.position, Quaternion.identity) as Image;
                item.name = itemName;
                RectTransform spriteRect = item.GetComponent<RectTransform>();
                spriteRect.SetParent(trashSlots[0].transform, false);
                trashCollected.Add(item);

                for (int i = 0; i < sweeper.itemIndex; i++)
                {
                    RectTransform rt = trashCollected[(sweeper.itemIndex - i - 1)].GetComponent<RectTransform>();
                    rt.SetParent(trashSlots[i].transform, false);
                }
            }
        }
    }

    public void ClearTrashUI()
    {
        foreach(Image item in trashCollected)
        {
            Destroy(item);
        }

        trashCollected.Clear();
    }

    public void DestroyTrashSprite(bool state)
    {
        if (state)
        {
            Destroy(trashCollected[0]);
            trashCollected.RemoveAt(0);
            Invoke("UpdateTrashSpacesForwards", 0.01f);
        }
        else
        {
            Destroy(trashCollected[sweeper.itemIndex]);
            trashCollected.RemoveAt(sweeper.itemIndex);
            Invoke("UpdateTrashSpacesBackwards", 0.01f);
        }
    }

    public void UpdateTrashSpacesForwards()
    {
        for (int i = 0; i < sweeper.itemIndex; i++)
        {
            if (sweeper.itemIndex - i - 1 != 0)
            {
                RectTransform rt = trashCollected[(sweeper.itemIndex - i - 1)].GetComponent<RectTransform>();
                rt.SetParent(trashSlots[i].transform, false);
            }
        }
    }

    public void UpdateTrashSpacesBackwards()
    {
        for (int i = 0; i < sweeper.itemIndex; i++)
        {
            RectTransform rt = trashCollected[(sweeper.itemIndex - i - 1)].GetComponent<RectTransform>();
            rt.SetParent(trashSlots[i].transform, false);
        }
    }

    public void DestroyResets()
    {
        foreach(Image reset in resets)
        {
            Destroy(reset);
        }
        resets.Clear();
        resetIndex = 0;
    }

    public void EditMonsterSprite(string monster)
    {
        foreach (string name in game.monsters)
        {
            if (name == monster)
            {
                Image monsterKilled = null;

                foreach (Image sprite in monstersInLevel)
                {
                    if (monster == sprite.name)
                    {
                        monsterKilled = sprite;
                        monstersInLevel.Remove(sprite);
                        break;
                    }
                }

                Image myTick = Instantiate(tick, transform.position, Quaternion.identity) as Image;
                myTick.name = "tick";
                RectTransform tickRect = myTick.GetComponent<RectTransform>();
                tickRect.SetParent(monsterKilled.transform, false);
                break;
            }
        }
    }

    public IEnumerator FadeOut(float time)
    {
        Color fade = fadeScreen.color;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
        {
            fade.a = Mathf.Lerp(fade.a, 1, t);
            fadeScreen.color = fade;
            yield return null;
        }
    }

    public IEnumerator DisplayMonstersLeft()
    {
        monstersLeft.GetComponent<Text>().enabled = true;
        monstersLeft.text = game.monsters.Count + " Monsters Left!";

        if (game.monsters.Count == 0)
            monstersLeft.text = "Head to the portal!";

        yield return new WaitForSeconds(2);

        monstersLeft.GetComponent<Text>().enabled = false;
    }
}
