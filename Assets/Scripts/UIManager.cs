using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

    public GameManager gameManager;
    

    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI waveText;

    public void UpdateMoneyText(int amount)
    {
        moneyText.text = amount.ToString() + " $";
    }

    public void UpdateWaveText()
    {
        waveText.text = "Wave " + gameManager.GetWave();        
    }

    public void Initialize(GameManager gameManager, int money)
    {
        this.gameManager = gameManager;
        UpdateMoneyText(money);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
