using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Control : MonoBehaviour
{
    public int quantity;
    public int cost;
    public int rarity; 
    //Other stuff

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override string ToString()
    {
        return quantity + "_" + cost + "_" + rarity;
    }

    public static Item_Control Parse(string input)
    {
        Item_Control temp = new Item_Control();
        string[] info = input.Split('_');

        temp.quantity = int.Parse(info[0]);
        temp.cost = int.Parse(info[1]);
        temp.rarity = int.Parse(info[2]);

        return temp;
    }

    public void FromString(string input)
    {
        string[] info = input.Split('_');

        quantity = int.Parse(info[0]);
        cost = int.Parse(info[1]);
        rarity = int.Parse(info[2]);
    }
}

