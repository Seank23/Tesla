using UnityEngine;
using Chronos;
using System.Collections.Generic;
using System.Collections;

public class Sweeper : BaseBehaviour
{
    private UIControl ui;
    private InGameManager game;
    private Animator animator;

    public LayerMask layerMask;
    public GameObject[] items;
    public float rayDistance;
    public int itemIndex = 0;
    public bool trashFired = false;
    private RaycastHit itemToBeCollected;
    public List<string> itemsCollectedNames = new List<string>();
    private double delayTimeElapsed;
    private double absorbTime;
    private float absorbPower;
    private int maxMass;
    private bool collecting = false;
    private bool rayHit = false;
    private bool firing = false;
    private Dictionary<string, int> trashDict = new Dictionary<string, int>();

    void Start()
    {
        ui = FindObjectOfType<UIControl>();
        game = FindObjectOfType<InGameManager>();
        animator = GetComponent<Animator>();

        foreach (GameObject item in items)
        {
            trashDict.Add(item.name, 0);
        }

        InitializeSweeper();
    }

    public void TrashInstances()
    {
        itemsCollectedNames.Clear();

        int numberOfIterations = 0;
        List<string> trashList = new List<string>();

        foreach (KeyValuePair<string, int> item in trashDict)
        {
            numberOfIterations += item.Value;
            for (int i = 0; i < item.Value; i++)
            {
                trashList.Add(item.Key);
            }
        }

        foreach (GameObject item in items)
        {
            trashDict[item.name] = 0;
        }

        itemIndex = 0;

        for (int i = 0; i < numberOfIterations; i++)
        {
            RespawnTrash(trashList[i]);
        }
    }

    void RespawnTrash(string name)
    {
        itemsCollectedNames.Add(name);
        trashDict[name]++;
        itemIndex++;

        if (itemIndex > PlayerDataControl.data.trashSlots)
        {
            itemIndex--;
            itemsCollectedNames.RemoveAt(0);
            ui.DestroyTrashSprite(true);
        }

        ui.DisplayTrashUI(name);
    }

    void Update ()
    {
        if (Input.GetButtonDown("Collect Trash"))
        {
            animator.SetBool("firing", false);
            RaycastHit hit;
            Vector3 forward = transform.TransformDirection(Vector3.forward) * -rayDistance;
            Vector3 rayPos = new Vector3(transform.position.x, (transform.position.y - 0.5f), transform.position.z);
            Debug.DrawRay(rayPos, forward, Color.green);

            if (Physics.Raycast(rayPos, forward, out hit, rayDistance, layerMask))
            {
                itemToBeCollected = hit;
                absorbTime = absorbPower * hit.rigidbody.mass;
                delayTimeElapsed = 0;
                if(hit.rigidbody.mass <= maxMass)
                {
                    rayHit = true;
                    animator.SetBool("absorbing", true);
                }
            }
        }
        else if(Input.GetButtonDown("Fire Trash") && !game.inMenu)
        {
            if (itemIndex > 0 && !trashFired)
            {
                itemIndex--;
                string nameOfItem = itemsCollectedNames[itemIndex]; 
                FireTrash(nameOfItem);
                itemsCollectedNames.RemoveAt(itemIndex);
            }
        }

        if (Input.GetButton("Collect Trash"))
        {
            collecting = true;
        }
        else
        {
            collecting = false;
            rayHit = false;
            animator.SetBool("absorbing", false);
        }

        if (collecting && rayHit && !firing)
        {
            delayTimeElapsed += Time.deltaTime;
            itemToBeCollected.rigidbody.AddForce((transform.forward * itemToBeCollected.rigidbody.mass + (Vector3.up * itemToBeCollected.rigidbody.mass)) / (1 / (itemToBeCollected.distance * 2)));

            if (delayTimeElapsed > absorbTime)
            {
                rayHit = false;
                CollectTrash(itemToBeCollected);
                animator.SetBool("absorbing", false);
            }
        }
    }

    void InitializeSweeper()
    {
        if (PlayerDataControl.data.sweeperType == 0)
        {
            absorbPower = 0.05f;
            maxMass = 50;
            GetComponent<Renderer>().materials[0].color = Color.red;
        }

        if (PlayerDataControl.data.sweeperType == 1)
        {
            absorbPower = 0.007f;
            maxMass = 300;
            GetComponent<Renderer>().materials[0].color = Color.green;
        }

        if (PlayerDataControl.data.sweeperType == 2)
        {
            absorbPower = 0.0001f;
            maxMass = 1000;
            GetComponent<Renderer>().materials[0].color = Color.blue;
        }
    }

    void CollectTrash(RaycastHit hit)
    {
        time.Do
        (
            false,
            delegate()
            {
                GameObject target = hit.collider.gameObject;

                itemsCollectedNames.Add(target.name);
                trashDict[target.name]++;
                itemIndex++;

                if (itemIndex > PlayerDataControl.data.trashSlots)
                {
                    itemIndex--;
                    trashDict[itemsCollectedNames[0]]--;
                    itemsCollectedNames.RemoveAt(0);
                    ui.DestroyTrashSprite(true);
                }

                ui.DisplayTrashUI(target.name);

                game.DisableGameObject(target);

                return target.name;
            },
            delegate(string name)
            {
                trashDict[name]--;
            }
        );   
    }

    void FireTrash(string nameOfItem)
    {
        animator.SetBool("firing", true);
        firing = true;
        trashFired = true;

        foreach (GameObject item in items)
        {
            if(item.name == nameOfItem)
            {
                time.Do
                (
                    false,
                    delegate()
                    {
                        trashDict[nameOfItem]--;
                        Vector3 instancePos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
                        GameObject firedItem = Instantiate(item, instancePos, Quaternion.identity) as GameObject;
                        firedItem.name = item.name;
                        Rigidbody rb = firedItem.GetComponent<Rigidbody>();
                        rb.AddForce(-transform.forward * rb.mass * 1000);

                        ui.DestroyTrashSprite(false);
                        StartCoroutine(StopAnimation("firing"));

                        return firedItem;
                    },
                    delegate(GameObject firedItem)
                    {
                        print(firedItem.name);
                        trashDict[firedItem.name]++;
                        Destroy(firedItem);
                    }
                );  
            }
        }
        firing = false;
        StartCoroutine(FiringFinished());
    }

    IEnumerator StopAnimation(string anim)
    {
        yield return new WaitForSeconds(0.3f);
        animator.SetBool(anim, false);
    }

    IEnumerator FiringFinished()
    {
        yield return gameObject.GetComponent<Timeline>().WaitForSeconds(1f);
        trashFired = false;
    }
}
