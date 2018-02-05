using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    void Start()
    {
        GlobalEvents<OnTargetHit>.Happened += HitBullseye1;
        GlobalEvents<OnTargetHit>.Happened += HitBullseye2;
        GlobalEvents<OnTargetHit>.Happened += HitBullseye3;

        GlobalEvents<OnTargetHit>.Happened += Score1;
        GlobalEvents<OnTargetHit>.Happened += Score2;

        //GlobalEvents<OnTargetHit>.Happened += TotalScore1;
        GlobalEvents<OnTargetHit>.Happened += TotalScore2;

        GlobalEvents<OnStartGame>.Happened += PlayGames1;
        GlobalEvents<OnStartGame>.Happened += PlayGames2;
        //GlobalEvents<OnStartGame>.Happened += PlayGames3;

        GlobalEvents<OnTargetHit>.Happened += ScoreInHits1;

        //GlobalEvents<OnTargetHit>.Happened += HitAfterTimerEnds1;

        GlobalEvents<OnLoadGame>.Happened += PlayForDays1;
        GlobalEvents<OnLoadGame>.Happened += PlayForDays2;
        GlobalEvents<OnLoadGame>.Happened += PlayForDays3;
    }

    private void CompleteQuest(Quest quest)
    {
        quest.progress = quest.total;
        if (quest.skinType == SkinType.Arrow)
            GlobalEvents<OnOpenSkinArrow>.Call(new OnOpenSkinArrow { Id = quest.skinId });
        else
            GlobalEvents<OnOpenSkinTarget>.Call(new OnOpenSkinTarget { Id = quest.skinId });
    }

    private void HitBullseye1(OnTargetHit obj)
    {
        var quest = Quest.bullseyeHit1;
        if (quest.IsCompleted) return;
        if (obj.bullseyeStreak >= quest.total)
        {
            CompleteQuest(quest);
        }
    }

    private void HitBullseye2(OnTargetHit obj)
    {
        var quest = Quest.bullseyeHit2;
        if (quest.IsCompleted) return;
        if (obj.bullseyeStreak >= quest.total)
        {
            CompleteQuest(quest);
        }
    }

    private void HitBullseye3(OnTargetHit obj)
    {
        var quest = Quest.bullseyeHit3;
        if (quest.IsCompleted) return;
        if (obj.isTargetMoving && obj.bullseyeStreak > 0)
        {
            CompleteQuest(quest);
        }
    }

    private void Score1(OnTargetHit obj)
    {
        var quest = Quest.score1;
        if (quest.IsCompleted) return;
        if (obj.score >= quest.total)
        {
            CompleteQuest(quest);
        }
    }

    private void Score2(OnTargetHit obj)
    {
        var quest = Quest.score2;
        if (quest.IsCompleted) return;
        if (obj.score >= quest.total)
        {
            CompleteQuest(quest);
        }
    }

    private void TotalScore1(OnTargetHit obj)
    {
        var quest = Quest.totalScore1;
        if (quest.IsCompleted) return;
        if (obj.totalScore >= quest.total)
        {
            CompleteQuest(quest);
        }
    }

    private void TotalScore2(OnTargetHit obj)
    {
        var quest = Quest.totalScore2;
        if (quest.IsCompleted) return;
        if (obj.totalScore >= quest.total)
        {
            CompleteQuest(quest);
        }
    }

    private void PlayGames1(OnStartGame obj)
    {
        var quest = Quest.playGames1;
        if (quest.IsCompleted) return;
        if (obj.totalGames >= quest.total)
        {
            CompleteQuest(quest);
        }
    }

    private void PlayGames2(OnStartGame obj)
    {
        var quest = Quest.playGames2;
        if (quest.IsCompleted) return;
        if (obj.totalGames >= quest.total)
        {
            CompleteQuest(quest);
        }
    }

    private void PlayGames3(OnStartGame obj)
    {
        var quest = Quest.playGames3;
        if (quest.IsCompleted) return;
        if (obj.totalGames >= quest.total)
        {
            CompleteQuest(quest);
        }
    }

    private void ScoreInHits1(OnTargetHit obj)
    {
        var quest = Quest.scoreInHits1;
        if (quest.IsCompleted) return;
        if (obj.score >= 100 && obj.targetHits <= 30)
        {
            CompleteQuest(quest);
        }
    }

    private void HitAfterTimerEnds1(OnTargetHit obj)
    {
        var quest = Quest.hitAfterTimerEnds1;
        if (quest.IsCompleted) return;
        if (obj.timerLeft <= 0f)
        {
            CompleteQuest(quest);
        }
    }

    private void PlayForDays1(OnLoadGame obj)
    {
        var quest = Quest.playForDays1;
        if (quest.IsCompleted) return;
        if (obj.daysInRow >= quest.total)
        {
            CompleteQuest(quest);
        }
    }

    private void PlayForDays2(OnLoadGame obj)
    {
        var quest = Quest.playForDays2;
        if (quest.IsCompleted) return;
        if (obj.daysInRow >= quest.total)
        {
            CompleteQuest(quest);
        }
    }

    private void PlayForDays3(OnLoadGame obj)
    {
        var quest = Quest.playForDays3;
        if (quest.IsCompleted) return;
        if (obj.daysInRow >= quest.total)
        {
            CompleteQuest(quest);
        }
    }
}
