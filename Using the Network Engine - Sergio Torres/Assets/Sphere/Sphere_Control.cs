using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;
using UnityEngine.UI;

public class Sphere_Control : NetworkComponent
{
    public bool canDestroy;
    public Button myButt;
    public bool clicked;

    public override void HandleMessage(string flag, string value)
    {
        if (flag == "Destroy")
        {
            if (!IsLocalPlayer)
            {
                MyCore.NetDestroyObject(MyId.NetId);
            }
        }
    }

    public override IEnumerator SlowUpdate()
    {
        if (IsClient)
        {
           
        }

        if (IsServer)
        {

        }

        if (IsLocalPlayer)
        {

            
        }

        yield return new WaitForSeconds(MyCore.MasterTimer);
    }



    public void destroySphere()
    {
        if (IsLocalPlayer)
        {
            SendCommand("Destroy", "1");
        }
    }
}
