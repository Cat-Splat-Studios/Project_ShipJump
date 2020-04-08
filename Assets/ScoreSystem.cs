using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public UIDelgate ui;

    private int score = 0;
   // private int farthestDistance = 0;
    private int curDistance = 0;
    private int nextDistance;

    public void ResetScore()
    {
        score = 0;
        nextDistance = 1;
        curDistance = 0;
      //  farthestDistance = 0;
        UpdateScoreUI();
    }

    public void ScoreUpdate(float distance, float speed)
    {
        curDistance = (int)distance;

        if (curDistance >= nextDistance)
        {
            AddToScore(speed);
            nextDistance++;
        }

        UpdateScoreUI();
    }


    private void AddToScore(float speedPoint)
    {
        score += (int)speedPoint;
    }

    public void CheckScore()
    {
        // leader borad stuff might change due to plugin
        int highscore = SaveManager.instance.GetHighscore();
        int highscoreStat = SaveManager.instance.GetHighscoreStat();
        if (curDistance > highscore)
        {
            GPGSUtils.instance.SubmitScore(curDistance);
            SaveManager.instance.SetHighScore(curDistance);
        }

        if (score > highscoreStat)
        {
            ui.Highscore(highscoreStat);
            GPGSUtils.instance.SubmitScoreStat(score);
            SaveManager.instance.SetHighScoreStat(score);
        }
    }


    private void UpdateScoreUI()
    {
        ui.curScore = score.ToString();
        ui.curDistance = curDistance.ToString();
        ui.UpdateScoreText();
    }
}
