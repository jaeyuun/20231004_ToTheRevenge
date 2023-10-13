using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogLoad : MonoBehaviour
{
    [SerializeField] private bool isGameScene;
    [SerializeField] private Sprite[] charImages;
    [SerializeField] private Image charImage;

    [SerializeField] private Sprite[] scriptImages;
    [SerializeField] private Image scriptImage;
    [SerializeField] private Text scriptText;
    [SerializeField] private GameObject textBox; // GameScene
    [SerializeField] private GameObject imagePanel; // GameScene

    [SerializeField] private GameObject selectPanel;
    [SerializeField] private Text[] scriptButton;

    private AudioController audioController;

    private string writerText = "";
    private ScriptData dialogFile;
    private int a = 1;

    private bool isEnd = false;
    private bool skipScript = false;
    private int skipScriptIndex = 0;
    private int skipCount = 0;

    private void Awake()
    {
        PlayerPrefs.SetString("SceneName", $"{SceneManager.GetActiveScene().name}");
        ScriptLoad load = new ScriptLoad();
        if (!isGameScene)
        {
            if (PlayerPrefs.GetInt("isEnd") == 1)
            {
                dialogFile = load.Load("End");
                if (PlayerPrefs.GetInt("EndingIndex").Equals(1))
                { // 힐링 캠프
                    scriptImage.sprite = scriptImages[1];
                    StartCoroutine(TypingText_Co(dialogFile.script[0]));
                    PlayerPrefs.SetInt("isEnd", 0);
                    return;
                }
                else
                { // 스스로 선택한 죽음
                    scriptImage.sprite = scriptImages[0];
                    StartCoroutine(TypingText_Co(dialogFile.script[1]));
                    PlayerPrefs.SetInt("isEnd", 0);
                    return;
                }
            } else
            {
                dialogFile = load.Load(PlayerPrefs.GetString("ScriptName"));
                StartCoroutine(TypingText_Co(dialogFile.script[0]));
            }
        }

        audioController = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioController>();
        if (PlayerPrefs.HasKey("ScriptIndex"))
        {
            a = PlayerPrefs.GetInt("ScriptIndex");
        }
    }

    private void Update()
    {
        if (!isGameScene)
        {
            if (PlayerPrefs.GetInt("isEnd") != 1 && !SceneManager.GetActiveScene().name.Equals("End"))
            {
                OnPressScript();
            }
        } else
        {
            OnClickScript();
        }
    }


    #region 스크립트씬
    private void OnPressAudio()
    { // 스크립트 도중 나오는 효과음
        if (PlayerPrefs.GetString("ScriptName").Equals("Prologue"))
        {
            switch (PlayerPrefs.GetInt("ScriptIndex"))
            {
                case 1:
                    audioController.PlayEffect(0); // News
                    break;
            }
        }
    }

    private void OnPressImage()
    {
        if (PlayerPrefs.GetString("ScriptName").Equals("Prologue"))
        { // 프롤로그
            switch (PlayerPrefs.GetInt("ScriptIndex"))
            {
                case 4:
                    scriptImage.sprite = scriptImages[0];
                    scriptImage.transform.gameObject.SetActive(true);
                    break;
                case 6:
                    scriptImage.sprite = scriptImages[3];
                    break;
                case 8:
                    scriptImage.sprite = scriptImages[2];
                    break;
                case 9:
                    scriptImage.transform.gameObject.SetActive(false);
                    break;
                case 10:
                    scriptImage.transform.gameObject.SetActive(true);
                    break;
                case 13: // 로비
                    scriptImage.sprite = scriptImages[4];
                    break;
                case 15: // 지희
                    charImage.sprite = charImages[2];
                    charImage.transform.gameObject.SetActive(true);
                    break;
                case 16: // 빈
                    charImage.sprite = charImages[0];
                    break;
                case 17: // 해용
                    charImage.sprite = charImages[1];
                    break;
                case 18: // 준원
                    charImage.sprite = charImages[3];
                    break;
                case 20: // 관리자야?
                    SelectButton(); // 관리자, 손님
                    PlayerPrefs.SetInt("ScriptIndex", a);
                    break;
                case 22:
                    charImage.sprite = charImages[0];
                    break;
                case 23:
                    charImage.transform.gameObject.SetActive(false);
                    break;
            }
        }
        else if (PlayerPrefs.GetString("ScriptName").Equals("Lobby")) {
            switch(PlayerPrefs.GetInt("ScriptIndex"))
            {
                case 1:
                    scriptImage.sprite = scriptImages[4]; // Lobby
                    scriptImage.transform.gameObject.SetActive(true);
                    charImage.sprite = charImages[4];
                    charImage.transform.gameObject.SetActive(true);
                    break;
                case 2:
                    charImage.sprite = charImages[3];
                    break;
                case 3:
                    charImage.sprite = charImages[1];
                    break;
                case 6:
                    charImage.sprite = charImages[2];
                    break;
                case 8:
                    charImage.sprite = charImages[0];
                    break;
                case 9: // 밀실 살인 사건?
                    SelectButton();
                    PlayerPrefs.SetInt("ScriptIndex", a);
                    break;
                case 10:
                    charImage.sprite = charImages[2];
                    break;
                case 11:
                    charImage.sprite = charImages[1];
                    break;
                case 12:
                    charImage.sprite = charImages[2];
                    break;
                case 13:
                    charImage.transform.gameObject.SetActive(false);
                    break;
                case 14:
                    charImage.sprite = charImages[4]; // 상현
                    charImage.transform.gameObject.SetActive(true);
                    break;
                case 17:
                    charImage.transform.gameObject.SetActive(false);
                    break;
            }
        }
        else if (PlayerPrefs.GetString("ScriptName").Equals("Ending") && !SceneManager.GetActiveScene().name.Equals("End"))
        {
            switch (PlayerPrefs.GetInt("ScriptIndex"))
            {
                case 1:
                    scriptImage.sprite = scriptImages[7]; // Dining
                    scriptImage.transform.gameObject.SetActive(true);
                    charImage.sprite = charImages[4];
                    charImage.transform.gameObject.SetActive(true);
                    break;
                case 3:
                    SelectButton();
                    PlayerPrefs.SetInt("ScriptIndex", a);
                    break;
            }
        }
    }

    private void SelectButton()
    {
        selectPanel.SetActive(true);
        if (PlayerPrefs.GetString("ScriptName").Equals("Prologue"))
        {
            switch (PlayerPrefs.GetInt("ScriptIndex"))
            {
                case 20:
                    scriptButton[0].text = "▶ 관리자입니다.";
                    scriptButton[1].text = "▶ 손님입니다.";
                    scriptButton[0].transform.gameObject.SetActive(true);
                    scriptButton[1].transform.gameObject.SetActive(true);
                    break;
            }
        }
        else if (PlayerPrefs.GetString("ScriptName").Equals("Lobby"))
        {
            switch (PlayerPrefs.GetInt("ScriptIndex"))
            {
                case 9:
                    scriptButton[0].text = "▶ 밀실 살인 사건?";
                    scriptButton[0].transform.gameObject.SetActive(true);
                    break;
            }
        }
        else if (PlayerPrefs.GetString("ScriptName").Equals("Ending"))
        {
            switch (PlayerPrefs.GetInt("ScriptIndex"))
            {
                case 3:
                    scriptButton[0].text = "▶ 망설임";
                    scriptButton[1].text = "▶ 계속 진행";
                    scriptButton[0].transform.gameObject.SetActive(true);
                    scriptButton[1].transform.gameObject.SetActive(true);
                    break;
            }
        }
    }

    private void SelectDialog(int index)
    {
        if (PlayerPrefs.GetString("ScriptName").Equals("Prologue"))
        {
            switch (PlayerPrefs.GetInt("ScriptIndex"))
            {
                case 20:
                    if (index.Equals(1))
                    { // 관리자입니다.
                        charImage.sprite = charImages[0];
                        a += 2;
                        PlayerPrefs.SetInt("ScriptIndex", a);
                        scriptText.text = "";
                        StartCoroutine(TypingText_Co(dialogFile.script[a]));
                    }
                    else if (index.Equals(2))
                    { // 손님입니다.
                        skipScript = true; // 스킵을 해야할 때
                        skipScriptIndex = 22; // 인덱스에서
                        skipCount = 1; // 1줄만큼
                    }
                    break;
            }
        }
        else if (PlayerPrefs.GetString("ScriptName").Equals("Lobby"))
        {
            switch (PlayerPrefs.GetInt("ScriptIndex"))
            {
                case 9:
                    if (index.Equals(1))
                    { // 밀실 살인 사건?
                        charImage.sprite = charImages[2];
                        PlayerPrefs.SetInt("ScriptIndex", a);
                        scriptText.text = "";
                        StartCoroutine(TypingText_Co(dialogFile.script[a]));
                    }
                    break;
            }
        }
        else if (PlayerPrefs.GetString("ScriptName").Equals("Ending"))
        {
            switch (PlayerPrefs.GetInt("ScriptIndex"))
            {
                case 3:
                    if (index.Equals(1))
                    { // 망설임
                        PlayerPrefs.SetInt("EndingIndex", 0); // 스스로 선택한 죽음
                        a += 4;
                        PlayerPrefs.SetInt("ScriptIndex", a);
                        scriptText.text = "";
                        StartCoroutine(TypingText_Co(dialogFile.script[a]));
                    }
                    else if (index.Equals(2))
                    { // 계속 진행
                        PlayerPrefs.SetInt("EndingIndex", 1); // 힐링캠프
                        skipScript = true;
                        skipScriptIndex = 6;
                        skipCount = 4;
                    }
                    break;
            }
        }
    }

    private void PlusIndex(int index)
    {
        if (skipScript && index == skipScriptIndex)
        {
            a += skipCount;
            PlayerPrefs.SetInt("ScriptIndex", a);
            skipScript = false;
            skipScriptIndex = 0;
            skipCount = 0;
        }
    }

    public void OnClickSelect(int index)
    { // 버튼을 클릭했을 때
        SelectDialog(index);
        a++;
        PlayerPrefs.SetInt("ScriptIndex", a);
        selectPanel.SetActive(false);
    }

    private void OnPressScript()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnPressAudio();
            OnPressImage();

            if (a.Equals(dialogFile.script.Length))
            {
                a = 1;
                PlayerPrefs.SetInt("ScriptIndex", a);
                if (!isGameScene)
                { // 게임씬이 아닐 때: 게임씬일 때는 대사가 끝나더라도 다른 씬으로 넘어가면 안됨
                    
                    /*switch (PlayerPrefs.GetString("ScriptName"))
                    {
                        case "Prologue":
                            keepScene = true;
                            break;
                        case "Lobby":
                            break;
                    }*/
                    if (PlayerPrefs.GetString("ScriptName").Equals("Ending"))
                    {
                        PlayerPrefs.SetInt("isEnd", 1);
                        isEnd = true;
                    }
                    if (isEnd)
                    { // 엔딩일 때
                        // 스크립트에 따라 엔딩 변경... todo
                        SceneManager.LoadScene("End");
                    }
                    else
                    { // 엔딩이 아닐 때
                        SceneManager.LoadScene("GameScene");
                    }
                }
                return;
            }

            scriptText.text = "";
            StartCoroutine(TypingText_Co(dialogFile.script[a]));

            if (!selectPanel.activeSelf)
            { // 선택지가 있을 때는 넘어가지 않음
                a++;
            }
            PlusIndex(a);

            // 현재 스크립트 저장
            PlayerPrefs.SetInt("ScriptIndex", a);
            PlayerPrefs.SetString("ScriptName", dialogFile.name);
        }
    }
    #endregion
    #region 게임씬
    private void OnClickScript()
    {
        if (Input.GetKeyDown(KeyCode.Space) && textBox.activeSelf)
        {
            if (a.Equals(ObjectClick.dialogFile.script.Length))
            {
                a = 1;
                PlayerPrefs.SetInt("ScriptIndex", a);
                scriptText.transform.gameObject.SetActive(false);
                imagePanel.SetActive(false);
                textBox.SetActive(false);
                return;
            }
            scriptText.text = "";
            StartCoroutine(TypingText_Co(ObjectClick.dialogFile.script[a]));

            a++;
        }
    }
    #endregion

    private IEnumerator TypingText_Co(string dialog)
    { // 대사 타이핑 효과
        writerText = "";

        for (int i = 0; i < dialog.Length; i++)
        {
            writerText += dialog[i];
            scriptText.text = writerText;
            yield return null;
        }
    }
}
