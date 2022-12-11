using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// 데이터를 직렬화하면 한줄로 데이터들이 나열되서 저장 장치에 읽고 쓰기가 쉬워짐
[System.Serializable]
public class SaveData       // 저장할 데이터
{
    public Vector3 playerPos;       // 플레이어의 위치
    public Vector3 playerRot;       // 플레이어의 방향

    public List<int> invenSlotsArrayNumber = new List<int>();        // 인벤토리 아이템슬롯 위치 번호
    public List<string> invenSlotsItemName = new List<string>();     // 인벤토리 아이템 이름
    public List<int> invenSlotsItemNumber = new List<int>();         // 인벤토리 아이템 개수

    public List<int> quickSlotsArrayNumber = new List<int>();        // 퀵슬롯 아이템슬롯 위치 번호
    public List<string> quickSlotsItemName = new List<string>();     // 퀵슬롯 아이템 이름
    public List<int> quickSlotsItemNumber = new List<int>();         // 퀵슬롯 아이템 개수
}

public class SaveNLoad : MonoBehaviour
{   // 선언 & 생성
    private SaveData saveData = new SaveData();

    private string SAVE_DATA_DIRECTORY;                 // 디렉토리 위치
    private string SAVE_FILENAME = "/SaveFile.txt";     // 생성되는 파일이름

    private PlayerController thePlayer;     // 플레이어의 위치를 가져오기 위한 컴포넌트
    private Inventory theInven;             // 인벤토리에 아이템 저장을 위한 컴포넌트

    // Start is called before the first frame update
    void Start()
    {   // 디렉토리 폴더명 생성
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/";

        // 디렉토리가 존재하지 않을때
        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY); // 디렉토리 생성
    }

    public void SaveData()      // 데이터 저장함수
    {
        thePlayer = FindObjectOfType<PlayerController>();
        theInven = FindObjectOfType<Inventory>();

        saveData.playerPos = thePlayer.transform.position;      // 플레이어 위치 저장
        saveData.playerRot = thePlayer.transform.eulerAngles;   // 플레이어의 방향 저장

        // 인벤토리창 아이템 정보 저장
        Slot[] slots = theInven.GetSlots();     // 슬롯 정보 받기
        for (int i = 0; i < slots.Length; i++)  // 슬롯의 길이만큼 반복
        {
            if(slots[i].item !=null)    // 아이템이 있다면
            {
                saveData.invenSlotsArrayNumber.Add(i);              // 아이템슬롯 위치 번호 저장
                saveData.invenSlotsItemName.Add(slots[i].item.itemName);     // 아이템 이름 저장
                saveData.invenSlotsItemNumber.Add(slots[i].itemCount);       // 아이템 개수 저장
            }
        }        
        // 퀵슬롯 아이템 정보 저장
        Slot[] quickslots = theInven.GetQuickSlots();     // 퀵슬롯 정보 받기
        for (int i = 0; i < quickslots.Length; i++)  // 슬롯의 길이만큼 반복
        {
            if(quickslots[i].item !=null)    // 아이템이 있다면
            {
                saveData.quickSlotsArrayNumber.Add(i);              // 아이템슬롯 위치 번호 저장
                saveData.quickSlotsItemName.Add(quickslots[i].item.itemName);     // 아이템 이름 저장
                saveData.quickSlotsItemNumber.Add(quickslots[i].itemCount);       // 아이템 개수 저장
            }
        }

        string json = JsonUtility.ToJson(saveData);     // 플레이어 위치가 담긴 파일을 제이슨화 시킨다

        // SAVE_DATA_DIRECTORY + SAVE_FILENAME 경로에 json텍스트를 전부다 기억시킨다
        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);

        Debug.Log("저장 완료");
        Debug.Log(json);
    }

    public void LoadData()      // 데이터 불러오기함수
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))       // 저장된 파일이 있다면
        {
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);    // 저장되어 있는 제이슨을 불러온다
            saveData = JsonUtility.FromJson<SaveData>(loadJson);    // Json화 되어있는 데이터를 다시 원래형태로 풀어낸다

            thePlayer = FindObjectOfType<PlayerController>();
            theInven = FindObjectOfType<Inventory>();

            thePlayer.transform.position = saveData.playerPos;      // 플레이어 현재 위치를 저장된 위치로 대입
            thePlayer.transform.eulerAngles = saveData.playerRot;   // 플레이어 현재 방향을 저장된 방향으로 대입

            // 리스트에 저장된 아이템 개수
            for (int i = 0; i < saveData.invenSlotsItemName.Count; i++)
            {   // 인벤토리의 해당 슬롯에 아이템 저장
                theInven.LoadToInvenSlots(saveData.invenSlotsArrayNumber[i], saveData.invenSlotsItemName[i], saveData.invenSlotsItemNumber[i]);
            }

            // 리스트에 저장된 아이템 개수
            for (int i = 0; i < saveData.invenSlotsItemName.Count; i++)
            {   // 퀵슬롯의 해당 슬롯에 아이템 저장
                theInven.LoadToQuickSlots(saveData.quickSlotsArrayNumber[i], saveData.quickSlotsItemName[i], saveData.quickSlotsItemNumber[i]);
            }
            
            Debug.Log("로드 완료");
        }
        else                    // 저장된 파일이 없다면
            Debug.Log("세이브 파일이 없습니다");
    }
}
