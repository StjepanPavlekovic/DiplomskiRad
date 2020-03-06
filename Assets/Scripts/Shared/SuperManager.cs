using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class SuperManager : MonoBehaviour
{
    public bool shooterComplete = false;
    public bool explorerComplete = false;
    public bool firstChosen = false;
    public Game gameChoosenFirst;
    public bool earlyEnd = false;

    public Guid userId;

    public Dictionary<string, object> parameters;

    public static SuperManager instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
        parameters = new Dictionary<string, object>();


        //UserID Related
        string id = PlayerPrefs.GetString("UserID");

        if(id != null && id != "")
        {
            if(!Guid.TryParse(id, out userId))
            {
                userId = Guid.NewGuid();
                Debug.Log("User assigned new ID by force: " + userId.ToString());
            }
            else
            {
                Debug.Log("UserID: " + userId.ToString());
            }
        }
        else
        {
            userId = Guid.NewGuid();
            try
            {
                PlayerPrefs.SetString("UserID", userId.ToString());
                Debug.Log("User assigned new ID: " + userId.ToString());
            }
            catch
            {
                Debug.Log("Error trying to save user...");
            }
        }

        Analytics.initializeOnStartup = true;
    }

    private void Dispatch(string customEvent)
    {
        parameters.Add("UserID", userId);
        AnalyticsResult res = Analytics.CustomEvent(customEvent, parameters);
        Debug.Log(res);
        Analytics.FlushEvents();
        parameters.Clear();
    }

    public void StartedFirstGame(Game game)
    {
        firstChosen = true;
        gameChoosenFirst = game;

        parameters.Add("Game", gameChoosenFirst);

        Dispatch("First Game Chosen");
    }

    public void StartedSecondGame(Game secondGame)
    {
        parameters.Add("First game ended early", earlyEnd);
        Dispatch("Started second game");
    }

    public void CompleteGame(Game game, float playTime, List<float> times, int deathCount, int interactionsCount, int jumpsCount, float moveTime)
    {
        earlyEnd = false;

        float averageQuestTime = 0;

        if (times.Count > 0)
        {
            for (int i = 0; i < times.Count; i++)
            {
                averageQuestTime += times[i];
            }
            averageQuestTime = averageQuestTime / times.Count;
        }

        parameters.Add("Game", game);
        parameters.Add("Total playtime", playTime);
        parameters.Add("Quests completed", times.Count);
        parameters.Add("Average time per quest", averageQuestTime);
        parameters.Add("Deaths", deathCount);
        parameters.Add("Interactions", interactionsCount);
        parameters.Add("Jumps done", jumpsCount);
        parameters.Add("Time spent moving", moveTime);

        switch (game)
        {
            case Game.Explorer:
                explorerComplete = true;
                Dispatch("Explorer Completed");
                return;

            case Game.Shooter:
                shooterComplete = true;
                Dispatch("Shooter Completed");
                return;
        }
    }

    public void CompleteGameEarly(Game game, float playTime, List<float> times, int deathCount, int interactionsCount, int jumpsCount, float moveTime)
    {
        earlyEnd = true;
        float averageQuestTime = 0;

        if(times.Count > 0)
        {
            for (int i = 0; i < times.Count; i++)
            {
                averageQuestTime += times[i];
            }
            averageQuestTime = averageQuestTime / times.Count;
        }

        parameters.Add("Game", game);
        parameters.Add("Total playtime", playTime);
        parameters.Add("Quests completed", times.Count);
        parameters.Add("Average time per quest", averageQuestTime);
        parameters.Add("Deaths", deathCount);
        parameters.Add("Interactions", interactionsCount);
        parameters.Add("Jumps done", jumpsCount);
        parameters.Add("Time spent moving", moveTime);

        switch (game)
        {
            case Game.Explorer:
                explorerComplete = true;
                Dispatch("Explorer Ended Early");
                return;

            case Game.Shooter:
                shooterComplete = true;
                Dispatch("Shooter Ended Early");
                return;
        }
    }
}
