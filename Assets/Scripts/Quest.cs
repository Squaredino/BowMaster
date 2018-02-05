using System.Collections;
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
    public int total;
    public int progress
    {
        get { return PlayerPrefs.GetInt("quest" + id, 0); }
        set { PlayerPrefs.SetInt("quest" + id, value); }
    }
    public bool IsCompleted
    {
        get { return progress >= total; }
    }

    public static Quest bullseyeHit1 = new Quest
    {
        id = 0,
        description = "Hit the bullseye 3 times in a row",
        total = 3,
        skinType = SkinType.Arrow,
        skinId = 4
    };

    public static Quest bullseyeHit2 = new Quest
    {
        id = 1,
        description = "Hit the bullseye 10 times in a row",
        total = 10,
        skinType = SkinType.Arrow,
        skinId = 7
    };

    public static Quest bullseyeHit3 = new Quest
    {
        id = 2,
        description = "Hit the bullseye of a moving target",
        total = 1,
        skinType = SkinType.Target,
        skinId = 3
    };

    public static Quest score1 = new Quest
    {
        id = 3,
        description = "Score 100 points in a single game",
        total = 100,
        skinType = SkinType.Arrow,
        skinId = 3
    };

    public static Quest score2 = new Quest
    {
        id = 4,
        description = "Score 250 points in a single game",
        total = 250,
        skinType = SkinType.Target,
        skinId = 5
    };

    public static Quest totalScore1 = new Quest
    {
        id = 5,
        description = "Score a total of 1000 points",
        total = 1000,
        skinType = SkinType.Arrow,
        skinId = 6
    };

    public static Quest totalScore2 = new Quest
    {
        id = 6,
        description = "Score a total of 10000 points",
        total = 10000,
        skinType = SkinType.Target,
        skinId = 6
    };

    public static Quest playGames1 = new Quest
    {
        id = 7,
        description = "Play the game 10 times",
        total = 10,
        skinType = SkinType.Arrow,
        skinId = 5
    };

    public static Quest playGames2 = new Quest
    {
        id = 8,
        description = "Play the game 100 times",
        total = 100,
        skinType = SkinType.Target,
        skinId = 4
    };

    public static Quest playGames3 = new Quest
    {
        id = 9,
        description = "Play the game 500 times",
        total = 500,
        skinType = SkinType.Target,
        skinId = 7
    };

    public static Quest scoreInHits1 = new Quest
    {
        id = 10,
        description = "Score 100 points with 30 or less target hits",
        total = 1,
        skinType = SkinType.Arrow,
        skinId = 9
    };

    public static Quest hitAfterTimerEnds1 = new Quest
    {
        id = 11,
        description = "Hit the target after the timer has ended",
        total = 1,
        skinType = SkinType.Arrow,
        skinId = 8
    };

    public static Quest playForDays1 = new Quest
    {
        id = 12,
        description = "Play the game for 2 days in a row",
        total = 2,
        skinType = SkinType.Target,
        skinId = 7
    };

    public static Quest playForDays2 = new Quest
    {
        id = 13,
        description = "Play the game for 3 days in a row",
        total = 3,
        skinType = SkinType.Arrow,
        skinId = 6
    };

    public static Quest playForDays3 = new Quest
    {
        id = 14,
        description = "Play the game for 7 days in a row",
        total = 7,
        skinType = SkinType.Arrow,
        skinId = 8
    };

    private static List<Quest> list = new List<Quest>
    {
        bullseyeHit1,
        bullseyeHit2,
        bullseyeHit3,
        score1,
        score2,
        //totalScore1,
        totalScore2,
        playGames1,
        playGames2,
        //playGames3,
        scoreInHits1,
        //hitAfterTimerEnds1,
        playForDays1,
        playForDays2,
        playForDays3
    };

    public static Quest GetBySkin(SkinType skinType, int skinId)
    {
        return list.FirstOrDefault(x => x.skinType == skinType && x.skinId == skinId);
    }
}
