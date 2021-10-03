using UnityEngine;
using System.Collections;

public class TimeKeeperControl : MonoBehaviour
{
    public static TimeKeeperControl timeKeeper;

	void Awake ()
    {
	    if(timeKeeper == null)
        {
            DontDestroyOnLoad(gameObject);
            timeKeeper = this;
        }
        else if(timeKeeper != this)
        {
            Destroy(gameObject);
        }
	}
}
