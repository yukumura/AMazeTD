using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Object", menuName = "Object for TD")]
public class ObjectBlueprint : ScriptableObject
{
    public GameObject prefab;
    public int cost;
    public int valueToSell;
    public bool isUpgradeable;
    public ObjectBlueprint upgrade;
}
