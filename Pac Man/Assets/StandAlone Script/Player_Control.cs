using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class Player_Control : NetworkComponent
{
    public Vector3 lastPos = Vector3.zero;
    public Vector3 lastRot = Vector3.zero;
    public Vector3 lastVel = Vector3.zero;
    public Vector3 lastAng = Vector3.zero;


    Vector3 tempPos = Vector3.zero;
    Vector3 tempRot = Vector3.zero;
    Vector3 tempVel = Vector3.zero;
    Vector3 tempAng = Vector3.zero;


    public override void HandleMessage(string flag, string value)
    {
        if (flag == "Move")
        {
            char[] removeParen = { '(', ')' };
            string[] data = value.Trim(removeParen).Split(',');

            lastPos = new Vector3(float.Parse(data[0]), float.Parse(data[1]), float.Parse(data[2]));
        }

        if (flag == "Rotate")
        {
            char[] removeParen = { '(', ')' };
            string[] data = value.Trim(removeParen).Split(',');

            lastRot = new Vector3(float.Parse(data[0]), float.Parse(data[1]), float.Parse(data[2]));
        }

        if (flag == "Velocity")
        {
            char[] removeParen = { '(', ')' };
            string[] data = value.Trim(removeParen).Split(',');

            lastVel = new Vector3(float.Parse(data[0]), float.Parse(data[1]), float.Parse(data[2]));
        }

        if (flag == "Angular Velocity")
        {
            char[] removeParen = { '(', ')' };
            string[] data = value.Trim(removeParen).Split(',');

            lastAng = new Vector3(float.Parse(data[0]), float.Parse(data[1]), float.Parse(data[2]));
        }
    }

    public override IEnumerator SlowUpdate()
    {
        while (true)
        {
            if (IsLocalPlayer)
            {
                tempPos = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);

                if ((tempPos - lastPos).magnitude > 0.01f)
                {
                    SendCommand("Move", tempPos.ToString());
                    lastPos = tempPos;
                }

                tempRot = new Vector3(this.transform.localRotation.x, this.transform.localRotation.y, this.transform.localRotation.z);

                if ((tempRot - lastRot).magnitude > 0.01f)
                {
                    SendCommand("Rotate", tempRot.ToString());
                    lastRot = tempRot;
                }


                tempVel = this.gameObject.GetComponent<Rigidbody>().velocity;

                if ((tempVel - lastVel).magnitude > 0.01f)
                {
                    SendCommand("Velocity", tempVel.ToString());
                    lastVel = tempVel;
                }

                tempAng = this.gameObject.GetComponent<Rigidbody>().angularVelocity;

                if ((tempAng - lastAng).magnitude > 0.01f)
                {
                    SendCommand("Angular Velocity", tempAng.ToString());
                    lastAng = tempAng;
                }
            }
            yield return new WaitForSeconds(MyCore.MasterTimer);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Only on Client Side
        //Graphical Effects
        if (IsServer)
        {
            this.transform.position += lastPos * Time.deltaTime;
            
        }
    }
}


