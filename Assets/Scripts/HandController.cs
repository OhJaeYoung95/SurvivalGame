using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CloseWeaponController 스크립트를 상속받는다
public class HandController : CloseWeaponController
{
    // 활성화 여부
    public static bool isActivate = false;
    public static Item currentKit;      // 설치하려는 키트 (연금 테이블)

    private bool isPreview = false;     // 키트 미리보기 활성화 여부

    private GameObject go_Preview;      // 설치할 키트 미리보기
    private Vector3 previewPos;         // 설치할 키트 위치
    [SerializeField]
    private float rangeAdd;             // 건축시 추가 사정거리

    [SerializeField]
    private QuickSlotController theQuickSlot;   // 퀵슬롯 관련 정보 가져오기 위한 변수

    void Update()
    {
        if (isActivate && !Inventory.inventoryActivated) // 손 활성화 & 인벤토리 비활성화시
        {
            if(currentKit == null)      // 설치하려는 키트가 없을시
            {
                if (QuickSlotController.go_HandItem == null)        // 손에 든 아이템이 없다면
                    TryAttack();
                else                                                // 있다면
                    TryEating();
            }
            else
            {
                if(!isPreview)      // 미리보기가 비활성화일때
                    InstallPreviewKit();        // 키트 미리보기 프리팹 생성
                PreviewPositionUpdate();        // 미리보기 프리팹 위치 업데이트
                Build();                // 키트 프리팹 건축 함수
            }
        }
    }

    private void InstallPreviewKit()        // 키트 미리보기 프리팹 생성
    {
        isPreview = true;
        go_Preview = Instantiate(currentKit.kitPreviewPrefab, transform.position, Quaternion.identity);
    }

    private void PreviewPositionUpdate()        // 미리보기 프리팹 위치 업데이트
    {
        // 전방으로 레이저를 쏴서 layerMask에 해당하는 레이어만 충돌하도록 설정
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range + rangeAdd, layerMask))
        {
            previewPos = hitInfo.point;     // 레이저에 맞은 위치
            go_Preview.transform.position = previewPos;     // 위치 매순간 업데이트
        }
    }

    private void Build()        // 키트 프리팹 건축 함수
    {
        if(Input.GetButtonDown("Fire1"))    // 마우스 좌클릭시
        {
            // 건물 설치 가능시
            if(go_Preview.GetComponent<PreviewObject>().IsBuildable())
            {
                theQuickSlot.DecreaseSelectedItem();        // 슬롯 아이템 개수 -1
                GameObject temp = Instantiate(currentKit.kitPrefab, previewPos, Quaternion.identity);   // 키트 프리팹 생성
                temp.name = "Archemy Kit";
                // 초기화 적업(아이템상태, bool형), 프리팹제거
                Destroy(go_Preview);        // 미리보기 프리팹 제거
                currentKit = null;
                isPreview = false;
            }
        }
    }

    public void Cancel()        // 외부에서 키트 미리보기 초기화해주는 함수
    {
        // 초기화 적업(아이템상태, bool형), 프리팹제거
        Destroy(go_Preview);        // 미리보기 프리팹 제거
        currentKit = null;
        isPreview = false;
    }

    private void TryEating()        // 소모품 사용(먹기)
    {
        if(Input.GetButtonDown("Fire1") && !theQuickSlot.GetIsCoolTime())        // 마우스 좌클릭시
        {
            currentCloseWeapon.anim.SetTrigger("Eat");      // 아이템 먹는 애니메이션 실행
            theQuickSlot.DecreaseSelectedItem();             // 선택된 퀵슬롯 아이템 소모 함수
        }
    }

    // 공격 코루틴
    protected override IEnumerator HitCoroutine()
    {
        while(isSwing)
        {
            if(CheckObject())
            {
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    // 근접 무기 변화 함수 덮어쓰기(부모 클래스에 있는 함수에서 자식 함수에 추가할 거 작성)
    public override void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        base.CloseWeaponChange(_closeWeapon);
        isActivate = true;
    }

}
