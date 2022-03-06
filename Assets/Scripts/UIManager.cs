using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public GameManager gameManager;

    public GameObject towerButtonPrefab;

    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI waveText;

    public GameObject towersParent;

    public void UpdateMoneyText(int amount)
    {
        moneyText.text = amount.ToString() + " $";
    }

    public void UpdateWaveText()
    {
        waveText.text = "Wave: " + gameManager.GetWave();
    }

    public void Initialize(GameManager gameManager, int money)
    {
        this.gameManager = gameManager;
        UpdateMoneyText(money);
    }

    public void GenerateTowerButtons()
    {
        int n = 0;
        foreach (GameObject tower in gameManager.interactionManager.towersPrefabs)
        {
            Tower towerComponent = tower.GetComponent<Tower>();
            GameObject button = Instantiate(towerButtonPrefab, towersParent.transform);
            //TODO: Rewrite this
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = towerComponent.gameObject.name;
            button.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = towerComponent.cost.ToString() + " $";
            TowerButton towerButComponnet = button.GetComponent<TowerButton>();

            towerButComponnet.towerIndex = n;

            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                gameManager.interactionManager.EquipTower(towerButComponnet.towerIndex);
            });
            n++;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateTowerButtons();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
