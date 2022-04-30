using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    /// <summary>
    /// For keep the healthbar in front of camera
    /// </summary>
    private Quaternion rotation;
    void Awake()
    {
        rotation = Quaternion.Euler(new Vector3(90, 0, 0));
    }

    private void Start()
    {
    }


    private void Update()
    {
        transform.rotation = rotation;
    }
}
