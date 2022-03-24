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
    public GameObject towersOptions;

    public GameObject towersParent;

    public GameObject towerInfoPanel;
    public TextMeshProUGUI towerInfoPanelName;
    public TextMeshProUGUI towerInfoPanelDamage;

    public GameObject youLostPanel;

    public Button sellTower;

    public GameObject namePrompt;
    public TextMeshProUGUI namePromptText;
    public TMP_InputField nameInput;
    public Button namePromptContinue;

    public TextMeshProUGUI usernameText;


    public Button playButton;
    public Button leaderboardButton;
    public Button quitButton;

    public GameObject leaderboardPanel;
    public GameObject leaderboardContentParent;
    public GameObject leaderboardCellPrefab;

    public TextMeshProUGUI playerHealthText;

    public Button speedUpButton;

    public Button settingsButton;
    public GameObject settingsPanel;

    private Tower tower;

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

    public void DisplayTowerInfo(Tower t)
    {
        towerInfoPanel.SetActive(true);
        towerInfoPanelName.text = t.towerName;
        towerInfoPanelDamage.text = t.damageGiven.ToString();
        tower = t;
    }

    public void HideTowerInfo() => towerInfoPanel.SetActive(false);

    public void GenerateTowerButtons()
    {
        int n = 0;
        foreach (GameObject tower in gameManager.towersPrefabs)
        {
            Tower towerComponent = tower.GetComponent<Tower>();
            GameObject button = Instantiate(towerButtonPrefab, towersParent.transform);
            //TODO: Rewrite this
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = towerComponent.gameObject.name;
            button.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = towerComponent.cost.ToString() + " $";
            TowerButton towerButComponnet = button.GetComponent<TowerButton>();

            towerButComponnet.towerIndex = n;

            button.GetComponent<Button>().onClick.AddListener(() => { gameManager.interactionManager.EquipTower(towerButComponnet.towerIndex); });
            n++;
        }
    }

    public void SpeedUpGame()
    {
        TextMeshProUGUI text = speedUpButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (Time.timeScale == 2)
        {
            gameManager.SetTimeScale(1);
            text.text = ">";
        }
        else
        {
            gameManager.SetTimeScale(2);
            text.text = ">>>";
        }
        
    }
    
    //Menu Behaviour
    public void Play()
    {
        // PlayerPrefs.SetString("username", "");
        // PlayerPrefs.Save();
        // gameManager.StartGame();
        playButton.gameObject.SetActive(false);
        leaderboardButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        usernameText.gameObject.SetActive(false);
        settingsButton.gameObject.SetActive(false);

        if (string.IsNullOrEmpty(PlayerPrefs.GetString("username")))
        {
            DisplayNamePrompt();
        }
        else
        {
            gameManager.StartGame();
            waveText.gameObject.SetActive(true);
            towersOptions.SetActive(true);
            moneyText.gameObject.SetActive(true);
            playerHealthText.gameObject.SetActive(true);
            speedUpButton.gameObject.SetActive(true);
        }
    }

    public void DisplayNamePrompt()
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("username")))
        {
            namePrompt.SetActive(true);


            namePromptContinue.onClick.AddListener(() =>
            {
                if (string.IsNullOrEmpty(nameInput.text) || nameInput.text.Length > 20)
                {
                    namePromptText.text = "Please enter a different name";
                    StartCoroutine(ResetNamePromptText());
                }
                else
                {
                    PlayerPrefs.SetString("username", nameInput.text);
                    namePrompt.SetActive(false);
                    gameManager.StartGame();
                    waveText.gameObject.SetActive(true);
                    towersOptions.SetActive(true);
                    moneyText.gameObject.SetActive(true);
                }
            });
        }
    }

    public IEnumerator ResetNamePromptText()
    {
        yield return new WaitForSeconds(1f);
        namePromptText.text = "Please enter your name";
    }

    public void DisplayYouLost()
    {
        youLostPanel.SetActive(true);
    }

    public void ReturnToMenu()
    {
        youLostPanel.SetActive(false);
        playButton.gameObject.SetActive(true);
        leaderboardButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        usernameText.gameObject.SetActive(true);
        settingsButton.gameObject.SetActive(true);
        
        towersOptions.SetActive(false);
        waveText.gameObject.SetActive(false);
        moneyText.gameObject.SetActive(false);
        leaderboardPanel.SetActive(false);
        playerHealthText.gameObject.SetActive(false);
        speedUpButton.gameObject.SetActive(false);
        settingsPanel.SetActive(false);
    }
    
    public void CreateLeaderboard()
    {
        //Destroy all current children
        for (int i = 0; i < leaderboardContentParent.transform.childCount; i++)
        {
            Destroy(leaderboardContentParent.transform.GetChild(i).gameObject);
        }

        // List<LeaderboardEntry> leaderboardData = StartCoroutine(gameManager.leaderboard.GetLeaderboard());
        StartCoroutine(gameManager.leaderboard.GetLeaderboard((leaderboardData) =>
        {
            for (int i = 0; i < leaderboardData.Count; i++)
            {
                GameObject leaderboardCell = Instantiate(leaderboardCellPrefab, leaderboardContentParent.transform);
                leaderboardCell.GetComponent<LeaderboardCell>().Populate(i + 1, leaderboardData[i]);
            }

            leaderboardPanel.SetActive(true);
        }));
    }

    public void ShowSettings()
    {
        settingsPanel.SetActive(true);
        gameManager.settings.LoadSettings();
    }
    
    public void UpdateHealthText(int n)
    {
        playerHealthText.text = "Health: " + n;
    }
    
    public void ResetSpeedButtonState() => speedUpButton.GetComponentInChildren<TextMeshProUGUI>().text = ">";

    public void Quit()
    {
        Application.Quit();
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateTowerButtons();
        usernameText.text = "Logged in as: " + PlayerPrefs.GetString("username");
    }

    // Update is called once per frame
    void Update()
    {
        if (towerInfoPanel.activeInHierarchy)
        {
            towerInfoPanelDamage.text = tower.damageGiven.ToString();
        }
    }
}