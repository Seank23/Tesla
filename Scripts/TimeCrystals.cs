using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Chronos;

public class TimeCrystals : BaseBehaviour
{
    private InGameManager game;
    private Rigidbody rb;
    private Collider col;
    private float[] newCrystalPosition = new float[3];

	void Start ()
    {
        game = FindObjectOfType<InGameManager>();
        rb = gameObject.GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        rb.isKinematic = false;
	}
	
	void Update ()
    {
        if (time.timeScale > 0)
        {
            // Makes each time crystal rotate
            transform.Rotate(0, 180 * time.deltaTime, 0, Space.World);
        }

        if(Physics.Raycast(transform.position, Vector3.down, 0.5f))
        {
            rb.isKinematic = true;
        }
        else
        {
            rb.isKinematic = false;
        }

        if(game.crystalIndex == 3)
        {
            col.enabled = false;
        }
        else
        {
            col.enabled = true;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player" && !game.rewinding)
        {
            newCrystalPosition[0] = transform.position.x;
            newCrystalPosition[1] = transform.position.y + 2;
            newCrystalPosition[2] = transform.position.z;

            game.CrystalCollected(gameObject.name, newCrystalPosition);
            game.DisableGameObject(gameObject);
        }
    }
}
