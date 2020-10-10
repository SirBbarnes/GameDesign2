using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;
using UnityEngine.UI;

public class Player_Option_Control : NetworkComponent
{
    public Text fakeText;
    public string nameText;
    GameObject temp;
    public int i = 6, j;
    bool firstTime = true;
    Vector3 cam;
    int count;
    Vector3 spawn;
    GameObject[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        fakeText = transform.GetChild(0).transform.GetChild(0).transform.GetChild(4).GetComponent<Text>();
        spawn = GameObject.FindGameObjectWithTag("Respawn").GetComponent<Transform>().position;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    public override void HandleMessage(string flag, string value)
    {
        if (flag == "Name")
        {
            fakeText.text = value;
        }

        if (flag == "Next")
        {
            j = i;

            if (firstTime == true)
            {
                firstTime = false;

                i++;

                if (IsServer)
                {
                    temp = MyCore.NetCreateObject(i, 1, new Vector3(0, 0, 0));
                    temp.transform.localScale = new Vector3(5, 5, 5);
                    temp.transform.LookAt(Camera.main.transform.forward);
                }
            }

            if (firstTime == false)
            {
                MyCore.NetDestroyObject(temp.GetComponent<NetworkID>().NetId);

                i++;

                if (i >= 9)
                {
                    i = 5;
                    j = 8;
                }

                if (IsServer)
                {
                    temp = MyCore.NetCreateObject(i, 1, new Vector3(0, 0, 0));
                    temp.transform.localScale = new Vector3(5, 5, 5);
                    temp.transform.LookAt(Camera.main.transform);
                }
            }
        }

        if (flag == "Previous")
        {
            j = i;

            if (firstTime == true)
            {
                firstTime = false;

                i--;

                if (IsServer)
                {
                    temp = MyCore.NetCreateObject(i, 1, new Vector3(0, 0, 0));
                    temp.transform.localScale = new Vector3(4, 4, 4);
                    temp.transform.LookAt(Camera.main.transform);
                }
            }

            if (firstTime == false)
            {
                MyCore.NetDestroyObject(temp.GetComponent<NetworkID>().NetId);

                i--;

                if (i <= 4)
                {
                    i = 8;
                    j = 5;
                }

                if (IsServer)
                {
                    temp = MyCore.NetCreateObject(i, 1, new Vector3(0, 0, 0));
                    temp.transform.localScale = new Vector3(5, 5, 5);
                    temp.transform.LookAt(Camera.main.transform);
                }
            }
        }

        if (flag == "Ready")
        {

                count++;

                this.gameObject.SetActive(false);

                MyCore.NetDestroyObject(temp.GetComponent<NetworkID>().NetId);

                switch(i)
                {
                    case 5:
                        i = 1;
                        break;

                    case 6:
                        i = 2;
                        break;

                    case 7:
                        i = 3;
                        break;

                    case 8:
                        i = 4;
                        break;
                }

            if (IsServer)
            {
                temp = MyCore.NetCreateObject(i, 1, spawn);
                temp.transform.GetChild(0).GetComponent<TextMesh>().text = nameText;
                Camera.main.transform.position = new Vector3(743, 1278, -2878);
                Camera.main.transform.Rotate(74, 0, 0);

                foreach (GameObject enemy in enemies)
                {
                    enemy.GetComponent<Bad_Guy_Movement>().enabled = true;
                }
            }
        }

    }

    public override IEnumerator SlowUpdate()
    {
        while(true)
        {
            if (IsServer)
            {

                if (IsDirty)
                {
                    SendUpdate("Name", fakeText.ToString());
                }
            }

            if (IsClient)
            {

            }

            if (IsLocalPlayer)
            {

            }

            yield return new WaitForSeconds(MyCore.MasterTimer);
        }
    }

    public void nextCharacter()
    {
        if (IsLocalPlayer)
        {
            SendCommand("Next", "1");
        }
    }

    public void previousCharacter()
    {
        if (IsLocalPlayer)
        {
            SendCommand("Previous", "1");
        }
    }

    public void Ready()
    {
        if (IsLocalPlayer)
        {
            SendCommand("Ready", "1");
            this.gameObject.SetActive(false);
            Camera.main.transform.position = new Vector3(743, 1278, -2878);
            Camera.main.transform.Rotate(74, 0, 0);
        }
    }

    public void setName(string value)
    {
        fakeText.text = value;
        SendCommand("Name", value);
    }
}
