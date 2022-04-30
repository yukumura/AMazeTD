using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    public ObjectBlueprint standardWall;
    public ObjectBlueprint standardTurret;
    public ObjectBlueprint rockTurret;
    public ObjectBlueprint poisonTurret;
    public ObjectBlueprint sniperTurret;

    public Text textStandardWall;
    public Text textStandardTurret;
    public Text textRockTurret;
    public Text textPoisonTurret;
    public Text textSniperTurret;

    BuildManager buildManager;
    GameManager gameManager;
    ManagerUI managerUI;

    private void OnEnable()
    {
        buildManager = BuildManager.instance;
        gameManager = GameManager.instance;
        managerUI = gameManager.GetComponent<ManagerUI>();
        UpdateInformationButtons();
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
        if (managerUI.pauseFlag) return;

        buildManager.SelectTurretToBuild(standardTurret);
    }

    /// <summary>
    /// UI method for select objects
    /// </summary>
    public void SelectRockTurret()
    {
        if (managerUI.pauseFlag) return;

        buildManager.SelectTurretToBuild(rockTurret);
    }

    public void SelectPoisonTurret()
    {
        if (managerUI.pauseFlag) return;

        buildManager.SelectTurretToBuild(poisonTurret);
    }

    public void SelectSniperTurret()
    {
        if (managerUI.pauseFlag) return;

        buildManager.SelectTurretToBuild(sniperTurret);
    }

    /// <summary>
    /// UI method for select objects
    /// </summary>
    public void SelectStandardWall()
    {
        if (managerUI.pauseFlag) return;
    
        buildManager.SelectTurretToBuild(standardWall);
    }

    /// <summary>
    /// Update information related to player gold
    /// </summary>
    public void UpdateInformationButtons()
    {
        if (gameManager.GetGolds() < standardTurret.cost)
            textStandardTurret.transform.parent.gameObject.SetActive(false);
        else
        {
            textStandardTurret.transform.parent.gameObject.SetActive(true);
            textStandardTurret.text = standardTurret.cost.ToString();
        }

        if (gameManager.GetGolds() < rockTurret.cost)
            textRockTurret.transform.parent.gameObject.SetActive(false);
        else
        {
            textRockTurret.transform.parent.gameObject.SetActive(true);
            textRockTurret.text = rockTurret.cost.ToString();
        }

        if (gameManager.GetGolds() < standardWall.cost)
            textStandardWall.transform.parent.gameObject.SetActive(false);
        else
        {
            textStandardWall.transform.parent.gameObject.SetActive(true);
            textStandardWall.text = standardWall.cost.ToString();
        }

        if (gameManager.GetGolds() < poisonTurret.cost)
            textPoisonTurret.transform.parent.gameObject.SetActive(false);
        else
        {
            textPoisonTurret.transform.parent.gameObject.SetActive(true);
            textPoisonTurret.text = poisonTurret.cost.ToString();
        }

        if (gameManager.GetGolds() < sniperTurret.cost)
            textSniperTurret.transform.parent.gameObject.SetActive(false);
        else
        {
            textSniperTurret.transform.parent.gameObject.SetActive(true);
            textSniperTurret.text = sniperTurret.cost.ToString();
        }
    }
}
