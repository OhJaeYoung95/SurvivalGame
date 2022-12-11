using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string sceneName = "GameStage";        // ���������� �̵��� �̸�

    private SaveNLoad theSaveNLoad;     // ���� �ҷ����⸦ ���� ������Ʈ

    public static Title instance;       // �̱���ȭ


    private void Awake()        // �̱���ȭ
    {
        if (instance == null)       // ��ü�� ���ٸ�
        {
            instance = this;        // ��ü����
            DontDestroyOnLoad(gameObject);  // ���̵� ���� ��ü �ı����� �ʰ� ����
        }
        else            // ��ü�� �ִٸ�
            Destroy(this.gameObject);       // ��ü����
    }

    public void ClickStart()        // ��ŸƮ ��ư Ŭ�� �Լ�
    {
        Debug.Log("�ε�");
        SceneManager.LoadScene(sceneName);      // ���̵�
    }

    public void ClickLoad()     // �ε� ��ư Ŭ�� �Լ�
    {
        Debug.Log("�ε�");

        StartCoroutine(LoadCoroutine());

    }

    IEnumerator LoadCoroutine()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while(!operation.isDone)        // ���� �� �ҷ��ö����� ���
        {
            yield return null;
        }

        theSaveNLoad = FindObjectOfType<SaveNLoad>();       // ���������� ��ü������
        theSaveNLoad.LoadData();                // ����
        gameObject.SetActive(false);       // Ÿ��Ʋ UI ��Ȱ��ȭ
    }

    public void ClickExit()     // �������� ��ư Ŭ�� �Լ�
    {
        Debug.Log("���� ����");
        Application.Quit();     // ��������
    }
}
