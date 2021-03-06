using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelStats : MonoBehaviour
{
    [Header("Manage Level")]
    public Text TextLevel;

    [Header("Manage Lives")]
    public Text TextLives;

    [Header("Manage Gold")]
    public Text TextGold;

    [Header("Manage Monster")]
    public Text TextMonster;

    private GameManager gameManager;
    // Use this for initialization

    void Start()
    {
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInformation();
    }

    /// <summary>
    /// Update information related to generic information
    /// </summary>
    public void UpdateInformation()
    {
        TextLevel.text = gameManager.GetLevel().ToString();
        TextMonster.text = gameManager.GetMonsters().ToString();
        TextGold.text = gameManager.GetGolds().ToString();
        TextLives.text = gameManager.GetLives().ToString();
    }
}
