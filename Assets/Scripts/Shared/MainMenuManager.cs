using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void OpenSurvey(string url);

    [SerializeField]
    private Canvas tos;
    public Canvas warning;
    private static bool acceptedTos = false;
    [SerializeField]
    private Canvas menu;

    [SerializeField]
    private GameObject continueButton;
    public GameObject warningContinueButton;

    private bool fading = false;

    public static MainMenuManager instance;

    public GameObject explorerGameButton;
    public GameObject shooterGameButton;
    public GameObject survey;
    public GameObject initialChoice;
    public GameObject loading;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        if (!acceptedTos)
        {
            if (warning.gameObject.activeInHierarchy)
            {
                warning.gameObject.SetActive(false);
            }
            tos.gameObject.SetActive(true);
            StartCoroutine(MenuScreenFader(tos.GetComponent<CanvasGroup>(), 0, 1, 1));
        }
        else
        {
            if (tos.gameObject.activeInHierarchy)
            {
                tos.gameObject.SetActive(false);
            }
            if (!warning.gameObject.activeInHierarchy)
            {
                warning.gameObject.SetActive(true);
            }
            StartCoroutine(MenuScreenFader(warning.GetComponent<CanvasGroup>(), 0, 1, 1));
        }

        if (SuperManager.instance.explorerComplete)
        {
            explorerGameButton.SetActive(false);
        }

        if (SuperManager.instance.shooterComplete)
        {
            shooterGameButton.SetActive(false);
        }

        if(SuperManager.instance.explorerComplete || SuperManager.instance.shooterComplete)
        {
            initialChoice.SetActive(false);
            survey.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void OpenSurveyButtonClick()
    {
        string url = "https://docs.google.com/forms/d/e/1FAIpQLSeHex5f6hZT2Lsnb8SBiXHu7boa3O60FP4V0p5wm7hkAeg7WQ/viewform?usp=pp_url&entry.277285449=" + SuperManager.instance.userId.ToString();
        OpenSurvey(url);
    }

    public void Continue()
    {
        acceptedTos = true;
        continueButton.SetActive(false);
        warning.gameObject.SetActive(true);
        StartCoroutine(MenuScreenFader(tos.GetComponent<CanvasGroup>(), 1, 0, 1, warning, true));
    }

    public void WarningContinue()
    {
        warningContinueButton.SetActive(false);
        StartCoroutine(MenuScreenFader(warning.GetComponent<CanvasGroup>(), 1, 0, 1, menu, true));
    }

    public void LoadExplorer()
    {
        if (!SuperManager.instance.firstChosen)
        {
            SuperManager.instance.StartedFirstGame(Game.Explorer);
        }
        else
        {
            SuperManager.instance.StartedSecondGame(Game.Explorer);
            Debug.Log("Started second game");

        }
        StartCoroutine(MenuScreenFader(menu.GetComponent<CanvasGroup>(), 1, 0, 1));
        StartCoroutine(BeginSceneLoad(Game.Explorer));
    }

    public void LoadShooter()
    {

        if (!SuperManager.instance.firstChosen)
        {
            SuperManager.instance.StartedFirstGame(Game.Shooter);
        }
        else
        {
            SuperManager.instance.StartedSecondGame(Game.Shooter);
        }
        StartCoroutine(MenuScreenFader(menu.GetComponent<CanvasGroup>(), 1, 0, 1));
        StartCoroutine(BeginSceneLoad(Game.Shooter));
    }

    private IEnumerator BeginSceneLoad(Game game)
    {
        while (fading)
        {
            yield return new WaitForEndOfFrame();
        }

        loading.SetActive(true);
        yield return new WaitForEndOfFrame();

        switch (game)
        {
            case Game.Explorer:
                SceneManager.LoadScene("ExplorerGame");
                break;
            default:
                SceneManager.LoadScene("ShooterGame");
                break;
        }
    }

    private IEnumerator MenuScreenFader(CanvasGroup group, float start, float end, float time, Canvas canvasToToggle = null, bool disableCanvas = false)
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

        if (disableCanvas)
        {
            group.gameObject.SetActive(false);
        }

        if (canvasToToggle != null)
        {
            StartCoroutine(MenuScreenFader(canvasToToggle.GetComponent<CanvasGroup>(), 0, 1, 2));
        }
        fading = false;
    }
}
