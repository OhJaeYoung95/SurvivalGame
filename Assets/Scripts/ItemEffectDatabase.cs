using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public string itemName;     // 아이템 이름(키값)
    public string[] part;         // 부위
    public int[] num;             // 수치
}
public class ItemEffectDatabase : MonoBehaviour
{
    private ItemEffect[] itemEffects;       // 아이템 사용효과

    // 필요한 컴포넌트
    private StatusController thePlayerStatus;       // 플레이어 스테이터스

    // 문자를 상수화 시켜 switch case 문에 보다 쉽게 입력
    private const string HP = "HP", SP = "SP", DP = "DP", HUNGRY = "HUNGRY", THIRSTY = "THIRSTY", SATISFY = "SATISFY";

    public void UseItem(Item _item)         // 아이템 사용 함수
    {
        if(_item.itemType == Item.ItemType.Used)        // 아이템 타입이 소모품이라면
        {
            for (int x = 0; x < itemEffects.Length; x++)
            {
                // 해당 아이템 효과를 찾아가는 과정
                if(itemEffects[x].itemName == _item.itemName)
                {
                    for(int y =0; y < itemEffects[x].part.Length; y++)
                    {
                        switch(itemEffects[x].part[y])
                        {
                            case HP:
                                break;
                            case SP:
                                break;
                            case DP:
                                break;
                            case HUNGRY:
                                break;
                            case THIRSTY:
                                break;
                            case SATISFY:
                                break;
                        }
                    }
                }
            }
        }
    }
 }
