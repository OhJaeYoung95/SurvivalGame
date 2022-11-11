using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ScriptableObject : ���� ������Ʈ�� ������ �ʿ䰡 ���� ������Ʈ�� ����� ���ؼ� ���
// [CreateAssetMenu] : Projectâ���� ��Ŭ���� New Item�̶� â�� ����� item�̶� ���� ���ܼ� �������� ����� �ִ� ������ ����
[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    // Image Sprite ������
    // Image : Canvas �ȿ����� Ȱ�� ����
    // Sprite : ����󿡼��� �̹��� ��� ����

    public string itemName;                // ������ �̸�
    public ItemType itemType;              // ������ ����
    public Sprite itemImage;               // ������ �̹���
    public GameObject itemPrefab;          // ������ ������

    public string weaponType;              // ���� ����

    public enum ItemType                    // ������ Ÿ��
    {
        Equipment,                          // ���
        Used,                               // �Ҹ�ǰ
        Ingredient,                         // ���
        ETC                                 // ��Ÿ
    }
}
