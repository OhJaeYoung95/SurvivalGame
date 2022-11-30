using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Kit                        // ŰƮ
{
    public string KitName;              // ŰƮ �̸�
    public string kitDescription;       // ŰƮ ����
    public string[] needItemName;       // ���ۿ� �ʿ��� ��� ������ �̸�
    public int[] needItemNumber;        // ���ۿ� �ʿ��� ��� ������ ����

    public GameObject go_Kit_Prefab;    // ŰƮ ������
}

public class ComputerKit : MonoBehaviour
{
    [SerializeField]
    private Kit[] kits;                 // ŰƮ �迭

    [SerializeField]
    private Transform tf_ItemAppear;     // ������ ������ ��ġ
    [SerializeField]
    private GameObject go_BaseUI;       // ��ǻ�� ŰƮ UI

    private bool isCraft = false;           // ŰƮ ���������� ����, �ߺ� ���� ����
    public bool isPowerOn = false;          // ���� ����

    // �ʿ��� ������Ʈ
    private Inventory theInven;             // �κ� ������ ���� ������ ���� ������Ʈ
    [SerializeField]
    private ComputerTooltip theToolTip;     // ����UI Ȱ��ȭ, ��Ȱ��ȭ�� ���� ������Ʈ

    private AudioSource theAudio;
    [SerializeField]
    private AudioClip sound_ButtonClick;        // ��ư Ŭ���� ������ �Ҹ�
    [SerializeField]
    private AudioClip sound_Beep;               // ��ġ ���� ���� �Ҹ�(��� ����)
    [SerializeField]
    private AudioClip sound_Activated;          // ��ġ �����Ǵ� �Ҹ�
    [SerializeField]
    private AudioClip sound_Output;             // ������ ��� �Ҹ�

    public void ShowToolTip(int _buttonNum)     // ���� Ȱ��ȭ, �����ִ� �Լ�
    {
        theToolTip.ShowToolTip(kits[_buttonNum].KitName, kits[_buttonNum].kitDescription, kits[_buttonNum].needItemName, kits[_buttonNum].needItemNumber);
    }

    public void HideToolTip()           // ���� ��Ȱ��ȭ, ����� �Լ�
    {
        theToolTip.HideToolTip();
    }

    void Start()
    {
        // Ŀ�� ����, Ŀ���� ������鼭 ���� ��� ����
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;     // ���콺 Ŀ�� ��Ȱ��ȭ

        theInven = FindObjectOfType<Inventory>();
        theAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isPowerOn)       // ��ǻ�� ������ On�϶�
            if (Input.GetKeyDown(KeyCode.Escape))
                PowerOff();
    }

    public void PowerOn()       // ��ǻ�� ���� On
    {
        GameManager.isOnComputer = true;     // Ŀ�� Ȱ��ȭ
        isPowerOn = true;               // ���� On ����
        go_BaseUI.SetActive(true);
    }

    private void PowerOff()      // ��ǻ�� ���� Off
    {
        GameManager.isOnComputer = false;     // Ŀ�� ��Ȱ��ȭ
        isPowerOn = false;              // ���� Off ����
        theToolTip.HideToolTip();       // ���� ��Ȱ��ȭ
        go_BaseUI.SetActive(false);
    }

    private void PlaySE(AudioClip _clip)    // ȿ���� ��� ����
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }

    public void ClickButton(int _slotNumber)    // ��ư�� Ŭ���Ҷ� �ߵ��ϴ� �Լ�
    {
        PlaySE(sound_ButtonClick);      // ��ư Ŭ�� ȿ���� ���
        if (!isCraft)        // ŰƮ �������� �ƴҶ�
        {
            if (!CheckIngredient(_slotNumber))      // ��� ������ ���� Ȯ��
                return;             // ��� �������� ���ڸ��� �ؿ� ���� ���ϰ� ����

            isCraft = true;         // �ߺ� ���� ����
            UseIngredient(_slotNumber);         // ��� ������ ��� �Լ�
            StartCoroutine(CraftCoroutine(_slotNumber));        // ŰƮ ���� �ڷ�ƾ
        }
    }

    private bool CheckIngredient(int _slotNumber)       // �������� ���� Ȯ�� �Լ�
    {
        // �ʿ��� ��� ������ ���� Ž��
        for (int i = 0; i < kits[_slotNumber].needItemName.Length; i++)
        {
            // �ʿ��� ��� �������� �������� ������
            if (theInven.GetItemCount(kits[_slotNumber].needItemName[i]) < kits[_slotNumber].needItemNumber[i])
            {
                PlaySE(sound_Beep);     // ��ġ ���� ���� ȿ���� ���(��� ����)
                return false;
            }
        }
        return true;
    }

    private void UseIngredient(int _slotNumber)
    {
        // �ʿ��� ��� ������ ���� Ž��
        for (int i = 0; i < kits[_slotNumber].needItemName.Length; i++)
        {
            // �ʿ��� ��� ������ ������ŭ ���
            theInven.SetItemCount(kits[_slotNumber].needItemName[i], kits[_slotNumber].needItemNumber[i]);
        }
    }

    IEnumerator CraftCoroutine(int _slotNumber)         // ŰƮ ���� �ڷ�ƾ
    {
        PlaySE(sound_Activated);        // ��ġ �����Ǵ� ȿ���� ���
        yield return new WaitForSeconds(3f);        // 3�� ���
        PlaySE(sound_Output);           // ������ ��� ȿ���� ���

        // �ش� ���Թ�ȣ�� ŰƮ������ ����
        Instantiate(kits[_slotNumber].go_Kit_Prefab, tf_ItemAppear.position, Quaternion.identity);
        isCraft = false;            // �ߺ� ���� ����
    }
}
