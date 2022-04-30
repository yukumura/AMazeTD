using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "Wave for TD")]

//Set of wave describe how round is structured
public class WaveDescriptor : ScriptableObject
{
    public int goldBonus;

    public Wave[] wave;

}
