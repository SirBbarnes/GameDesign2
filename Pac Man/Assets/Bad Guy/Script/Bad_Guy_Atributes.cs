using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class Bad_Guy_Atributes : NetworkComponent
{
    public int health;
    // Start is called before the first frame update
    void Start()
    {
        health = 3;
    }

    void OnCollisionEnter(Collision collision)
	{
        if (collision.gameObject.name == "Bullet")
        {
            health--;
        }
	}

    private void Update()
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public override IEnumerator SlowUpdate()
    {
        while (true)
        {
            if (IsServer)
            {
                if (health <= 0)
                {
                    MyCore.NetDestroyObject(NetId);
                }
            }

            yield return new WaitForSeconds(MyCore.MasterTimer);
        }
    }

    public override void HandleMessage(string flag, string value)
    {
        throw new System.NotImplementedException();
    }
}
