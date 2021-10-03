using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AwardsUI : MonoBehaviour
{
    public Canvas awardCanvas;
    public Button awardTemplate;
    public GameObject AwardHolder;
    public Sprite lockedAward;
    public Image awardImage;
    public GameObject imageHolder;
    public Text description;
    public Text merit;

    private bool awardsShown;

	void Start ()
    {
        awardCanvas.enabled = false;
        InstantiateAwards();
	}
	
	void Update ()
    {
	    if(Input.GetKeyDown("a"))
        {
            awardsShown = !awardsShown;

            if (awardsShown)
                awardCanvas.enabled = true;
            else
            {
                awardCanvas.enabled = false;
                Destroy(imageHolder.GetComponentInChildren<Image>());
                description.text = "";
                merit.text = "";
            }
        }
	}

    void InstantiateAwards()
    {
        int awardIndex = 0;
        foreach (Award award in PlayerDataControl.data.awards)
        {
            if (PlayerDataControl.data.awardsUnlocked.Contains(award.name))
            {
                Button awardButton = Instantiate(awardTemplate);
                awardButton.name = award.name;
                RectTransform awardRect = awardButton.GetComponent<RectTransform>();
                awardRect.SetParent(AwardHolder.transform, false);
                awardButton.GetComponent<Image>().sprite = award.awardImage;
                awardButton.onClick.AddListener(() => { DisplayAward(award); });
            }
            else
            {
                Button awardButton = Instantiate(awardTemplate);
                awardButton.name = award.name;
                RectTransform awardRect = awardButton.GetComponent<RectTransform>();
                awardRect.SetParent(AwardHolder.transform, false);
                awardButton.GetComponent<Image>().sprite = lockedAward;
            }
            awardIndex++;
        }

        if (awardIndex < 9)
        {
            for(int i = 0; i < 9 - awardIndex; i++)
            {
                Button awardButton = Instantiate(awardTemplate);
                awardButton.name = "Empty Award";
                RectTransform awardRect = awardButton.GetComponent<RectTransform>();
                awardRect.SetParent(AwardHolder.transform, false);
                awardButton.GetComponent<Image>().sprite = lockedAward;
            }
        }
    }

    void DisplayAward(Award award)
    {
        foreach (Transform oldImage in imageHolder.transform)
            Destroy(oldImage.gameObject);
        description.text = "";
        merit.text = "";

        Image image = Instantiate(awardImage);
        image.name = award.name;
        RectTransform imageRect = image.GetComponent<RectTransform>();
        imageRect.SetParent(imageHolder.transform, false);
        image.GetComponent<Image>().sprite = award.awardImage;

        description.text = award.awardDescription;
        merit.text = "Merit Points: " + award.awardMeritPoints;
    }
}
