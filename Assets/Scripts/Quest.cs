using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SkinType
{
    Target,
    Arrow
}

public class Quest
{
    public int id;
    public string description;
    public int skinId;
    public SkinType skinType;
    public Func<object, int> getProgress;
    public int total;
    public int progress
    {
        get { return PlayerPrefs.GetInt("quest" + id, 0); }
        set { PlayerPrefs.SetInt("quest" + id, value); }
    }
    public bool IsCompleted //todo: non-property
    {
        get { return progress >= total; }
    }

    public static readonly List<Quest> questList = new List<Quest>();

    public static void Init()
    {
        questList.Add(new Quest
        {
            id = 0,
            description = "Make a perfect hit 3 times in a row",
            total = 3,
            skinType = SkinType.Arrow,
            skinId = 3,
            getProgress = (o) => { return o is OnTargetHit ? ((OnTargetHit)o).bullseyeStreak : -1; }
        });

        questList.Add(new Quest
        {
            id = 1,
            description = "Make a perfect hit 10 times in a row",
            total = 10,
            skinType = SkinType.Arrow,
            skinId = 4,
            getProgress = (o) => { return o is OnTargetHit ? ((OnTargetHit)o).bullseyeStreak : -1; }
        });

        questList.Add(new Quest
        {
            id = 2,
            description = "Make a perfect hit of a moving target",
            total = 1,
            skinType = SkinType.Arrow,
            skinId = 5,
            getProgress = (o) => { return o is OnTargetHit ? ((OnTargetHit)o).isTargetMoving && ((OnTargetHit)o).bullseyeStreak > 0 ? 1 : 0 : -1; }
        });

        questList.Add(new Quest
        {
            id = 3,
            description = "Score 100 points in a single game",
            total = 100,
            skinType = SkinType.Arrow,
            skinId = 6,
            getProgress = (o) => { return o is OnTargetHit ? ((OnTargetHit)o).score : -1; }
        });

        questList.Add(new Quest
        {
            id = 4,
            description = "Score 250 points in a single game",
            total = 250,
            skinType = SkinType.Arrow,
            skinId = 7,
            getProgress = (o) => { return o is OnTargetHit ? ((OnTargetHit)o).score : -1; }
        });

        //questList.Add(new Quest
        //{
        //    id = 5,
        //    description = "Score a total of 1000 points",
        //    total = 1000,
        //    skinType = SkinType.Arrow,
        //    skinId = 6,
        //    getProgress = (o) => { return o is OnTargetHit ? ((OnTargetHit)o).totalScore : -1; }
        //});

        questList.Add(new Quest
        {
            id = 6,
            description = "Score a total of 10000 points",
            total = 10000,
            skinType = SkinType.Arrow,
            skinId = 8,
            getProgress = (o) => { return o is OnTargetHit ? ((OnTargetHit)o).totalScore : -1; }
        });

        questList.Add(new Quest
        {
            id = 7,
            description = "Play the game 20 times",
            total = 20,
            skinType = SkinType.Arrow,
            skinId = 9,
            getProgress = (o) => { return o is OnStartGame ? ((OnStartGame)o).totalGames : -1; }
        });

        questList.Add(new Quest
        {
            id = 8,
            description = "Play the game 100 times",
            total = 100,
            skinType = SkinType.Target,
            skinId = 3,
            getProgress = (o) => { return o is OnStartGame ? ((OnStartGame)o).totalGames : -1; }
        });

        //questList.Add(new Quest
        //{
        //    id = 9,
        //    description = "Play the game 500 times",
        //    total = 500,
        //    skinType = SkinType.Target,
        //    skinId = 7,
        //    getProgress = (o) => { return o is OnStartGame ? ((OnStartGame)o).totalGames : -1; }
        //});

        questList.Add(new Quest
        {
            id = 10,
            description = "Score 100 points with 30 or less target hits",
            total = 1,
            skinType = SkinType.Target,
            skinId = 4,
            getProgress = (o) => { return o is OnTargetHit ? ((OnTargetHit)o).score >= 100 && ((OnTargetHit)o).targetHits <= 30 ? 1 : 0 : -1; }
        });

        //questList.Add(new Quest
        //{
        //    id = 11,
        //    description = "Hit the target after the timer has ended",
        //    total = 1,
        //    skinType = SkinType.Arrow,
        //    skinId = 8,
        //    getProgress = (o) => { return o is OnTargetHit ? ((OnTargetHit)o).timerLeft <= 0f ? 1 : 0 : -1; }
        //});

        questList.Add(new Quest
        {
            id = 12,
            description = "Play the game for 2 days in a row",
            total = 2,
            skinType = SkinType.Target,
            skinId = 5,
            getProgress = (o) => { return o is OnGameAwake ? ((OnGameAwake)o).daysInRow : -1; }
        });

        questList.Add(new Quest
        {
            id = 13,
            description = "Play the game for 3 days in a row",
            total = 3,
            skinType = SkinType.Target,
            skinId = 6,
            getProgress = (o) => { return o is OnGameAwake ? ((OnGameAwake)o).daysInRow : -1; }
        });

        questList.Add(new Quest
        {
            id = 14,
            description = "Play the game for 7 days in a row",
            total = 7,
            skinType = SkinType.Target,
            skinId = 7,
            getProgress = (o) => { return o is OnGameAwake ? ((OnGameAwake)o).daysInRow : -1; }
        });
    }

    public static Quest GetByID(SkinType skinType, int skinId)
    {
        return questList.FirstOrDefault(x => x.skinType == skinType && x.skinId == skinId);
    }
}
