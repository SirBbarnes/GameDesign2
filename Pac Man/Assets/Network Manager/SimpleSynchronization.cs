using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class SimpleSynchronization : NetworkComponent
{
    //synchronized variables
    public int score = 0;
    public int count = 0;
    public int jumpCount = 0;
    public string pname = "";
    public bool canJump = true;

    //Non-synchronized variables
    public float jumpCoolDown = 5.0f;
    public float jumpTimer = 9.0f;

    public override void HandleMessage(string flag, string value)
    {
        if (flag == "SCORE")
        {
            if (IsClient)
            {
                score = int.Parse(value);
            }
        }

        if(flag == "PN")
        {
            pname = value;

            if(IsServer)
            {
                SendUpdate("PN", value);
            }
        }

        if(flag == "CanJump")
        {
            //client
            canJump = bool.Parse(value);
        }

        if (flag == "JUMP")
        {
            if (IsServer && canJump == true)
            {
                //server
                jumpCount++;
                canJump = false;
                SendUpdate("CanJump", false.ToString());
                SendUpdate("JumpCount", jumpCount.ToString());

                //Set a timer
                StartCoroutine(waitForJump());
            }

            if (IsClient)
            {
                jumpCount = int.Parse(value);
            }
        }

        //if(flag == "JumpCount")....
    }

    public IEnumerator waitForJump()
    {
        yield return new WaitForSeconds(jumpCoolDown);
        canJump = true;
        SendUpdate("CanJump", true.ToString());
    }

    public override IEnumerator SlowUpdate()
    {
        //initialize your class
        //Netowrk Start code would go here
        while(true)
        {
            //Game Logic Loop
            if(IsClient)
            {

            }

            if (IsLocalPlayer)
            {
                if(Input.GetAxisRaw("Jump") > 0 && canJump)
                {
                    SendCommand("JUMP", "1");
                    jumpTimer = jumpCoolDown;
                }

                if(jumpTimer > 0)
                {
                    jumpTimer -= MyCore.MasterTimer;
                }
            }

            if(IsServer)
            {
                count++;
                if(count % 10 == 0)
                {
                    //increase score
                    setScore(score += 1);

                }

                if (IsDirty)
                {
                    //Send all synchronized info.
                    SendUpdate("SCORE", score.ToString());
                    SendUpdate("JUMP", jumpCount.ToString());
                    IsDirty = false;
                }
            }

            yield return new WaitForSeconds(MyCore.MasterTimer);
        }
    }

    public void SetPlayerName(string n)
    {
        SendCommand("PN", n);
    }

    public void setScore(int s)
    {
        if (IsServer)
        {
            score = s;
            SendUpdate("SCORE", score.ToString());
        }
    }
}
