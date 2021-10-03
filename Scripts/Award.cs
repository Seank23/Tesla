using UnityEngine;


[System.Serializable]
public class Award : ScriptableObject
{
    public string awardName;
    public string awardDescription;
    public int awardMeritPoints;
    public Sprite awardImage;
}
