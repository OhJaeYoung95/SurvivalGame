using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public string itemName;     // ������ �̸�(Ű��)
    [Tooltip("HP, SP, DP, HUNGRY, THIRSTY, SATISFY�� �����մϴ�")]
    public string[] part;         // ����
    public int[] num;             // ��ġ
}
public class ItemEffectDatabase : MonoBehaviour
{
    [SerializeField]
    private ItemEffect[] itemEffects;       // ������ ���ȿ��

    // �ʿ��� ������Ʈ
    [SerializeField]
    private StatusController thePlayerStatus;       // �÷��̾� �������ͽ�
    [SerializeField]
    private WeaponManager theWeaponManager;         // �� ������ ���� ����
    [SerializeField]
    private SlotToolTip theSlotToolTip;             // ������ ���� â�� �ٷ�� ���� ����
    [SerializeField]
    private QuickSlotController theQuickSlotController;     //

    // ���ڸ� ���ȭ ���� switch case ���� ���� ���� �Է�
    private const string HP = "HP", SP = "SP", DP = "DP", HUNGRY = "HUNGRY", THIRSTY = "THIRSTY", SATISFY = "SATISFY";

    // QuickSlotController ¡�˴ٸ� ����
    public void IsActivatedQuickSlot(int _num)
    {
        theQuickSlotController.IsActivatedQuickSlot(_num);
    }


    //������ �������̶� �ܺ� �ڿ��� �����ö� �޸� ���� ���ؼ�
    //�̷��� ����ȣ���� �ؼ� �޸� ���� ���δ�
    // SlotTooltip ¡�˴ٸ� ���� �Լ�
    public void ShowToolTip(Item _item, Vector3 _pos)     // ���� Ȱ��ȭ
    {
        theSlotToolTip.ShowToolTip(_item, _pos);
    }

    // SlotTooltip ¡�˴ٸ� ���� �Լ�
    public void HideToolTip()               // ���� ��Ȱ��ȭ
    {
        theSlotToolTip.HideToolTip();
    }

    public void UseItem(Item _item)         // ������ ��� �Լ�
    {
        if (_item.itemType == Item.ItemType.Equipment)        // ��� �������̶��
        {
            // ����
            StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(_item.weaponType, _item.itemName)); // WeaponManager�� ���ⱳü �ڷ�ƾ �Լ� ����
        }
        else if (_item.itemType == Item.ItemType.Used)        // ������ Ÿ���� �Ҹ�ǰ�̶��
        {
            for (int x = 0; x < itemEffects.Length; x++)
            {
                // �ش� ������ ȿ���� ã�ư��� ����
                if(itemEffects[x].itemName == _item.itemName)
                {
                    for(int y =0; y < itemEffects[x].part.Length; y++)
                    {
                        switch(itemEffects[x].part[y])                  // ������ ������ ���� ������ �������ͽ� ����
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
                                Debug.Log("�߸��� Status ���� HP, SP, DP, HUNGRY, THIRSTY, SATISFY�� �����մϴ�");
                                break;
                        }
                        Debug.Log(_item.itemName + " �� ����߽��ϴ�");
                    }
                    return;
                }
            }
            Debug.Log("ItemEffectDatabase�� ��ġ�ϴ�itemName �����ϴ�");
        }
    }
 }
