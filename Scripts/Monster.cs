using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Chronos;

public class Monster : BaseBehaviour
{
    private InGameManager game;
    private Sweeper sweeper;

    public GameObject waypointHolder;
    public List<Transform> waypoints = new List<Transform>();
    public float waitingTime;
    public float rotationSpeed;
    public bool playerSpotted = false;
    public int monsterLives;
    private Transform player;
    private UnityEngine.AI.NavMeshAgent agent;
    private int waypointIndex;
    private float roamTimer;
    private RaycastHit hit;
    private bool playerHit = false;
    private bool timeOut = false;

    void Start ()
    {
        game = FindObjectOfType<InGameManager>();
        sweeper = FindObjectOfType<Sweeper>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
	}
	
	void Update ()
    {
        if (!timeOut)
        {
            if (playerSpotted)
            {
                AIChasePlayer();
            }
            else
            {
                AIRoam();
            }

            if (Physics.Raycast(transform.position, transform.forward, out hit, 1f))
            {
                if (hit.collider.tag == "Player" && !playerHit && !game.rewinding)
                {
                    playerHit = true;
                    game.PlayerLostLife();
                }
            }
            else
            {
                playerHit = false;
            }
        }
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "Trash" && sweeper.trashFired)
        {
            MonsterLostLife();
            game.DisableGameObject(col.gameObject);
        }
    }

    void MonsterLostLife()
    {
        monsterLives--;

        if(monsterLives == 0)
        {
            game.MonsterKilled(gameObject);
        }

        StartCoroutine(PauseMonster(2));
    }

    void AIChasePlayer()
    {
        agent.transform.position = transform.position;
        agent.destination = player.position;

        Vector3 velocity = agent.desiredVelocity * 3f;
        Vector3 playerPos = new Vector3(player.position.x, player.position.y + 0.5f, player.position.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerPos - transform.position), rotationSpeed * time.deltaTime);
        transform.position += velocity * time.deltaTime;
    }

    void AIRoam()
    {
        agent.speed = 1;

        if(waypoints.Count > 0)
        {
            if(agent.remainingDistance < agent.stoppingDistance)
            {
                roamTimer += time.deltaTime;

                if(roamTimer >= waitingTime)
                {
                    if(waypointIndex == waypoints.Count - 1)
                    {
                        waypointIndex = 0;
                    }
                    else
                    {
                        waypointIndex++;
                    }

                    roamTimer = 0;
                }
            }
            else
            {
                roamTimer = 0;
            }

            agent.transform.position = transform.position;
            agent.destination = waypoints[waypointIndex].position;

            Vector3 velocity = agent.desiredVelocity * 3f;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(waypoints[waypointIndex].position - transform.position), rotationSpeed * time.deltaTime);
            transform.position += velocity * time.deltaTime;
        }
    }

    IEnumerator PauseMonster(int pauseTime)
    {
        timeOut = true;
        yield return time.WaitForSeconds(pauseTime);
        timeOut = false;
    }
}
