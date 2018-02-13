using UnityEngine;

public class QuestManager : MonoBehaviour
{
    void Awake()
    {
        Quest.Init();

        GlobalEvents<OnGameAwake>.Happened += OnGameAwake;
        GlobalEvents<OnTargetHit>.Happened += OnTargetHit;
        GlobalEvents<OnStartGame>.Happened += OnStartGame;

        GlobalEvents<OnGetQuest>.Happened += OnGetQuest;
    }

    private void OnGetQuest(OnGetQuest obj)
    {
        Quest quest = Quest.GetByID(obj.SkinType, obj.Id);
        if (quest != null)
            GlobalEvents<OnSendQuest>.Call(new OnSendQuest { QuestItem = quest });
    }

    private void CompleteQuest(Quest quest)
    {
        quest.progress = quest.total;
        GlobalEvents<OnQuestCompleted>.Call(new OnQuestCompleted { QuestItem = quest });
        GlobalEvents<OnOpenSkin>.Call(new OnOpenSkin { QuestItem = quest });
    }

    private void OnGameAwake(OnGameAwake obj)
    {
        CheckQuests(obj);
    }

    private void OnTargetHit(OnTargetHit obj)
    {
        CheckQuests(obj);
    }

    private void OnStartGame(OnStartGame obj)
    {
        CheckQuests(obj);
    }

    private void CheckQuests(object obj)
    {
        foreach (var quest in Quest.questList)
        {
            if (quest.IsCompleted) return;
            quest.progress = Mathf.Max(quest.progress, quest.getProgress(obj));
            if (quest.IsCompleted)
            {
                CompleteQuest(quest);
            }
        }
    }
}
