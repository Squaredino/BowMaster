using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quests : MonoBehaviour
{
    private bool hitBullseye1Completed, hitBullseye2Completed, hitBullseye3Completed;
    private bool score1Completed, score2Completed;
    private bool totalScore1Completed, totalScore2Completed;
    private bool playGames1Completed, playGames2Completed, playGames3Completed;
    private bool scoreInHits1Completed;
    private bool hitAfterTimerEnds1Completed;
    private bool playForDays1Completed;
    private bool unlockArrows1Completed, unlockArrows2Completed;
    private bool unlockTargets1Completed, unlockTargets2Completed;

    void Start()
    {
        // load progress
        hitBullseye1Completed = PlayerPrefs.GetInt("hitBullseye1Completed", 0) == 1;
        hitBullseye2Completed = PlayerPrefs.GetInt("hitBullseye2Completed", 0) == 1;
        hitBullseye3Completed = PlayerPrefs.GetInt("hitBullseye3Completed", 0) == 1;

        score1Completed = PlayerPrefs.GetInt("score1Completed", 0) == 1;
        score2Completed = PlayerPrefs.GetInt("score2Completed", 0) == 1;

        totalScore1Completed = PlayerPrefs.GetInt("totalScore1Completed", 0) == 1;
        totalScore2Completed = PlayerPrefs.GetInt("totalScore2Completed", 0) == 1;

        playGames1Completed = PlayerPrefs.GetInt("playGames1Completed", 0) == 1;
        playGames2Completed = PlayerPrefs.GetInt("playGames2Completed", 0) == 1;
        playGames3Completed = PlayerPrefs.GetInt("playGames3Completed", 0) == 1;

        scoreInHits1Completed = PlayerPrefs.GetInt("scoreInHits1Completed", 0) == 1;

        hitAfterTimerEnds1Completed = PlayerPrefs.GetInt("hitAfterTimerEnds1Completed", 0) == 1;

        // sub to events
        GlobalEvents<OnTargetHit>.Happened += HitBullseye1;
        GlobalEvents<OnTargetHit>.Happened += HitBullseye2;
        GlobalEvents<OnTargetHit>.Happened += HitBullseye3;

        GlobalEvents<OnTargetHit>.Happened += Score1;
        GlobalEvents<OnTargetHit>.Happened += Score2;

        GlobalEvents<OnTargetHit>.Happened += TotalScore1;
        GlobalEvents<OnTargetHit>.Happened += TotalScore2;

        GlobalEvents<OnStartGame>.Happened += PlayGames1;
        GlobalEvents<OnStartGame>.Happened += PlayGames2;
        GlobalEvents<OnStartGame>.Happened += PlayGames3;

        GlobalEvents<OnTargetHit>.Happened += ScoreInHits1;

        GlobalEvents<OnTargetHit>.Happened += HitAfterTimerEnds1;

    }

    private void HitBullseye1(OnTargetHit obj)
    {
        if (hitBullseye1Completed) return;
        if (obj.bullseyeStreak >= 3)
        {
            hitBullseye1Completed = true;
            PlayerPrefs.SetInt("hitBullseye1Completed", 1);
            GlobalEvents<OnOpenSkinArrow>.Call(new OnOpenSkinArrow { Id = 4 });
            Debug.Break();
        }
    }

    private void HitBullseye2(OnTargetHit obj)
    {
        if (hitBullseye2Completed) return;
        if (obj.bullseyeStreak >= 10)
        {
            hitBullseye2Completed = true;
            PlayerPrefs.SetInt("hitBullseye2Completed", 1);
            GlobalEvents<OnOpenSkinArrow>.Call(new OnOpenSkinArrow { Id = 7 });
        }
    }

    private void HitBullseye3(OnTargetHit obj)
    {
        if (hitBullseye3Completed) return;
        if (obj.isTargetMoving && obj.bullseyeStreak > 0)
        {
            hitBullseye3Completed = true;
            PlayerPrefs.SetInt("hitBullseye3Completed", 1);
            GlobalEvents<OnOpenSkinTarget>.Call(new OnOpenSkinTarget { Id = 3 });
        }
    }

    private void Score1(OnTargetHit obj)
    {
        if (score1Completed) return;
        if (obj.score >= 100)
        {
            score1Completed = true;
            PlayerPrefs.SetInt("score1Completed", 1);
            GlobalEvents<OnOpenSkinArrow>.Call(new OnOpenSkinArrow { Id = 3 });
        }
    }

    private void Score2(OnTargetHit obj)
    {
        if (score2Completed) return;
        if (obj.score >= 250)
        {
            score2Completed = true;
            PlayerPrefs.SetInt("score2Completed", 1);
            GlobalEvents<OnOpenSkinTarget>.Call(new OnOpenSkinTarget { Id = 5 });
        }
    }

    private void TotalScore1(OnTargetHit obj)
    {
        if (totalScore1Completed) return;
        if (obj.totalScore >= 1000)
        {
            totalScore1Completed = true;
            PlayerPrefs.SetInt("totalScore1Completed", 1);
            GlobalEvents<OnOpenSkinArrow>.Call(new OnOpenSkinArrow { Id = 6 });
        }
    }

    private void TotalScore2(OnTargetHit obj)
    {
        if (totalScore2Completed) return;
        if (obj.totalScore >= 10000)
        {
            totalScore2Completed = true;
            PlayerPrefs.SetInt("totalScore2Completed", 1);
            GlobalEvents<OnOpenSkinTarget>.Call(new OnOpenSkinTarget { Id = 6 });
        }
    }

    private void PlayGames1(OnStartGame obj)
    {
        if (playGames1Completed) return;
        if (obj.totalGames >= 10)
        {
            playGames1Completed = true;
            PlayerPrefs.SetInt("playGames1Completed", 1);
            GlobalEvents<OnOpenSkinArrow>.Call(new OnOpenSkinArrow { Id = 5 });
        }
    }

    private void PlayGames2(OnStartGame obj)
    {
        if (playGames2Completed) return;
        if (obj.totalGames >= 100)
        {
            playGames2Completed = true;
            PlayerPrefs.SetInt("playGames2Completed", 1);
            GlobalEvents<OnOpenSkinTarget>.Call(new OnOpenSkinTarget { Id = 4 });
        }
    }

    private void PlayGames3(OnStartGame obj)
    {
        if (playGames3Completed) return;
        if (obj.totalGames >= 500)
        {
            playGames3Completed = true;
            PlayerPrefs.SetInt("playGames3Completed", 1);
            GlobalEvents<OnOpenSkinTarget>.Call(new OnOpenSkinTarget { Id = 7 });
        }
    }

    private void ScoreInHits1(OnTargetHit obj)
    {
        if (scoreInHits1Completed) return;
        if (obj.score >= 100 && obj.targetHits <= 30)
        {
            scoreInHits1Completed = true;
            PlayerPrefs.SetInt("scoreInHits1Completed", 1);
            GlobalEvents<OnOpenSkinArrow>.Call(new OnOpenSkinArrow { Id = 9 });
        }
    }

    private void HitAfterTimerEnds1(OnTargetHit obj)
    {
        if (hitAfterTimerEnds1Completed) return;
        if (obj.timerLeft <= 0f)
        {
            hitAfterTimerEnds1Completed = true;
            PlayerPrefs.SetInt("hitAfterTimerEnds1Completed", 1);
            GlobalEvents<OnOpenSkinArrow>.Call(new OnOpenSkinArrow { Id = 8 });
        }
    }
}
