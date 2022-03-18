using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ScoreManager : MonoBehaviour
{
    private ScoreData sd;

    // private ScoreData scoreData = new ScoreData();

    private string SavePath => $"{Application.persistentDataPath}/ScoreData.json";

    private void Start()
    {
        sd = new ScoreData();

        Debug.Log(Application.persistentDataPath);

        LoadScore();
        // sd.scores.Add(new Score("x", "9"));
        // sd.scores.Add(new Score("y", "8"));
        // sd.scores.Add(new Score("z", "7"));
        SaveScore();
    }

    private void LoadScore()
    {
        if (!File.Exists(SavePath))
        {
            File.Create(SavePath).Dispose();
            return;
        }

        using (StreamReader stream = new StreamReader(SavePath))
        {
            string json = stream.ReadToEnd();
            sd = JsonUtility.FromJson<ScoreData>(json); 
        }

        foreach (var score in sd.scores)
        {
            Debug.Log(score.date + ", " + score.timeSpent);
        }
    }

    private void SaveScore()
    {
        using (StreamWriter stream = new StreamWriter(SavePath))
        {
            string json = JsonUtility.ToJson(sd, true);
            stream.Write(json);
        }
    }
}

[Serializable]
public class ScoreData
{
    public List<Score> scores;

    public ScoreData()
    {
        scores = new List<Score>();
    }
}

[Serializable]
public class Score
{
    public string date;
    public string timeSpent;

    public Score(string date, string timeSpent)
    {
        this.date = date;
        this.timeSpent = timeSpent;
    }
}
