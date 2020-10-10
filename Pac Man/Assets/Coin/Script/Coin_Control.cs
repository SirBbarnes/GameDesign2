using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class Coin_Control : NetworkComponent
{
    GameObject temp;
    int score;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "GoldCoin")
        {
            temp = GameObject.Find("GoldCoin");
            Destroy(temp);

            score++;



        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override IEnumerator SlowUpdate()
    {
        if (IsLocalPlayer)
        {

        }

        if (IsClient)
        {

        }

        if (IsServer)
        {

        }

        yield return new WaitForSeconds(MyCore.MasterTimer);
    }

    public override void HandleMessage(string flag, string value)
    {
        if (flag == "Coin")
        {
            score++;

            if (IsServer)
            {
                SendUpdate("Coin", score.ToString());
            }
        }
    }
}
