using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("Music", 99) == 99 || PlayerPrefs.GetInt("Music", 99) == 1)
            GetComponent<AudioSource>().volume = 0.8f;
        else
            GetComponent<AudioSource>().volume = 0f;
        
    }
}
