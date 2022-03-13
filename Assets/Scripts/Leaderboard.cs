using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.Networking;

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

public class LeaderboardReqWrap
{
    public LeaderboardEntry leaderboardData;
}



public class Leaderboard : MonoBehaviour
{
    public GameManager gameManager;

    public List<LeaderboardEntry> leaderboard = new List<LeaderboardEntry>();

    public int maxEntries = 10;

    public IEnumerator AddEntry(LeaderboardEntry entry)
    {
        // leaderboard.Add(entry);

        LeaderboardReqWrap entryWrap = new LeaderboardReqWrap();
        entryWrap.leaderboardData = entry;

        string bodyData = JsonConvert.SerializeObject(entryWrap);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(bodyData);

        UnityWebRequest request = new UnityWebRequest();
        request.url = "http://localhost:3000/leaderboard/new";
        request.method = "POST";
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        Debug.Log(request.result);
        
        
        //Sort the leadeboard and trim it to maxEntries
        // leaderboard.Sort((a, b) => b.score.CompareTo(a.score));
        // try
        // {
        //     leaderboard.RemoveRange(maxEntries, leaderboard.Count - maxEntries);
        // } catch
        // {
        //
        // }
        // Save();
    }

    public IEnumerator GetLeaderboard(System.Action<List<LeaderboardEntry>> callback)
    {
        //Get the leaderboard from the server
        
        UnityWebRequest request = new UnityWebRequest();
        request.url = "http://localhost:3000/leaderboard/all";
        request.method = "GET";
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        
        yield return request.SendWebRequest();
        string leaderboardData = request.downloadHandler.text;
        leaderboard = JsonConvert.DeserializeObject<List<LeaderboardEntry>>(request.downloadHandler.text);
        
        leaderboard.Sort((a, b) => b.score.CompareTo(a.score));
        try
        {
            leaderboard.RemoveRange(maxEntries, leaderboard.Count - maxEntries);
        } catch
        {

        }

        callback?.Invoke(leaderboard);
        
        // return leaderboard;
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
        // Load();
    }

    // Update is called once per frame
    void Update()
    {
    }
}