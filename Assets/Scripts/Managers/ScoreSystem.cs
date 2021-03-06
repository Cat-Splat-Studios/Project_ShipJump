﻿/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle the scoring of player based on speed and distance
**/

using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public UIDelgate ui;
    
    // Total score for each round
    private int score = 0;

    // Distance tracking for score
    private int curDistance = 0;
    private int nextDistance;

    // Highscores stored
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
        UpdateScoreUI();
    }

    public void ScoreUpdate(float distance, float speed)
    {
        // Set distance and modifier
        curDistance = (int)distance;

        // Check if you gone 1km, add to score
        if (curDistance >= nextDistance)
        {
            AddToScore(speed);
            nextDistance++;
        }

        UpdateScoreUI();
    }

    public void CheckScore()
    {
        // Check both distance and score for highest and update leaderboards

        if (curDistance > highDistance)
        {
            //GameService.ReportScore("Highest Kilometer's Traveled", curDistance);
            //ui.HighDistance();
            highDistance = curDistance;
        }

        if (score > highscore)
        {
            GameService.ReportScore("Highest Score", score);
            ui.Highscore(); 
            highscore = score;         
        }

        ui.UpdateHighscoreText();
    }

    public void ForwardAgain(float distance)
    {
        curDistance = (int)distance;
        nextDistance = curDistance + 1;
    }

    public int GetHighscore(bool isScore = true)
    {
        if (isScore)
            return highscore;
        else
            return highDistance;
    }

    private void AddToScore(float speedPoint)
    {
        // Add score based on current speed
        score += (int)(speedPoint);
    }


    private void UpdateScoreUI()
    {
        // Update all score information on UI
        ui.curScore = score.ToString();
        ui.curDistance = curDistance.ToString();
        ui.UpdateScoreText();
    }
}
