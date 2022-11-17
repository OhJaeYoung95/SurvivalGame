using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public string itemName;     // ������ �̸�(Ű��)
    public string[] part;         // ����
    public int[] num;             // ��ġ
}
public class ItemEffectDatabase : MonoBehaviour
{
    private ItemEffect[] itemEffects;       // ������ ���ȿ��

    // �ʿ��� ������Ʈ
    private StatusController thePlayerStatus;       // �÷��̾� �������ͽ�

    // ���ڸ� ���ȭ ���� switch case ���� ���� ���� �Է�
    private const string HP = "HP", SP = "SP", DP = "DP", HUNGRY = "HUNGRY", THIRSTY = "THIRSTY", SATISFY = "SATISFY";

    public void UseItem(Item _item)         // ������ ��� �Լ�
    {
        if(_item.itemType == Item.ItemType.Used)        // ������ Ÿ���� �Ҹ�ǰ�̶��
        {
            for (int x = 0; x < itemEffects.Length; x++)
            {
                // �ش� ������ ȿ���� ã�ư��� ����
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
