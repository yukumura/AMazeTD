using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{

    public GameObject particleLivesLost;
    public Vector3 positionOffset;
    private GameManager gameManager;

    /// <summary>
    /// Get position for instantiate death particle effect
    /// </summary>
    /// <returns></returns>
    public Vector3 GetBuildPosition()
    {
        return transform.position + positionOffset;
    }
    // Use this for initialization
    void Start()
    {
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Enemy")
        {
            //logic for leave points
            gameManager.AddMonster(-1);
            gameManager.AddLives(-1);
            Destroy(collision.gameObject);
            Instantiate(particleLivesLost, GetBuildPosition(), transform.rotation);
        }
    }
}
