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

    private void AddToScore(float speedPoint, float mod)
    {
        score += (int)(speedPoint * mod);
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
        ui.SetModText(currentMod);
        ui.UpdateScoreText();
    }
}
