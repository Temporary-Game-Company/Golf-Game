using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Equipment
{
    public static List<Equipment> Equipments;
    public int id;
    public string name;
    public string type;
    public int damage;

    public Equipment(int id)
    {
        this.id = id;

        
    }
}

