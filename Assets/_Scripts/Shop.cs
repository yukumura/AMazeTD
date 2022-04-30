using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    public ObjectBlueprint standardWall;
    public ObjectBlueprint standardTurret;
    public ObjectBlueprint rockTurret;

    public Text textStandardWall;
    public Text textStandardTurret;
    public Text textRockTurret;

    BuildManager buildManager;
    GameManager gameManager;

    private void OnEnable()
    {
        buildManager = BuildManager.instance;
        gameManager = GameManager.instance;
    }

    private void Update()
    {
        if(gameManager.GetTileSelected() == null)
            gameObject.SetActive(false);

        UpdateInformationButtons();
    }

    /// <summary>
    /// UI method for select objects
    /// </summary>
    public void SelectStandardTurret()
    {
        buildManager.SelectTurretToBuild(standardTurret);
        buildManager.BuildObjectOn(gameManager.GetTileSelected());
        gameManager.DeselectTile();
    }

    /// <summary>
    /// UI method for select objects
    /// </summary>
    public void SelectRockTurret()
    {
        buildManager.SelectTurretToBuild(rockTurret);
        buildManager.BuildObjectOn(gameManager.GetTileSelected());
        gameManager.DeselectTile();
    }

    /// <summary>
    /// UI method for select objects
    /// </summary>
    public void SelectStandardWall()
    {
        buildManager.SelectTurretToBuild(standardWall);
        buildManager.BuildObjectOn(gameManager.GetTileSelected());
        gameManager.DeselectTile();
    }

    /// <summary>
    /// Update information related to player gold
    /// </summary>
    public void UpdateInformationButtons()
    {
        if (gameManager.GetGolds() < standardTurret.cost)
            textStandardTurret.text = "Not enough money";
        else
            textStandardTurret.text = standardTurret.cost.ToString();

        if (gameManager.GetGolds() < rockTurret.cost)
            textRockTurret.text = "Not enough money";
        else
            textRockTurret.text = rockTurret.cost.ToString();

        if (gameManager.GetGolds() < standardWall.cost)
            textStandardWall.text = "Not enough money";
        else
            textStandardWall.text = standardWall.cost.ToString();
    }
}
