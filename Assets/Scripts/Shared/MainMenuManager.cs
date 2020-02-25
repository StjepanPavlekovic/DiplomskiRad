using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
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
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        //DontDestroyOnLoad(this);
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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
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
        StartCoroutine(MenuScreenFader(menu.GetComponent<CanvasGroup>(), 1, 0, 1));
        StartCoroutine(BeginSceneLoad(Game.Explorer));
    }

    public void LoadShooter()
    {
        StartCoroutine(MenuScreenFader(menu.GetComponent<CanvasGroup>(), 1, 0, 1));
        StartCoroutine(BeginSceneLoad(Game.Shooter));
    }

    private IEnumerator BeginSceneLoad(Game game)
    {
        while (fading)
        {
            yield return new WaitForEndOfFrame();
        }
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
