using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurrentQuest
{
    //EXPLORER QUESTS
    FindTorch, BurnTorch, ExploreTomb, PlaceTorch, FindTheWayOut, SolveCrystals, TakeGem, EscapeTomb,

    ____________,

    //SHOOTER QUESTS
    DisableSecurity, AvoidScouts, FindTheDrive, EscapeTheBase
}

public enum Game
{
    Explorer, Shooter
}

public class Hint
{
    public string title;
    public string content;

    public Hint(string title, string content)
    {
        this.title = title;
        this.content = content;
    }
}

public class GameManager : MonoBehaviour
{
    public Game game;
    public List<CollectibleItem> questItems;
    public static GameManager instance;
    public CurrentQuest currentQuest;

    public List<RespawnPoint> respawnPoints;
    public RespawnPoint currentRespawn;

    public Dictionary<CurrentQuest, Hint> hints;

    public bool gamePaused = false;
    public bool isDead = false;

    public PlayerController player;

    public List<EnemyAI> enemies;
    public List<EnemySpawner> enemySpawners;
    public List<GameObject> cleanUpOnReset;

    private void Awake()
    {
        instance = this;

        hints = new Dictionary<CurrentQuest, Hint>();

        hints.Add(CurrentQuest.FindTorch, new Hint("Illuminate the tomb", "The tomb is too dark to explore... Maybe I can find something to light up using the bonfire if I look around..."));
        hints.Add(CurrentQuest.BurnTorch, new Hint("Start the fire", "I might be able to light up this old torch I found... Let's try the bonfire."));
        hints.Add(CurrentQuest.ExploreTomb, new Hint("Explore the tomb", "Now that I have some light, I may be able to look around the tomb and try to find the treasure."));
        hints.Add(CurrentQuest.FindTheWayOut, new Hint("Find the way out", "Both paths appear to be a dead end. I feel a draft coming through the left hall... Maybe there's a hidden passage. I did hear some mechanism activate."));
        hints.Add(CurrentQuest.SolveCrystals, new Hint("Solve the puzzle", "The room ahead contains a puzzle... I should look closely, I might figure out a hint to solving it."));
        hints.Add(CurrentQuest.TakeGem, new Hint("Get the treasure", "I have finally found the treasure. Now I need to take it and find the way out..."));
        hints.Add(CurrentQuest.EscapeTomb, new Hint("Escape the tomb", "This place could fall apart any second now, I need to get out now! (HOLD SHIFT TO RUN!)"));

        respawnPoints = new List<RespawnPoint>();

        if (game == Game.Shooter)
        {
            enemies = new List<EnemyAI>();
            enemySpawners = new List<EnemySpawner>();
        }
    }

    private void Start()
    {
        if (game == Game.Explorer)
        {
            UIManager.instance.UpdateHint(hints[CurrentQuest.FindTorch]);
        }
    }

    public string deathNote;
    public void KillPlayer(GameObject resetableObject = null, string deathMessage = null)
    {
        if (deathMessage != null)
        {
            deathNote = deathMessage;
            AudioManager.instance.PlaySound(AudioClips.Alert);
        }
        else
        {
            deathNote = null;
        }
        isDead = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (resetableObject != null)
        {
            resetableObject.SetActive(false);
        }
    }

    public void RespawnPlayer()
    {
        player.transform.position = currentRespawn.gameObject.transform.position;
        player.transform.rotation = currentRespawn.gameObject.transform.rotation;
        player.died = false;
        isDead = false;
        player.rb.velocity = Vector3.zero;
        if (game == Game.Shooter)
        {
            AudioManager.instance.StopSounds();
            ResetEnemies();
            player.ResetPlayer();
        }
    }

    public bool AlreadyCollected(KeyItemType type)
    {
        for (int i = 0; i < player.keyItems.Count; i++)
        {
            if (player.keyItems[i].type == type)
            {
                return true;
            }
        }
        return false;
    }

    private void ResetEnemies()
    {
        for (int i = 0; i < cleanUpOnReset.Count; i++)
        {
            if (cleanUpOnReset[i] != null)
            {
                Destroy(cleanUpOnReset[i].gameObject);
            }
        }
        foreach (var item in enemySpawners)
        {
            if (item.spawnerForRespawn == currentRespawn)
            {
                item.SpawnEnemy();
            }
        }
    }

    public void UpdateRespawnPoint()
    {
        for (int i = 0; i < respawnPoints.Count; i++)
        {
            if (respawnPoints[i].respawnForQuest == currentQuest) currentRespawn = respawnPoints[i];
        }
    }

    public void NextQuest()
    {
        switch (currentQuest)
        {
            //ExplorerQuestCheck
            case CurrentQuest.FindTorch:
                currentQuest = CurrentQuest.BurnTorch;
                break;
            case CurrentQuest.BurnTorch:
                currentQuest = CurrentQuest.ExploreTomb;
                break;
            case CurrentQuest.ExploreTomb:
                currentQuest = CurrentQuest.FindTheWayOut;
                break;
            case CurrentQuest.PlaceTorch:
                currentQuest = CurrentQuest.SolveCrystals;
                break;
            case CurrentQuest.FindTheWayOut:
                currentQuest = CurrentQuest.SolveCrystals;
                break;
            case CurrentQuest.SolveCrystals:
                currentQuest = CurrentQuest.TakeGem;
                break;
            case CurrentQuest.TakeGem:
                currentQuest = CurrentQuest.EscapeTomb;
                break;

            //ShooterQuestCheck
            case CurrentQuest.DisableSecurity:
                currentQuest = CurrentQuest.AvoidScouts;
                break;
            case CurrentQuest.AvoidScouts:
                currentQuest = CurrentQuest.FindTheDrive;
                break;
            case CurrentQuest.FindTheDrive:
                currentQuest = CurrentQuest.EscapeTheBase;
                break;
        }

        UpdateRespawnPoint();

        if (game == Game.Explorer)
        {
            for (int i = 0; i < questItems.Count; i++)
            {
                if (questItems[i].questItem == currentQuest)
                {
                    questItems[i].interactable = true;
                }
                else
                {
                    questItems[i].interactable = false;
                }
            }
            UIManager.instance.UpdateHint(hints[currentQuest]);
        }
    }
}
