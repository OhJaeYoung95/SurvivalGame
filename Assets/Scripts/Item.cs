using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ScriptableObject : 게임 오브젝트에 부착할 필요가 없는 오브젝트를 만들기 위해서 사용
// [CreateAssetMenu] : Project창에서 우클릭시 New Item이란 창이 생기고 item이란 란이 생겨서 아이템을 만들수 있는 수단이 생김
// [CreateAssetMenu] : Project창에서 우클릭시 menuName 에 있는 New Item -> item 경로가 생김, 생성된 파일 이름은 fileName에 있는 New Item으로 생성
[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    // Image Sprite 차이점
    // Image : Canvas 안에서만 활용 가능
    // Sprite : 월드상에서도 이미지 출력 가능

    public string itemName;                // 아이템 이름
    [TextArea]      // 여러줄 사용 가능
    public string itemDesc;                // 아이템 설명
    public ItemType itemType;              // 아이템 유형
    public Sprite itemImage;               // 아이템 이미지
    public GameObject itemPrefab;          // 아이템 프리팹

    public GameObject kitPrefab;           // 키트 프리팹
    public GameObject kitPreviewPrefab;    // 키트 프리팹 미리보기

    public string weaponType;              // 무기 유형

    public enum ItemType                    // 아이템 타입
    {
        Equipment,                          // 장비
        Used,                               // 소모품
        Ingredient,                         // 재료
        Kit,                                // 키트
        ETC                                 // 기타
    }
}
