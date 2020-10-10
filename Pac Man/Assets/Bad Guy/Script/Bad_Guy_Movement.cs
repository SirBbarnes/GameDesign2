using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class Bad_Guy_Movement : NetworkComponent
{
    GameObject player;
    UnityEngine.AI.NavMeshAgent myNav;
    public Transform[] movePoints;
    int destPoint;
    float playPos;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        myNav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        myNav.autoBraking = false;
        NextPoint();
    }

    void NextPoint()
	{
        if (movePoints.Length == 0)
            return;

        myNav.destination = movePoints[destPoint].position;
        destPoint = (destPoint + 1) % movePoints.Length;
	}

    public override IEnumerator SlowUpdate()
    {
        while (true)
        {
            if (IsServer)
            {
                playPos = Vector3.Distance(player.transform.position, transform.position);

                NextPoint();

                if (playPos < 2.5)
                {
                    myNav.SetDestination(player.transform.position);
                }

                if (!myNav.pathPending && myNav.remainingDistance < 0.5f)
                {
                    NextPoint();
                }
            }
        }
    }

    public override void HandleMessage(string flag, string value)
    {

    }
}
