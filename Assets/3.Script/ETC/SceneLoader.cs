using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BackGround
{
    Lobby = 0,
    Dining
}

public class SceneLoader : MonoBehaviour
{
    private float time = 0;
    [SerializeField] private bool isGameScene;
    [SerializeField] private GameObject[] backGround;
    [SerializeField] private GameObject loadingScreen;

    private void Awake()
    {
        if (isGameScene)
        { // 게임씬일때만
            switch (PlayerPrefs.GetString("ScriptName"))
            { // 스크립트에 따라 게임씬 배경 변경... todo
                case "Prologue":
                    backGround[(int)BackGround.Lobby].SetActive(true);
                    break;
                case "Lobby":
                    backGround[(int)BackGround.Dining].SetActive(true);
                    break;
            }
        }
    }

    public void LoadStartScene(bool isFirst)
    { // 게임 시작, 불러오기 때 사용
        StartCoroutine(LoadingScene_Co("ScriptScene"));
        PlayerPrefs.SetString("ScriptName", "Prologue");
        PlayerPrefs.SetInt("ScriptIndex", 1);
    }

    private IEnumerator LoadingScene_Co(string sceneName)
    { // 비동기 로딩 씬
        loadingScreen.SetActive(true); // 로딩 화면
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while(!operation.isDone)
        {
            time += Time.deltaTime;
            if (time > 2)
            {
                operation.allowSceneActivation = true;
            }

            yield return null; // 한 프레임 넘김
        }
        loadingScreen.SetActive(false); // 씬 로딩 후 화면 숨김
    }

    public void LoadScene(string sceneName = "")
    {
        if (sceneName.Equals(""))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            if (sceneName.Equals("Intro"))
            { // Home button press, 현재 씬 위치 저장
                PlayerPrefs.SetString("SceneName", $"{SceneManager.GetActiveScene().name}");
            }
            switch (PlayerPrefs.GetString("ScriptName"))
            {
                case "Prologue":
                    PlayerPrefs.SetString("ScriptName", "Lobby");
                    break;
                case "Lobby":
                    PlayerPrefs.SetString("ScriptName", "Ending");
                    break;
            }

            SceneManager.LoadScene(sceneName);
        }
    }

    public void OnClickFolder(GameObject gameObject)
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        } else
        {
            gameObject.SetActive(true);
        }
    }

    public void GameExit()
    {
        Application.Quit();
    }
}