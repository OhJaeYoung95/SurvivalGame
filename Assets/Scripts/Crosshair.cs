using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    // ũ�ν���� ���¿� ���� ���� ��Ȯ��
    private float gunAccuracy;

    // ũ�ν���� Ȱ��ȭ, ��Ȱ��ȭ�� ���� ����(�θ�ü)
    [SerializeField]
    private GameObject go_CrosshairHUD;
    [SerializeField]
    private GunController theGunController;

    public void WalkingAnimation(bool _flag)        // �Ȱ� ������ ũ�ν���� �ִϸ��̼� ����
    {
        WeaponManager.currentWeaponAnim.SetBool("Walk", _flag);
        animator.SetBool("Walking", _flag);
    }
    public void RunningAnimation(bool _flag)        // �ٰ� ������ ũ�ν���� �ִϸ��̼� ����
    {
        WeaponManager.currentWeaponAnim.SetBool("Run", _flag);
        animator.SetBool("Running", _flag);
    }
    public void JumpingAnimation(bool _flag)        // �ٰ� ������ ũ�ν���� �ִϸ��̼� ����
    {
        animator.SetBool("Running", _flag);
    }
    public void CrouchingAnimation(bool _flag)      // �ɾ����� ũ�ν���� �ִϸ��̼� ����
    {
        animator.SetBool("Crouching", _flag);
    }    
    public void FineSightAnimation(bool _flag)      // �ɾ����� ũ�ν���� �ִϸ��̼� ����
    {
        animator.SetBool("FineSight", _flag);
    }

    public void FireAnimation()                     // ���� ����ϴ� �ڼ��� ���� ũ�ν���� �ִϸ��̼� ����
    {
        if(animator.GetBool("Walking"))             // ������
            animator.SetTrigger("Walk_Fire");

        else if(animator.GetBool("Crouching"))      // ������
            animator.SetTrigger("Crouch_Fire");

        else
            animator.SetTrigger("Idle_Fire");       // ������ ������
    }

    public float GetAccuracy()          // ����ڼ��� ���� ���� ��Ȯ���� ��� �Լ�
    {
        if (animator.GetBool("Walking"))             // ������
            gunAccuracy = 0.06f;
        else if (animator.GetBool("Crouching"))      // ������
            gunAccuracy = 0.015f;
        else if (theGunController.GetFineSightMode())    // �����ؽ�
            gunAccuracy = 0.001f;
        else
            gunAccuracy = 0.035f;                         // ������ ������

        return gunAccuracy;

    }
}