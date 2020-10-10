using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;
using UnityEngine.UI;

public class Player_Input : NetworkComponent
{
    Vector3 velocityTemp = Vector3.zero;
    Vector3 angularTemp = Vector3.zero;
    Vector3 rotationTemp = Vector3.zero;

    public Vector3 velocityOut;

    public override void HandleMessage(string flag, string value)
    {
        if (flag == "Rotation")
        {
            if (IsServer)
            {
                char[] removeParen = { '(', ')' };
                string[] data = value.Trim(removeParen).Split(',');

                rotationTemp = new Vector3(float.Parse(data[0]), float.Parse(data[1]), float.Parse(data[2]));
            }
        }

        if (flag == "Velocity")
        {
            if (IsServer)
            {
                char[] removeParen = { '(', ')' };
                string[] data = value.Trim(removeParen).Split(',');

                velocityTemp = new Vector3(float.Parse(data[0]), float.Parse(data[1]), float.Parse(data[2]));
            }
        }

        if (flag == "Angular Velocity")
        {
            if (IsServer)
            {
                char[] removeParen = { '(', ')' };
                string[] data = value.Trim(removeParen).Split(',');

                angularTemp = new Vector3(float.Parse(data[0]), float.Parse(data[1]), float.Parse(data[2]));
            }
        }
    }

    public override IEnumerator SlowUpdate()
    {
        if (IsLocalPlayer)
        {

        }

        if (IsServer)
        {

        }
        yield return new WaitForSeconds(MyCore.MasterTimer);
    }

    public void setRotation(string input)
    {
        if (IsLocalPlayer)
        {
            SendCommand("Rotation", input);
            this.gameObject.transform.rotation = Quaternion.Euler(rotationTemp);
        }
    }

    public void setVelocity(string input)
    {
        if (IsLocalPlayer)
        {
            SendCommand("Velocity", input);
            this.gameObject.GetComponent<Rigidbody>().velocity = velocityTemp;
        }
    }

    public void setAngularVelocity(string input)
    {
        if (IsLocalPlayer)
        {
            SendCommand("Angular Velocity", input);
            this.gameObject.GetComponent<Rigidbody>().angularVelocity = angularTemp;
        }
    }
}
