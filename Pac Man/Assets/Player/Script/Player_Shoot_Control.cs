using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class Player_Shoot_Control : NetworkComponent
{
    bool canShoot;
    float shootTimer = 5.0f;
    float shootCoolDown = 5.0f;
    GameObject player;
    GameObject temp;

    public override void HandleMessage(string flag, string value)
    {
        if (flag == "Shoot")
        {
            if (IsServer && canShoot == true)
            {
                temp = MyCore.NetCreateObject(3, 500, new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z));
                temp.GetComponent<Rigidbody>().AddForce(transform.forward * 10);
                canShoot = false;
                SendUpdate("Can Shoot", false.ToString());
                StartCoroutine(waitforShoot());
            }
        }
    }

    public IEnumerator waitforShoot()
    {
        yield return new WaitForSeconds(shootCoolDown);
        canShoot = true;
        SendUpdate("Can Shoot", true.ToString());
    }

    public override IEnumerator SlowUpdate()
    {
        while (true)
        {
            if (IsLocalPlayer)
            {
                if (Input.GetAxisRaw("Jump") > 0 && canShoot)
                {
                    SendCommand("Shoot", "1");


                }

                if (shootTimer > 0)
                {
                    shootTimer -= MyCore.MasterTimer;
                }
            }

            yield return new WaitForSeconds(MyCore.MasterTimer);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
