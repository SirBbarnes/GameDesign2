using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class Network_Transform : NetworkComponent
{
    public Vector3 prevPos = Vector3.zero;
    public Vector3 prevRot = Vector3.zero;

    public override void HandleMessage(string flag, string value)
    {
        char[] remove = { '(', ')' };
        if (string.CompareOrdinal(flag, "POS") == 0)
        {
            string[] data = value.Trim(remove).Split(',');
            this.transform.position = new Vector3(
                                float.Parse(data[0]),
                                float.Parse(data[1]),
                                float.Parse(data[2])
                                );

        }
        if (string.CompareOrdinal(flag, "ROT") == 0)
        {
            string[] data = value.Trim(remove).Split(',');
            Vector3 euler = new Vector3(
                                float.Parse(data[0]),
                                float.Parse(data[1]),
                                float.Parse(data[2])
                                );
            this.transform.rotation = Quaternion.Euler(euler);
        }
    }
    public override IEnumerator SlowUpdate()
    {

        while (IsServer)
        {
            if ((prevPos - this.transform.position).magnitude > 0.01f)
            {
                SendUpdate("POS", this.transform.position.ToString());
                prevPos = this.transform.position;
            }
            if (prevRot != this.transform.rotation.eulerAngles)
            {
                SendUpdate("ROT", this.transform.rotation.eulerAngles.ToString());
                prevRot = this.transform.rotation.eulerAngles;
            }

            if (IsDirty)
            {
                SendUpdate("POS", this.transform.position.ToString());
                SendUpdate("ROT", this.transform.rotation.eulerAngles.ToString());
                IsDirty = false;
            }
            yield return new WaitForSeconds(MyCore.MasterTimer); //potentially slower
        }
    }
}
