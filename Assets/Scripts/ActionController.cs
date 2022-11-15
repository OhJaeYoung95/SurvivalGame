using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range;    // 습득 가능한 최대 거리

    private bool pickupActivated = false;   // 습득 가능할 시 true

    private RaycastHit hitInfo;         // 충돌체 정보 저장

    [SerializeField]
    private LayerMask layerMask;        // 아이템 레이어에만 반응하도록 레이어 마스크 설정

    // 필요한 컴포넌트
    [SerializeField]
    private Text actionText;            // 액션에 쓰이는 텍스트

    // Update is called once per frame
    void Update()
    {
        CheckItem();
        TryAction();
    }

    private void TryAction()            // 아이템 습득
    {
        if(Input.GetKeyDown(KeyCode.E))     // E키를 누른다면
        {
            CheckItem();
            CanPickUp();
        }
    }

    private void CanPickUp()                // 아이템 습득함수
    {
        if(pickupActivated)         // 아이템 습득 가능할시
        {
            if(hitInfo.transform != null)       // Raycast광선에 충돌 물체가 있다면
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득했습니다");      // 아이템 획득여부 콘솔창에 띄어줌
                Destroy(hitInfo.transform.gameObject);          // 아이템 게임오브젝트 제거
                InfoDisappear();                                // 아이템 획득시 아이템 정보 비활성화
            }
        }
    }

    private void CheckItem()            // 아이템이 존재 여부 확인
    {
        // 광선을 플레이어에 위치에서 플레이어가 바라보는 방향으로 쏘아 레이어 마스크가 닿는지 확인
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, layerMask))
        {
            if(hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
        }
        else                        // 아이템이 광선에 닿지 않았을때
        {
            InfoDisappear();
        }
    }

    private void ItemInfoAppear()           // 아이템 정보 활성화
    {
        pickupActivated = true;                      // 아이템 줍기 가능
        actionText.gameObject.SetActive(true);       // 아이템 정보 활성화
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득" + "<color=yellow>" + "(E)" + "</color>";        // 해당 아이템 이름 획득표시, 텍스트에 색 입힘
    }

    private void InfoDisappear()            // 아이템 정보 비활성화
    {
        pickupActivated = false;                          // 아이템 줍기 불가능
        actionText.gameObject.SetActive(false);           // 아이템 정보 비활성화
    }
}
