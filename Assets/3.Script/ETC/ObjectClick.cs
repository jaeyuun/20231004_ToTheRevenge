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
    { // ������Ʈ Ŭ�� �� �ش� ��ġ�� �޾ƿ��� ��
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition); // ī�޶� ȭ�� ���� ��ǥ��
            Vector2 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition); // UI ȭ�� ����
            
            hit = Physics2D.Raycast(position, Vector2.zero); // �ش� ��ǥ�� ������Ʈ
            if (!textBox.activeSelf)
            { // �ؽ�Ʈ�ڽ��� Ȱ��ȭ ���¸� �ٸ� ������Ʈ Ŭ�� �ȵǰ�
                SelectObject();
            }
        }
    }

    private void SelectObject()
    {
        if (hit.collider != null)
        { // ������Ʈ�� ���õǾ��� ��
            target = hit.transform.gameObject;
            PlayerPrefs.SetString("ObjectName", $"{target.name}"); // Ŭ���� ������Ʈ �̸� ����
            ScriptLoad load = new ScriptLoad();
            dialogFile = load.Load(PlayerPrefs.GetString("ObjectName"));
            textScript.text = dialogFile.script[0];
            SetObjectInfo(target);
        } else
        { // ������Ʈ�� ���õ��� �ʾ��� ��
            imagePanel.SetActive(false);
            textScript.transform.gameObject.SetActive(false);
            textBox.SetActive(false);
        }
    }

    private void SetObjectInfo(GameObject gameObject)
    { // ���õ� ������Ʈ�� ���� ǥ��
        if (gameObject.CompareTag("Object"))
        { // �ܼ��� ���� ������Ʈ�� ��
            itemImage.sprite = gameObject.GetComponent<ObjectInformation>().image;
            itemText.text = gameObject.GetComponent<ObjectInformation>().text;

            imagePanel.SetActive(true);
            textScript.transform.gameObject.SetActive(true);
            textBox.SetActive(true);
        } else if (gameObject.CompareTag("NotObject"))
        { // �ܼ��� ���� �ʴ� ������Ʈ�� ��
            imagePanel.SetActive(false);
            textScript.transform.gameObject.SetActive(true);
            textBox.SetActive(true);
        }
    }
}
