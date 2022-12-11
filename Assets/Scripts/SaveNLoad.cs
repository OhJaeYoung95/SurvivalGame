using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// �����͸� ����ȭ�ϸ� ���ٷ� �����͵��� �����Ǽ� ���� ��ġ�� �а� ���Ⱑ ������
[System.Serializable]
public class SaveData       // ������ ������
{
    public Vector3 playerPos;       // �÷��̾��� ��ġ
    public Vector3 playerRot;       // �÷��̾��� ����

    public List<int> invenSlotsArrayNumber = new List<int>();        // �κ��丮 �����۽��� ��ġ ��ȣ
    public List<string> invenSlotsItemName = new List<string>();     // �κ��丮 ������ �̸�
    public List<int> invenSlotsItemNumber = new List<int>();         // �κ��丮 ������ ����

    public List<int> quickSlotsArrayNumber = new List<int>();        // ������ �����۽��� ��ġ ��ȣ
    public List<string> quickSlotsItemName = new List<string>();     // ������ ������ �̸�
    public List<int> quickSlotsItemNumber = new List<int>();         // ������ ������ ����
}

public class SaveNLoad : MonoBehaviour
{   // ���� & ����
    private SaveData saveData = new SaveData();

    private string SAVE_DATA_DIRECTORY;                 // ���丮 ��ġ
    private string SAVE_FILENAME = "/SaveFile.txt";     // �����Ǵ� �����̸�

    private PlayerController thePlayer;     // �÷��̾��� ��ġ�� �������� ���� ������Ʈ
    private Inventory theInven;             // �κ��丮�� ������ ������ ���� ������Ʈ

    // Start is called before the first frame update
    void Start()
    {   // ���丮 ������ ����
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/";

        // ���丮�� �������� ������
        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY); // ���丮 ����
    }

    public void SaveData()      // ������ �����Լ�
    {
        thePlayer = FindObjectOfType<PlayerController>();
        theInven = FindObjectOfType<Inventory>();

        saveData.playerPos = thePlayer.transform.position;      // �÷��̾� ��ġ ����
        saveData.playerRot = thePlayer.transform.eulerAngles;   // �÷��̾��� ���� ����

        // �κ��丮â ������ ���� ����
        Slot[] slots = theInven.GetSlots();     // ���� ���� �ޱ�
        for (int i = 0; i < slots.Length; i++)  // ������ ���̸�ŭ �ݺ�
        {
            if(slots[i].item !=null)    // �������� �ִٸ�
            {
                saveData.invenSlotsArrayNumber.Add(i);              // �����۽��� ��ġ ��ȣ ����
                saveData.invenSlotsItemName.Add(slots[i].item.itemName);     // ������ �̸� ����
                saveData.invenSlotsItemNumber.Add(slots[i].itemCount);       // ������ ���� ����
            }
        }        
        // ������ ������ ���� ����
        Slot[] quickslots = theInven.GetQuickSlots();     // ������ ���� �ޱ�
        for (int i = 0; i < quickslots.Length; i++)  // ������ ���̸�ŭ �ݺ�
        {
            if(quickslots[i].item !=null)    // �������� �ִٸ�
            {
                saveData.quickSlotsArrayNumber.Add(i);              // �����۽��� ��ġ ��ȣ ����
                saveData.quickSlotsItemName.Add(quickslots[i].item.itemName);     // ������ �̸� ����
                saveData.quickSlotsItemNumber.Add(quickslots[i].itemCount);       // ������ ���� ����
            }
        }

        string json = JsonUtility.ToJson(saveData);     // �÷��̾� ��ġ�� ��� ������ ���̽�ȭ ��Ų��

        // SAVE_DATA_DIRECTORY + SAVE_FILENAME ��ο� json�ؽ�Ʈ�� ���δ� ����Ų��
        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);

        Debug.Log("���� �Ϸ�");
        Debug.Log(json);
    }

    public void LoadData()      // ������ �ҷ������Լ�
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))       // ����� ������ �ִٸ�
        {
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);    // ����Ǿ� �ִ� ���̽��� �ҷ��´�
            saveData = JsonUtility.FromJson<SaveData>(loadJson);    // Jsonȭ �Ǿ��ִ� �����͸� �ٽ� �������·� Ǯ���

            thePlayer = FindObjectOfType<PlayerController>();
            theInven = FindObjectOfType<Inventory>();

            thePlayer.transform.position = saveData.playerPos;      // �÷��̾� ���� ��ġ�� ����� ��ġ�� ����
            thePlayer.transform.eulerAngles = saveData.playerRot;   // �÷��̾� ���� ������ ����� �������� ����

            // ����Ʈ�� ����� ������ ����
            for (int i = 0; i < saveData.invenSlotsItemName.Count; i++)
            {   // �κ��丮�� �ش� ���Կ� ������ ����
                theInven.LoadToInvenSlots(saveData.invenSlotsArrayNumber[i], saveData.invenSlotsItemName[i], saveData.invenSlotsItemNumber[i]);
            }

            // ����Ʈ�� ����� ������ ����
            for (int i = 0; i < saveData.invenSlotsItemName.Count; i++)
            {   // �������� �ش� ���Կ� ������ ����
                theInven.LoadToQuickSlots(saveData.quickSlotsArrayNumber[i], saveData.quickSlotsItemName[i], saveData.quickSlotsItemNumber[i]);
            }
            
            Debug.Log("�ε� �Ϸ�");
        }
        else                    // ����� ������ ���ٸ�
            Debug.Log("���̺� ������ �����ϴ�");
    }
}
