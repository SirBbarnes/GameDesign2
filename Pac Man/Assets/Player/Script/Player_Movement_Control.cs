using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class Player_Movement_Control : NetworkComponent
{
    public Vector3 lastInput = Vector3.zero;
    float forward;
    float rotat;
    float speed;
    Vector3 forw;
    Vector3 rot;
    float turnSpeed;

    public override void HandleMessage(string flag, string value)
    {
        if (flag == "Go")
        {
            float dir = int.Parse(value);

            forw= this.transform.position + this.transform.forward * dir * speed * Time.deltaTime;
            this.transform.position = Vector3.Lerp(this.transform.position, forw, 0.7f);
        }

        if (flag == "Turn")
        {
            float turning = int.Parse(value);

            rot = this.transform.rotation.eulerAngles + new Vector3(0, turning * turnSpeed, 0);
            this.transform.rotation = Quaternion.Euler(Vector3.Lerp(this.transform.rotation.eulerAngles, rot, 0.7f));
        }
    }

    public override IEnumerator SlowUpdate()
    {
        while (true)
        {
            if (IsLocalPlayer)
            {
                forward = 0;
                rotat = 0;

                forward = Input.GetAxisRaw("Vertical");
                if (forward != 0)
                {
                    SendCommand("Go", forward.ToString());
                }

                rotat = Input.GetAxisRaw("Horizontal");
                if (rotat != 0)
                {
                    SendCommand("Turn", rotat.ToString());
                }
            }

            if (IsServer)
            {

            }

            
            yield return new WaitForSeconds(MyCore.MasterTimer);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}