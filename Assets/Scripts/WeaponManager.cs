using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    // Static = ���� �ڿ�, Ŭ���� ���� = ���� ���� , ���� ������ ���������� �׸�ŭ ��ȣ������ �������� �޸𸮰� ����ȴ�.
    // ���� �ߺ� ��ü ���� ������ ���� bool�� ����
    public static bool isChangeWeapon = false;

    // ���� ����� ���� ������ �ִϸ��̼�
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;

    // ���� ������ Ÿ��
    [SerializeField]
    private string currentWeaponType;

    // ���� ��ü ������ Ÿ��, ���� ��ü�� ������ ���� ����.
    [SerializeField]
    private float changeWeaponDelayTime;
    [SerializeField]
    private float changeWeaponEndDelayTime;


    // ���� ������ �����ϴ� �迭����
    [SerializeField]
    private Gun[] guns;
    [SerializeField]
    private CloseWeapon[] hands;
    [SerializeField]
    private CloseWeapon[] axes;
    [SerializeField]
    private CloseWeapon[] pickaxes;

    // ���� �������� ���� ���� ������ �����ϵ��� ����
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, CloseWeapon> handDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> axeDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> pickaxeDictionary = new Dictionary<string, CloseWeapon>();

    //�ʿ��� ������Ʈ
    // ���� Ÿ�Կ� ���� ȿ���� �ٲ��ֱ� ���ؼ� �־��ִ� ���� (On, Off)
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

    // Update is called once per frame
    void Update()
    {
        if (!Inventory.inventoryActivated)       // �κ��丮 ��Ȱ��ȭ�ÿ���
        {
            if (!isChangeWeapon)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))    // ���� ��ü ���� (�Ǽ�)
                    StartCoroutine(ChangeWeaponCoroutine("HAND", "�Ǽ�"));
                else if (Input.GetKeyDown(KeyCode.Alpha2))    // ���� ��ü ���� (����ӽŰ�)
                    StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));
                else if (Input.GetKeyDown(KeyCode.Alpha3))    // ���� ��ü ���� (����)
                    StartCoroutine(ChangeWeaponCoroutine("AXE", "Axe"));
                else if (Input.GetKeyDown(KeyCode.Alpha4))    // ���� ��ü ���� (���)
                    StartCoroutine(ChangeWeaponCoroutine("PICKAXE", "Pickaxe"));
            }
        }
    }

    public IEnumerator ChangeWeaponCoroutine(string _type, string name)     // ���⸦ �ٲ��ִ� �ڷ�ƾ �Լ�
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out");     // ���� ����־ ���⸦ ������ �ִϸ��̼� ����

        yield return new WaitForSeconds(changeWeaponDelayTime);     // ���� ��ü, ���⸦ �������� ���� ����ִ� ������ �ð�

        CancelPreWeaponAction();                // �������̴� �׼� ������ִ� �Լ�
        WeaponChange(_type, name);              // ���ϴ� ����� �ٲ��ִ� �Լ�

        yield return new WaitForSeconds(changeWeaponEndDelayTime);      // ���� ��ü ,���⸦ ������ �ִϸ��̼� ������ �ð�

        currentWeaponType = _type;
        isChangeWeapon = false;
    }

    private void CancelPreWeaponAction()    // �������̴� �׼� ������ִ� �Լ�
    {
        switch(currentWeaponType)
        {
            case "GUN":
                theGunController.CancelFineSight();     // ������ ���
                theGunController.CancelReload();        // ������ ���
                GunController.isActivate = false;       // ��Ȱ��ȭ
                break;
            case "HAND":
                HandController.isActivate = false;      // ��Ȱ��ȭ
                break;
            case "AXE":
                AxeController.isActivate = false;      // ��Ȱ��ȭ
                break;
            case "PICKAXE":
                PickaxeController.isActivate = false;      // ��Ȱ��ȭ
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
}
