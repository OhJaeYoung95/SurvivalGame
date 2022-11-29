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

    private bool isCraft = false;           // ŰƮ ���������� ����, �ߺ� ���� ����
    
    // �ʿ��� ������Ʈ
    private Inventory theInven;

    private AudioSource theAudio;
    [SerializeField]
    private AudioClip sound_ButtonClick;        // ��ư Ŭ���� ������ �Ҹ�
    [SerializeField]
    private AudioClip sound_Beep;               // ��ġ ���� ���� �Ҹ�(��� ����)
    [SerializeField]
    private AudioClip sound_Activated;          // ��ġ �����Ǵ� �Ҹ�
    [SerializeField]
    private AudioClip sound_Output;             // ������ ��� �Ҹ�

    void Start()
    {
        theInven = FindObjectOfType<Inventory>();
        theAudio = GetComponent<AudioSource>();
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
