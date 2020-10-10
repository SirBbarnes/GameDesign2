using System.Collections;
using System.Collections.Generic;
using NETWORK_ENGINE;
using UnityEngine;
using UnityEngine.UI;

public class Play_Manager_Control : NetworkComponent
{
    public int score;
    private int count;
    Text scoreText;
    Text playText;
    public Button myButt;
    GameObject[] players;
    int playCount;
    public int sphere;

    private void Start()
    {
        scoreText = GameObject.FindGameObjectWithTag("Text").GetComponent<Text>();
        playText = GameObject.FindGameObjectWithTag("playText").GetComponent<Text>();

        if (!IsLocalPlayer)
        {
            GetComponentInParent<Canvas>().enabled = false;
        }
    }

    public override void HandleMessage(string flag, string value)
    {
        if (flag == "SCORE")
        {
            if (IsClient)
            {
                score = int.Parse(value);
            }
        }

        if (flag == "Create")
        {
            if (IsServer)
            {
                MyCore.NetCreateObject(sphere, MyId.NetId, new Vector3(Random.Range(-4, 4), Random.Range(-4, 4)));

            }
        }

        if (flag == "Players")
        {
            if (IsServer)
            {
                playCount = int.Parse(value);
            }
        }
    }


    public override IEnumerator SlowUpdate()
    {
        while(true)
        {

            if (IsServer)
            {
                count++;
                if (count % 10 == 0)
                {
                    setScore(score += 1);
                }
                if (IsDirty)
                {
                    //Send all synchronized info.
                    SendUpdate("SCORE", score.ToString());
                    IsDirty = false;
                }
            }

            if (IsClient)
            {

                players = GameObject.FindGameObjectsWithTag("Player");
                setPlayerCount(players.Length);

            }

            if (IsLocalPlayer)
            {
                playText.text = "No. of Players: " + playCount.ToString();
                scoreText.text = "Player Score: " + score.ToString();

            }

            yield return new WaitForSeconds(MyCore.MasterTimer);
        }
    }

    public void setPlayerCount(int t)
    {
        if (IsClient)
        {
            playCount = t;
            SendCommand("Players", playCount.ToString());
        }
    }

    public void setScore(int s)
    {
        if (IsServer)
        {
            score = s;
            SendUpdate("SCORE", score.ToString());
        }
    }

    public void buttClick()
    {
        if (IsLocalPlayer)
        {
            SendCommand("Create", "1");
        }
    }
}
