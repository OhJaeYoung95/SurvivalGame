using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    // Static = 공유 자원, 클래스 변수 = 정적 변수 , 쉽게 접근이 가능하지만 그만큼 보호수준이 떨어지고 메모리가 낭비된다.
    // 무기 중복 교체 실행 방지를 위한 bool형 변수
    public static bool isChangeWeapon = false;

    // 현재 무기와 현재 무기의 애니메이션
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;

    // 현재 무기의 타입
    [SerializeField]
    private string currentWeaponType;

    // 무기 교체 딜레이 타임, 무기 교체가 완전히 끝난 시점.
    [SerializeField]
    private float changeWeaponDelayTime;
    [SerializeField]
    private float changeWeaponEndDelayTime;


    // 무기 종류들 관리하는 배열변수
    [SerializeField]
    private Gun[] guns;
    [SerializeField]
    private CloseWeapon[] hands;
    [SerializeField]
    private CloseWeapon[] axes;
    [SerializeField]
    private CloseWeapon[] pickaxes;

    // 관리 차원에서 쉽게 무기 접근이 가능하도록 만듦
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, CloseWeapon> handDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> axeDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> pickaxeDictionary = new Dictionary<string, CloseWeapon>();

    //필요한 컴포넌트
    // 무기 타입에 따라서 효과를 바꿔주기 위해서 넣어주는 변수 (On, Off)
    [SerializeField]
    private GunController theGunController;
    [SerializeField]
    private HandController theHandController;
    [SerializeField]
    private AxeController theAxeController;
    [SerializeField]
    private PickaxeController thePickaxeController;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }
        for (int i = 0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].closeWeaponName, hands[i]);
        }       
        for (int i = 0; i < axes.Length; i++)
        {
            axeDictionary.Add(axes[i].closeWeaponName, axes[i]);
        }
        for (int i = 0; i < pickaxes.Length; i++)
        {
            pickaxeDictionary.Add(pickaxes[i].closeWeaponName, pickaxes[i]);
        }
    }

    public IEnumerator ChangeWeaponCoroutine(string _type, string name)     // 무기를 바꿔주는 코루틴 함수
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out");     // 손을 집어넣어서 무기를 꺼내는 애니메이션 실행

        yield return new WaitForSeconds(changeWeaponDelayTime);     // 무기 교체, 무기를 꺼낼려고 손을 집어넣는 딜레이 시간

        CancelPreWeaponAction();                // 실행중이던 액션 취소해주는 함수
        WeaponChange(_type, name);              // 원하는 무기로 바꿔주는 함수

        yield return new WaitForSeconds(changeWeaponEndDelayTime);      // 무기 교체 ,무기를 꺼내는 애니메이션 딜레이 시간

        currentWeaponType = _type;
        isChangeWeapon = false;
    }

    private void CancelPreWeaponAction()    // 실행중이던 무기액션 취소해주는 함수
    {
        switch(currentWeaponType)
        {
            case "GUN":
                theGunController.CancelFineSight();     // 정조준 취소
                theGunController.CancelReload();        // 재장전 취소
                GunController.isActivate = false;       // 비활성화
                break;
            case "HAND":
                HandController.isActivate = false;      // 비활성화
                if (HandController.currentKit != null)       // 키트를 들고 있을때
                    theHandController.Cancel();     // 외부에서 키트 미리보기 초기화해주는 함수
                if (QuickSlotController.go_HandItem != null)        // 손으로 교체시 아이템이 있다면
                    Destroy(QuickSlotController.go_HandItem);       // 아이템 제거
                break;
            case "AXE":
                AxeController.isActivate = false;      // 비활성화
                break;
            case "PICKAXE":
                PickaxeController.isActivate = false;      // 비활성화
                break;
        }
    }

    private void WeaponChange(string _type, string _name)
    {
        if(_type == "GUN")
            theGunController.GunChange(gunDictionary[_name]);
        else if(_type == "HAND")
            theHandController.CloseWeaponChange(handDictionary[_name]);
        else if(_type == "AXE")
            theAxeController.CloseWeaponChange(axeDictionary[_name]);
        else if(_type == "PICKAXE")
            thePickaxeController.CloseWeaponChange(pickaxeDictionary[_name]);
    }

    public IEnumerator WeaponInCoroutine()         // 무기 꺼내는 코루틴
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out");     // 손을 집어넣어서 무기를 꺼내는 애니메이션 실행

        yield return new WaitForSeconds(changeWeaponDelayTime);

        currentWeapon.gameObject.SetActive(false);
    }
    public void WeaponOut()         // 무기 집어넣는 코루틴
    {
        isChangeWeapon = false;

        currentWeapon.gameObject.SetActive(true);
    }
}
