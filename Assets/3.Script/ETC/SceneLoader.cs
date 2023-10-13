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
        { // ���Ӿ��϶���
            switch (PlayerPrefs.GetString("ScriptName"))
            { // ��ũ��Ʈ�� ���� ���Ӿ� ��� ����... todo
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
    { // ���� ����, �ҷ����� �� ���
        StartCoroutine(LoadingScene_Co("ScriptScene"));
        PlayerPrefs.SetString("ScriptName", "Prologue");
        PlayerPrefs.SetInt("ScriptIndex", 1);
    }

    private IEnumerator LoadingScene_Co(string sceneName)
    { // �񵿱� �ε� ��
        loadingScreen.SetActive(true); // �ε� ȭ��
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while(!operation.isDone)
        {
            time += Time.deltaTime;
            if (time > 2)
            {
                operation.allowSceneActivation = true;
            }

            yield return null; // �� ������ �ѱ�
        }
        loadingScreen.SetActive(false); // �� �ε� �� ȭ�� ����
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
            { // Home button press, ���� �� ��ġ ����
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