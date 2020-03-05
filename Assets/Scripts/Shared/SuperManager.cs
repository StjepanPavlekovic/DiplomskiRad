using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperManager : MonoBehaviour
{
    public bool shooterComplete = false;
    public bool explorerComplete = false;
    public float playTimeShooter;
    public float playTimeExplorer;
    public float moveTimeShooter;
    public float moveTimeExplorer;
    public List<float> timesPerQuestExplorer;
    public List<float> timesPerQuestShooter;
    public int deathCountExplorer = 0;
    public int deathCountShooter = 0;
    public int interactionsCountExplorer = 0;
    public int interactionsCountShooter = 0;
    public int jumpsCountExplorer = 0;
    public int jumpsCountShooter = 0;
    public bool firstChosen = false;
    public Game gameChoosenFirst;

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
    }

    void Start()
    {
        timesPerQuestExplorer = new List<float>();
        timesPerQuestShooter = new List<float>();
    }

    public void StartedFirstGame(Game game)
    {
        firstChosen = true;
        gameChoosenFirst = game;
        Debug.Log("First game: " + game);
    }

    public void CompleteGame(Game game, float playTime, List<float> times, int deathCount, int interactionsCount, int jumpsCount, float moveTime)
    {
        switch (game)
        {
            case Game.Explorer:
                explorerComplete = true;
                playTimeExplorer = playTime;
                timesPerQuestExplorer = times;
                deathCountExplorer = deathCount;
                interactionsCountExplorer = interactionsCount;
                jumpsCountExplorer = jumpsCount;
                moveTimeExplorer = moveTime;
                return;

            case Game.Shooter:
                shooterComplete = true;
                playTimeShooter = playTime;
                timesPerQuestExplorer = times;
                deathCountShooter = deathCount;
                interactionsCountShooter = interactionsCount;
                jumpsCountExplorer = jumpsCount;
                moveTimeShooter = moveTime;
                return;
        }
    }
}
