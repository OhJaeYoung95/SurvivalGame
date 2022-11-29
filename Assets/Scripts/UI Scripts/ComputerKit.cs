using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Kit                        // 키트
{
    public string KitName;              // 키트 이름
    public string kitDescription;       // 키트 설명
    public string[] needItemName;       // 제작에 필요한 재료 아이템 이름
    public int[] needItemNumber;        // 제작에 필요한 재료 아이템 수량

    public GameObject go_Kit_Prefab;    // 키트 프리팹
}

public class ComputerKit : MonoBehaviour
{
    [SerializeField]
    private Kit[] kits;                 // 키트 배열

    [SerializeField]
    private Transform tf_ItemAppear;     // 생성될 아이템 위치

    private bool isCraft = false;           // 키트 생성중인지 여부, 중복 실행 방지
    
    // 필요한 컴포넌트
    private Inventory theInven;

    private AudioSource theAudio;
    [SerializeField]
    private AudioClip sound_ButtonClick;        // 버튼 클릭시 나오는 소리
    [SerializeField]
    private AudioClip sound_Beep;               // 장치 가동 실패 소리(재료 부족)
    [SerializeField]
    private AudioClip sound_Activated;          // 장치 가동되는 소리
    [SerializeField]
    private AudioClip sound_Output;             // 아이템 드랍 소리

    void Start()
    {
        theInven = FindObjectOfType<Inventory>();
        theAudio = GetComponent<AudioSource>();
    }

    private void PlaySE(AudioClip _clip)    // 효과음 재생 사운드
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }

    public void ClickButton(int _slotNumber)    // 버튼을 클릭할때 발동하는 함수
    {
        PlaySE(sound_ButtonClick);      // 버튼 클릭 효과음 재생
        if (!isCraft)        // 키트 생성중이 아닐때
        {
            if (!CheckIngredient(_slotNumber))      // 재료 아이템 개수 확인
                return;             // 재료 아이템이 모자르면 밑에 실행 못하게 방지

            isCraft = true;         // 중복 생성 방지
            UseIngredient(_slotNumber);         // 재료 아이템 사용 함수
            StartCoroutine(CraftCoroutine(_slotNumber));        // 키트 생성 코루틴
        }
    }

    private bool CheckIngredient(int _slotNumber)       // 재료아이템 개수 확인 함수
    {
        // 필요한 재료 아이템 전부 탐색
        for (int i = 0; i < kits[_slotNumber].needItemName.Length; i++)
        {
            // 필요한 재료 아이템의 갯수보다 적으면
            if (theInven.GetItemCount(kits[_slotNumber].needItemName[i]) < kits[_slotNumber].needItemNumber[i])
            {
                PlaySE(sound_Beep);     // 장치 가동 실패 효과음 재생(재료 부족)
                return false;
            }
        }
        return true;
    }

    private void UseIngredient(int _slotNumber)
    {
        // 필요한 재료 아이템 전부 탐색
        for (int i = 0; i < kits[_slotNumber].needItemName.Length; i++)
        {
            // 필요한 재료 아이템 개수만큼 사용
            theInven.SetItemCount(kits[_slotNumber].needItemName[i], kits[_slotNumber].needItemNumber[i]);
        }
    }

    IEnumerator CraftCoroutine(int _slotNumber)         // 키트 생성 코루틴
    {
        PlaySE(sound_Activated);        // 장치 가동되는 효과음 재생
        yield return new WaitForSeconds(3f);        // 3초 대기
        PlaySE(sound_Output);           // 아이템 드랍 효과음 재생

        // 해당 슬롯번호의 키트프리팹 생성
        Instantiate(kits[_slotNumber].go_Kit_Prefab, tf_ItemAppear.position, Quaternion.identity);
        isCraft = false;            // 중복 생성 방지
    }
}
