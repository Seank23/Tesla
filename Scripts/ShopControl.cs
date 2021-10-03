using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopControl : MonoBehaviour
{
    public ShopItem[] items;
    public GameObject imageHolder;
    public GameObject textHolder;
    public GameObject toggleHolder;
    public Image shopImage;
    public Text shopText;
    public Text txtGold;
    public Text txtCost;
    public Text txtDescription;
    public Toggle shopToggle;
    public Button purchase;
    public Button cancel;
    private List<Toggle> toggleList = new List<Toggle>();
    private LoadingScreen loadingScreen;
    private DialogueBox dialog;

    private int currentGold;
    private int totalCost;

	void Start ()
    {
        loadingScreen = FindObjectOfType<LoadingScreen>();
        dialog = FindObjectOfType<DialogueBox>();
        InstantiateItems();
	}
	
	void Update ()
    {
        txtCost.text = "Total Cost: " + totalCost.ToString() + "G";

        if(dialog.dialogOpen)
        {
            purchase.enabled = false;
            cancel.enabled = false;
            foreach(Toggle toggle in toggleList)
            {
                toggle.enabled = false;
            }
        }
        else
        {
            purchase.enabled = true;
            cancel.enabled = true;
            foreach (Toggle toggle in toggleList)
            {
                toggle.enabled = true;
            }
        }
	}

    void InstantiateItems()
    {
        foreach(ShopItem item in items)
        {
            Image itemImage = Instantiate(shopImage);
            RectTransform imageRect = itemImage.GetComponent<RectTransform>();
            imageRect.SetParent(imageHolder.transform, false);
            itemImage.sprite = item.itemThumbnail;

            Text itemText = Instantiate(shopText);
            RectTransform textRect = itemText.GetComponent<RectTransform>();
            textRect.SetParent(textHolder.transform, false);
            itemText.text = "\n" + item.itemName + "\n" + item.itemCost + "G";

            Toggle itemToggle = Instantiate(shopToggle);
            RectTransform toggleRect = itemToggle.GetComponent<RectTransform>();
            toggleRect.SetParent(toggleHolder.transform, false);
            itemToggle.isOn = false;
            itemToggle.onValueChanged.AddListener((value) => { OnToggleChange(value); });
            toggleList.Add(itemToggle);
        }

        currentGold = PlayerDataControl.data.gold;
        txtGold.text = "Gold: " + currentGold.ToString() + "G";
        txtCost.text = "Total Cost: 0G";
    }

    public void PurchaseClicked()
    {
        for (int i = 0; i < toggleList.Count; i++)
        {
            if (toggleList[i].isOn)
            {
                if (currentGold >= totalCost)
                {
                    Audio.gameAudio.PlaySFX("btnClick1");
                    dialog.ShowDialogue("Are you sure you want to buy these items?\n" + currentGold + "G -> " + (currentGold - totalCost) + "G", "No", "Yes");
                    dialog.btnB.onClick.AddListener(() => { PurchaseItems(); });
                }
                else
                {
                    Audio.gameAudio.PlaySFX("unable");
                    dialog.ShowDialogue("You do not have enough gold to purchase these items", "Cancel", "OK");
                    dialog.btnB.onClick.AddListener(() => { dialog.CloseDialog(); dialog.DialogAudio(); });
                }
            }
        }
    }

    void PurchaseItems()
    {
        Audio.gameAudio.PlaySFX("itemsBought");
        for(int i = 0; i < toggleList.Count; i++)
        {
            if (toggleList[i].isOn)
            {
                PlayerDataControl.data.gold -= items[i].itemCost;
                Buy(items[i].itemName, i);
                print(items[i].itemName + " brought");
                toggleList[i].isOn = false;
            }
        }
        currentGold = PlayerDataControl.data.gold;
        txtGold.text = "Gold: " + currentGold.ToString() + "G";
        Leave();
    }

    void OnToggleChange(bool value)
    {
        int tempCost = 0;
        for(int i = 0; i < toggleList.Count; i++)
        {
            if(toggleList[i].isOn)
            {
                tempCost += items[i].itemCost;
                txtDescription.text = items[i].ItemDescription;
            }
        }
        totalCost = tempCost;
        tempCost = 0;
    }

    void Buy(string itemName, int itemNo)
    {
        if(itemName == "Reset Crystal")
        {
            if(PlayerDataControl.data.resets < PlayerDataControl.data.resetSlots)
            {
                PlayerDataControl.data.resets++;
            }
            else
            {
                PlayerDataControl.data.gold += items[itemNo].itemCost;
            }
        }

        if(itemName == "Time Control Slot")
        {
            if(PlayerDataControl.data.controlSlots < 10)
            {
                PlayerDataControl.data.controlSlots++;
            }
            else
            {
                PlayerDataControl.data.gold += items[itemNo].itemCost;
            }
        }

        if(itemName == "Reset Slot")
        {
            if (PlayerDataControl.data.resetSlots < 5)
            {
                PlayerDataControl.data.resetSlots++;
            }
            else
            {
                PlayerDataControl.data.gold += items[itemNo].itemCost;
            }
        }

        if (itemName == "Trash Slot")
        {
            if (PlayerDataControl.data.trashSlots < 6)
            {
                PlayerDataControl.data.trashSlots++;
            }
            else
            {
                PlayerDataControl.data.gold += items[itemNo].itemCost;
            }
        }

        if (itemName == "Sweeper MkII")
        {
            if (PlayerDataControl.data.sweeperType == 0)
            {
                PlayerDataControl.data.sweeperType = 1;
            }
            else
            {
                PlayerDataControl.data.gold += items[itemNo].itemCost;
            }
        }

        if (itemName == "Sweeper MkIII")
        {
            if (PlayerDataControl.data.sweeperType == 1)
            {
                PlayerDataControl.data.sweeperType = 2;
            }
            else
            {
                PlayerDataControl.data.gold += items[itemNo].itemCost;
            }
        }
        currentGold = PlayerDataControl.data.gold;
        txtGold.text = "Gold: " + currentGold.ToString() + "G";
    }

    public void Leave()
    {
        if (PlayerDataControl.data.playerName.Length > 0)
        {
            GameControl.control.Save(PlayerDataControl.data.playerName, "data");
        }
        loadingScreen.StartLoadingScreen("05_Level_Selector");
    }

    public void Cancel()
    {
        Audio.gameAudio.PlaySFX("btnClick2");
        Leave();
    }
}
