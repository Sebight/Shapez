using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

[System.Serializable]
public class LeaderboardEntry
{
    public string username;
    public int score;

    public LeaderboardEntry(string username, int score)
    {
        this.username = username;
        this.score = score;
    }
}

public class Leaderboard : MonoBehaviour
{

    public GameManager gameManager;

    public List<LeaderboardEntry> leaderboard = new List<LeaderboardEntry>();

    public int maxEntries = 10;

    public void AddEntry(LeaderboardEntry entry)
    {
        leaderboard.Add(entry);

        //Sort the leadeboard and trim it to maxEntries
        leaderboard.Sort((a, b) => b.score.CompareTo(a.score));
        try
        {
            leaderboard.RemoveRange(maxEntries, leaderboard.Count - maxEntries);
        } catch
        {

        }
        Save();
    }

    public List<LeaderboardEntry> GetLeaderboard()
    {
        leaderboard.Sort((a, b) => b.score.CompareTo(a.score));
        return leaderboard;
    }

    public void Save()
    {
        //Save the leaderboard to a file
        string json = JsonConvert.SerializeObject(leaderboard);
        File.WriteAllText(Application.persistentDataPath + "/leaderboard.json", json);
    }

    public void Load()
    {
        //Load the leaderboard from a file
        string json = File.ReadAllText(Application.persistentDataPath + "/leaderboard.json");
        leaderboard = JsonConvert.DeserializeObject<List<LeaderboardEntry>>(json);
    }


    // Start is called before the first frame update
    void Start()
    {
        Load();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
