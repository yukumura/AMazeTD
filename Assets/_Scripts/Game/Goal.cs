using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{

    public GameObject particleLivesLost;
    public Vector3 positionOffset;
    public AudioClip reachGoalSound;
    private GameManager gameManager;
    private ManagerUI managerUI;
    private AudioSource audioSource;

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
        managerUI = gameManager.GetComponent<ManagerUI>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            //logic for leave points
            gameManager.RemoveMonster(1);
            gameManager.RemoveLives(enemy.lifesToRemove);
            Destroy(collision.gameObject);
            Instantiate(particleLivesLost, GetBuildPosition(), transform.rotation);

            if (managerUI.effectFlag)
            {
                //audioSource.clip = collision.gameObject.GetComponent<Enemy>().deathSound;
                audioSource.clip = reachGoalSound;
                audioSource.Play();
            }                     
        }
    }
}
