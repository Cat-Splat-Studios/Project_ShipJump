using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public UIDelgate ui;

    private int score = 0;
    private float currentMod;
   // private int farthestDistance = 0;
    private int curDistance = 0;
    private int nextDistance;

    private int highscore;
    private int highDistance;
    
    public void SetHighscores(int score, int distance)
    {
        highscore = score;
        highDistance = distance;
    }

    public void ResetScore()
    {
        score = 0;
        nextDistance = 1;
        curDistance = 0;
        currentMod = 1;
      //  farthestDistance = 0;
        UpdateScoreUI();
    }

    public void ScoreUpdate(float distance, float speed, float mod)
    {
        curDistance = (int)distance;
        currentMod = mod;

        if (curDistance >= nextDistance)
        {
            AddToScore(speed, mod);
            nextDistance++;
        }

        UpdateScoreUI();
    }

    public void CheckScore()
    {
        // leader borad stuff might change due to plugin

        if (curDistance > highDistance)
        {
            SignInScript.ReportScore("Highest Kilometer's Traveled", curDistance);
            ui.HighDistance();
            highDistance = curDistance;
        }

        if (score > highscore)
        {
            SignInScript.ReportScore("Highest Score", score);
            ui.Highscore();
            highscore = score;         
        }
    }

    public int GetHighscore(bool isScore = true)
    {
        if (isScore)
            return highscore;
        else
            return highDistance;
    }

    private void AddToScore(float speedPoint, float mod)
    {
        score += (int)(speedPoint * mod);
    }


    private void UpdateScoreUI()
    {
        ui.curScore = score.ToString();
        ui.curDistance = curDistance.ToString();
        ui.SetModText(currentMod);
        ui.UpdateScoreText();
    }
}
