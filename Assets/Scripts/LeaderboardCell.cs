using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LeaderboardCell : MonoBehaviour
{

    public TextMeshProUGUI usernameText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI positionText;

    public void Populate(int pos, LeaderboardEntry data)
    {
        usernameText.text = data.username;
        scoreText.text = data.score.ToString();
        positionText.text = pos.ToString();
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
