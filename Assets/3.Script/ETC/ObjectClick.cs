using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ObjectClick : MonoBehaviour
{
    [SerializeField] private GameObject imagePanel;
    [SerializeField] private Image itemImage;
    [SerializeField] private Text itemText;
    [SerializeField] private Text textScript;
    [SerializeField] private GameObject textBox;

    RaycastHit2D hit;
    private GameObject target;
    public static ScriptData dialogFile;

    private void Update()
    {
        MouseClick();
    }

    private void MouseClick()
    { // 오브젝트 클릭 시 해당 위치를 받아오는 것
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 카메라 화면 기준 좌표값
            Vector2 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition); // UI 화면 기준
            
            hit = Physics2D.Raycast(position, Vector2.zero); // 해당 좌표의 오브젝트
            if (!textBox.activeSelf)
            { // 텍스트박스가 활성화 상태면 다른 오브젝트 클릭 안되게
                SelectObject();
            }
        }
    }

    private void SelectObject()
    {
        if (hit.collider != null)
        { // 오브젝트가 선택되었을 때
            target = hit.transform.gameObject;
            PlayerPrefs.SetString("ObjectName", $"{target.name}"); // 클릭한 오브젝트 이름 저장
            ScriptLoad load = new ScriptLoad();
            dialogFile = load.Load(PlayerPrefs.GetString("ObjectName"));
            textScript.text = dialogFile.script[0];
            SetObjectInfo(target);
        } else
        { // 오브젝트가 선택되지 않았을 때
            imagePanel.SetActive(false);
            textScript.transform.gameObject.SetActive(false);
            textBox.SetActive(false);
        }
    }

    private void SetObjectInfo(GameObject gameObject)
    { // 선택된 오브젝트의 정보 표시
        if (gameObject.CompareTag("Object"))
        { // 단서로 들어가는 오브젝트일 때
            itemImage.sprite = gameObject.GetComponent<ObjectInformation>().image;
            itemText.text = gameObject.GetComponent<ObjectInformation>().text;

            imagePanel.SetActive(true);
            textScript.transform.gameObject.SetActive(true);
            textBox.SetActive(true);
        } else if (gameObject.CompareTag("NotObject"))
        { // 단서로 들어가지 않는 오브젝트일 때
            imagePanel.SetActive(false);
            textScript.transform.gameObject.SetActive(true);
            textBox.SetActive(true);
        }
    }
}
