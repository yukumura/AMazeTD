using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//describe how the wave is composed
[Serializable]
public class Wave
{
    public GameObject monster;
    public int count;
    public float rateForSpawn;
}
