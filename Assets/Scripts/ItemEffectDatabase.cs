using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public string itemName;     // 아이템 이름(키값)
    [Tooltip("HP, SP, DP, HUNGRY, THIRSTY, SATISFY만 가능합니다")]
    public string[] part;         // 부위
    public int[] num;             // 수치
}
public class ItemEffectDatabase : MonoBehaviour
{
    [SerializeField]
    private ItemEffect[] itemEffects;       // 아이템 사용효과

    // 필요한 컴포넌트
    [SerializeField]
    private StatusController thePlayerStatus;       // 플레이어 스테이터스
    [SerializeField]
    private WeaponManager theWeaponManager;         // 총 장착을 위한 변수
    [SerializeField]
    private SlotToolTip theSlotToolTip;             // 아이템 툴팁 창을 다루기 위한 변수
    [SerializeField]
    private QuickSlotController theQuickSlotController;     //

    // 문자를 상수화 시켜 switch case 문에 보다 쉽게 입력
    private const string HP = "HP", SP = "SP", DP = "DP", HUNGRY = "HUNGRY", THIRSTY = "THIRSTY", SATISFY = "SATISFY";

    // QuickSlotController 징검다리 역할
    public void IsActivatedQuickSlot(int _num)
    {
        theQuickSlotController.IsActivatedQuickSlot(_num);
    }


    //슬롯은 프리팹이라 외부 자원을 가져올때 메모리 낭비가 심해서
    //이렇게 이중호출을 해서 메모리 낭비를 줄인다
    // SlotTooltip 징검다리 역할 함수
    public void ShowToolTip(Item _item, Vector3 _pos)     // 툴팁 활성화
    {
        theSlotToolTip.ShowToolTip(_item, _pos);
    }

    // SlotTooltip 징검다리 역할 함수
    public void HideToolTip()               // 툴팁 비활성화
    {
        theSlotToolTip.HideToolTip();
    }

    public void UseItem(Item _item)         // 아이템 사용 함수
    {
        if (_item.itemType == Item.ItemType.Equipment)        // 장비 아이템이라면
        {
            // 장착
            StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(_item.weaponType, _item.itemName)); // WeaponManager에 무기교체 코루틴 함수 실행
        }
        else if (_item.itemType == Item.ItemType.Used)        // 아이템 타입이 소모품이라면
        {
            for (int x = 0; x < itemEffects.Length; x++)
            {
                // 해당 아이템 효과를 찾아가는 과정
                if(itemEffects[x].itemName == _item.itemName)
                {
                    for(int y =0; y < itemEffects[x].part.Length; y++)
                    {
                        switch(itemEffects[x].part[y])                  // 아이템 종류에 따른 부위별 스테이터스 증가
                        {
                            case HP:
                                thePlayerStatus.IncreaseHP(itemEffects[x].num[y]);
                                break;
                            case SP:
                                thePlayerStatus.IncreaseSP(itemEffects[x].num[y]);
                                break;
                            case DP:
                                thePlayerStatus.IncreaseDP(itemEffects[x].num[y]);
                                break;
                            case HUNGRY:
                                thePlayerStatus.IncreaseHungry(itemEffects[x].num[y]);
                                break;
                            case THIRSTY:
                                thePlayerStatus.IncreaseThirsty(itemEffects[x].num[y]);
                                break;
                            case SATISFY:
                                break;
                            default:
                                Debug.Log("잘못된 Status 부위 HP, SP, DP, HUNGRY, THIRSTY, SATISFY만 가능합니다");
                                break;
                        }
                        Debug.Log(_item.itemName + " 을 사용했습니다");
                    }
                    return;
                }
            }
            Debug.Log("ItemEffectDatabase에 일치하는itemName 없습니다");
        }
    }
 }
