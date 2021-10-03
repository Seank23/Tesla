using UnityEngine;
using UnityEngine.UI;
using System;

public class ControlActive : MonoBehaviour
{
    public GameObject canvas;
    public GameObject tint;
    public Text txtTimeLeft;
    public GameObject iconHolder;
    public Image[] controls = new Image[5];

    private bool isActive;
    float timeLeft;

    void Start()
    {
        canvas.SetActive(false);
    }

    void Update()
    {
        if(isActive)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                txtTimeLeft.text = "0" + Math.Round(timeLeft, 3).ToString();
            }
            else
            {
                isActive = false;
                canvas.SetActive(false);
            }
        }
    }

    public void Display(Color color, float controlTime, string control)
    {
        timeLeft = controlTime;
        isActive = true;
        canvas.SetActive(true);
        tint.GetComponent<Image>().color = color;
        Destroy(iconHolder.GetComponentInChildren<Image>());
        foreach(Image sprite in controls)
        {
            if(sprite.name == control + "Control_Sprite")
            {
                Image icon = Instantiate(sprite);
                RectTransform rect = icon.GetComponent<RectTransform>();
                rect.SetParent(iconHolder.transform, false);
                rect.anchorMax = new Vector2(1, 1);
                rect.anchorMin = new Vector2(0, 0);
            }
        }
    }
}
