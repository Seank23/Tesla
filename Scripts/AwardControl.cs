using UnityEngine;

public class AwardControl : MonoBehaviour
{
    public static void UnlockAward(string awardName)
    {
        foreach(Award award in PlayerDataControl.data.awards)
        {
            if(awardName == award.awardName && !PlayerDataControl.data.awardsUnlocked.Contains(award.name))
            {
                PlayerDataControl.data.awardsUnlocked.Add(award.name);
            }
        }
        GameControl.control.Save(PlayerDataControl.data.playerName, "awards");
    }
}
