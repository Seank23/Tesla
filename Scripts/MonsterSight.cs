using UnityEngine;
using System.Collections;

public class MonsterSight : MonoBehaviour
{
    private Monster monster;

    void Start()
    {
        monster = GetComponentInParent<Monster>();
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            monster.playerSpotted = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(col.tag == "Player")
        {
            monster.playerSpotted = false;
        }
    }
}
