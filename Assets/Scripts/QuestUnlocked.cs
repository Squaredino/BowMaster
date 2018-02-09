using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUnlocked : MonoBehaviour
{
    private Queue<Quest> unlockedQuests;

    void Awake()
    {
        unlockedQuests = new Queue<Quest>();

        GlobalEvents<OnQuestCompleted>.Happened += OnQuestCompleted;
        GlobalEvents<OnGameLoaded>.Happened += OnGameLoaded;
        GlobalEvents<OnGameOver>.Happened += OnGameOver;
    }

    private void OnQuestCompleted(OnQuestCompleted e)
    {
        unlockedQuests.Enqueue(e.QuestItem);
    }

    private void OnGameLoaded(OnGameLoaded e)
    {
        ShowUnlockedDialog();
    }

    private void OnGameOver(OnGameOver e)
    {
        ShowUnlockedDialog();
    }

    private void ShowUnlockedDialog()
    {
        if (unlockedQuests.Count > 0)
        {
            var quest = unlockedQuests.Dequeue();
            GlobalEvents<OnQuestShowUnlockedDialog>.Call(new OnQuestShowUnlockedDialog { QuestItem = quest });
        }
    }
}
