using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using System;

public class ScoreManager : MonoBehaviour
{
    private ScoreData sd;
    private string SavePath => $"{Application.persistentDataPath}/ScoreData.json";

    private Score[] scoresArray;

    private void Start()
    {
        sd = new ScoreData();

        Debug.Log(Application.persistentDataPath);

        LoadScore();
    }

    public void AddScore(Score score)
    {
        sd.scores.Add(score);
    }

    public void SortScore()
    {
        // sd.scores = sd.scores.OrderBy(x => x.second).ToList();

        // foreach (var score in sd.scores)
        // {
        //     Debug.Log(score.date + ", " + score.timeSpent + ", " + score.second);
        // }

        scoresArray = sd.scores.OrderBy(x => x.second).ToArray();

        ShowScore(scoresArray);
    }

    public void ShowScore(Score[] scoresArray)
    {
        var length = scoresArray.Length;
        if (scoresArray.Length > 5) length = 5;
        for (int i = 0; i < length; i++)
        {
            var scoreUi = GameObject.Find("Score" + i.ToString());
            var dateUi = scoreUi.transform.Find("Date").gameObject;
            dateUi.GetComponent<Text>().text = scoresArray[i].date;
            var timeSpent = scoreUi.transform.Find("TimeSpent").gameObject;
            timeSpent.GetComponent<Text>().text = scoresArray[i].timeSpent;
        }
    }

    public void SaveScore()
    {
        using (StreamWriter stream = new StreamWriter(SavePath))
        {
            string json = JsonUtility.ToJson(sd, true);
            stream.Write(json);
        }
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

            if (JsonUtility.FromJson<ScoreData>(json) != null) //ถ้าในไฟล์โล่ง ไม่ต้องเอามาเก็บใน sd (ไม่งั้นจะ sd จะเป็น null)
            {
                sd = JsonUtility.FromJson<ScoreData>(json);
                SortScore();
            }
        }

        foreach (var score in sd.scores)
        {
            Debug.Log(score.date + ", " + score.timeSpent + ", " + score.second);
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
    public float second;

    public Score(string date, string timeSpent, float second)
    {
        this.date = date;
        this.timeSpent = timeSpent;
        this.second = second;
    }
}
