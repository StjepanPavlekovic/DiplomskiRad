using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    //EXPLORER RELATED
    [SerializeField]
    private float initialHintTimer;
    private float currentHintTimer;
    public Canvas hintScreen;
    public Canvas endScreen;
    public Text hintTitle;
    public Text hintContent;
    private bool finalFade = false;
    public GameObject newQuestIndicator;
    public AudioSource ambience;

    //SHARED FIELDS
    public Canvas hudScreen;
    public Canvas deathScreen;
    public Canvas pauseScreen;
    public Slider mouseSensitivity;
    public Toggle useAudio;
    public Button exitButton;
    public GameObject croshair;
    public GameObject croshairInteractable;
    private bool fading = false;
    public Canvas introScreen;
    private bool gameCleared = false;

    public MouseLookScript mouseController;

    //SHOOTER RELATED
    public Text currentAmmo;
    public Text ammoLeft;
    public Text health;
    public Text deathText;
    private string initialDeathText;


    [SerializeField]
    private float fadeTime = 0.5f;

    private void Awake()
    {
        instance = this;

        currentHintTimer = initialHintTimer;
    }

    private void Start()
    {
        if (GameManager.instance.game == Game.Shooter)
        {
            initialDeathText = deathText.text;
            if (WeaponManager.instance.currentWeapon != null)
            {
                ammoLeft.text = WeaponManager.instance.currentWeapon.ammo.ToString();
                currentAmmo.text = WeaponManager.instance.currentWeapon.ammoLeft.ToString();
                health.text = 100 + "";
            }
        }

        GameManager.instance.gamePaused = true;
    }

    public void StartGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        introScreen.gameObject.SetActive(false);
        GameManager.instance.gamePaused = false;
        hudScreen.gameObject.SetActive(true);
    }

    public void UpdateAmmo()
    {
        if (WeaponManager.instance.currentWeapon != null)
        {
            ammoLeft.text = WeaponManager.instance.currentWeapon.ammo.ToString();
            currentAmmo.text = WeaponManager.instance.currentWeapon.ammoLeft.ToString();
        }
        else
        {
            ammoLeft.text = 0 + "";
            currentAmmo.text = 0 + "";
        }
    }

    public void UpdateHealth(float healthLeft)
    {
        health.text = healthLeft + "";
    }

    private void Update()
    {
        if (!GameManager.instance.isDead)
        {
            if (GameManager.instance.game == Game.Explorer && !GameManager.instance.gamePaused)
            {
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    if (newQuestIndicator.activeInHierarchy)
                    {
                        newQuestIndicator.GetComponentInChildren<Animator>().enabled = false;
                        newQuestIndicator.SetActive(false);
                    }
                    ToggleHint();
                }
            }
            else if (GameManager.instance.game == Game.Shooter && GameManager.instance.currentQuest == CurrentQuest.AvoidScouts)
            {
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    foreach (var item in GameManager.instance.player.keyItems)
                    {
                        if (item.type == KeyItemType.Note)
                        {
                            if (newQuestIndicator.activeInHierarchy)
                            {
                                AudioManager.instance.PlaySound(AudioClips.NewQuest);
                                newQuestIndicator.GetComponentInChildren<Animator>().enabled = false;
                                newQuestIndicator.SetActive(false);
                            }
                            ToggleHint();
                        }
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.O) && !gameCleared)
            {
                if (!GameManager.instance.gamePaused)
                {
                    PauseGame();
                }
                else
                {
                    ResumeGame();
                }
            }
        }
        else
        {
            if (!deathScreen.gameObject.activeInHierarchy)
            {
                hudScreen.gameObject.SetActive(false);
                if (GameManager.instance.game == Game.Explorer)
                {
                    hintScreen.gameObject.SetActive(false);
                    deathScreen.gameObject.SetActive(true);
                    deathScreen.GetComponent<CanvasGroup>().alpha = 0;
                    StartCoroutine(ScreenFader(deathScreen.GetComponent<CanvasGroup>(), deathScreen.GetComponent<CanvasGroup>().alpha, 1, fadeTime));
                }
                else
                {
                    if (GameManager.instance.deathNote != null)
                    {
                        deathText.text = GameManager.instance.deathNote;
                    }
                    else
                    {
                        deathText.text = initialDeathText;
                    }

                    GameManager.instance.gamePaused = true;
                    deathScreen.gameObject.SetActive(true);
                    deathScreen.GetComponent<CanvasGroup>().alpha = 0;
                    StartCoroutine(ScreenFader(deathScreen.GetComponent<CanvasGroup>(), deathScreen.GetComponent<CanvasGroup>().alpha, 1, fadeTime));
                }
            }
        }
    }

    public void UpdateMouseSensitivity()
    {
        mouseController.rotationSpeed = mouseSensitivity.value;
    }

    public void ButtonRespawnClick()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        GameManager.instance.RespawnPlayer();
        deathScreen.GetComponent<CanvasGroup>().alpha = 0;
        deathScreen.gameObject.SetActive(false);
        hudScreen.gameObject.SetActive(true);
        if (GameManager.instance.game == Game.Shooter)
        {
            GameManager.instance.gamePaused = false;
        }
    }

    public void ToggleHint()
    {
        if (!hintScreen.gameObject.activeInHierarchy)
        {
            if (GameManager.instance.game == Game.Shooter)
            {
                GameManager.instance.gamePaused = true;
            }
            hintScreen.gameObject.SetActive(true);
        }
        else
        {
            if (GameManager.instance.game == Game.Shooter)
            {
                GameManager.instance.gamePaused = false;
            }
            hintScreen.gameObject.SetActive(false);
        }
    }

    private IEnumerator ScreenFader(CanvasGroup group, float start, float end, float time)
    {
        while (fading)
        {
            yield return new WaitForEndOfFrame();
        }

        fading = true;

        float startedAt = Time.time;
        float elapsedTime = Time.time - startedAt;
        float percentageFaded = elapsedTime / time;

        while (true)
        {
            elapsedTime = Time.time - startedAt;
            percentageFaded = elapsedTime / time;

            float current = Mathf.Lerp(start, end, percentageFaded);
            group.alpha = current;

            if (percentageFaded >= 1) break;
            yield return new WaitForEndOfFrame();
        }

        if (finalFade)
        {
            exitButton.gameObject.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            finalFade = false;
        }

        fading = false;
    }

    public void HandleCroshairs(bool looking)
    {
        if (looking)
        {
            if (croshair.gameObject.activeSelf)
            {
                croshair.gameObject.SetActive(false);
                croshairInteractable.gameObject.SetActive(true);
            }
        }
        else
        {
            if (!croshair.gameObject.activeSelf)
            {
                croshair.gameObject.SetActive(true);
                croshairInteractable.gameObject.SetActive(false);
            }
        }
    }

    public void EndTheGame()
    {
        endScreen.gameObject.SetActive(true);
        finalFade = true;
        StartCoroutine(ScreenFader(endScreen.GetComponent<CanvasGroup>(), 0, 1, 4.5f));
    }

    public void HideScreens()
    {
        gameCleared = true;
        hudScreen.gameObject.SetActive(false);
        if (hintScreen.gameObject.activeInHierarchy)
        {
            hintScreen.gameObject.SetActive(false);
        }
    }

    public void ResumeGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseScreen.gameObject.SetActive(false);
        hudScreen.gameObject.SetActive(true);
        GameManager.instance.gamePaused = false;
    }

    public void PauseGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        pauseScreen.gameObject.SetActive(true);
        hudScreen.gameObject.SetActive(false);
        if (GameManager.instance.game == Game.Explorer)
        {
            if (hintScreen.gameObject.activeInHierarchy)
            {
                hintScreen.gameObject.SetActive(false);
            }
        }
        GameManager.instance.gamePaused = true;
    }

    public void ToggleAudio()
    {
        AudioManager.instance.useAudio = useAudio.isOn;
        if (GameManager.instance.game == Game.Explorer)
        {
            ambience.enabled = useAudio.isOn;
        }
    }

    public void ExitGame()
    {
        GameManager.instance.EndGame();
    }

    public void UpdateHint(Hint hint = null)
    {
        if (!newQuestIndicator.activeInHierarchy)
        {
            if (hintScreen.gameObject.activeInHierarchy && GameManager.instance.game == Game.Explorer)
            {
                ToggleHint();
            }
            newQuestIndicator.SetActive(true);
            newQuestIndicator.GetComponentInChildren<Animator>().enabled = true;
        }
        if (GameManager.instance.game == Game.Explorer)
        {
            AudioManager.instance.PlaySound(AudioClips.NewQuest);
            hintTitle.text = hint.title;
            hintContent.text = hint.content;
        }
    }
}
