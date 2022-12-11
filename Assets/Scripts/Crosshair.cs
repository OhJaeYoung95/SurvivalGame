using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    // 크로스헤어 상태에 따른 총의 정확도
    private float gunAccuracy;

    // 크로스헤어 활성화, 비활성화를 위한 변수(부모객체)
    [SerializeField]
    private GameObject go_CrosshairHUD;
    [SerializeField]
    private GunController theGunController;

    public void WalkingAnimation(bool _flag)        // 걷고 있을때 크로스헤어 애니메이션 실행
    {
        if(!GameManager.isWater)
        {
            WeaponManager.currentWeaponAnim.SetBool("Walk", _flag);
            animator.SetBool("Walking", _flag);
        }
    }
    public void RunningAnimation(bool _flag)        // 뛰고 있을때 크로스헤어 애니메이션 실행
    {
        if (!GameManager.isWater)
        {
            WeaponManager.currentWeaponAnim.SetBool("Run", _flag);
            animator.SetBool("Running", _flag);
        }
    }
    public void JumpingAnimation(bool _flag)        // 뛰고 있을때 크로스헤어 애니메이션 실행
    {
        if (!GameManager.isWater)
        {
            animator.SetBool("Running", _flag);
        }
    }
    public void CrouchingAnimation(bool _flag)      // 앉았을때 크로스헤어 애니메이션 실행
    {
        if (!GameManager.isWater)
        {
            animator.SetBool("Crouching", _flag);
        }
    }    
    public void FineSightAnimation(bool _flag)      // 앉았을때 크로스헤어 애니메이션 실행
    {
        if (!GameManager.isWater)
        {
            animator.SetBool("FineSight", _flag);
        }
    }

    public void FireAnimation()                     // 총을 사격하는 자세에 따른 크로스헤어 애니메이션 실행
    {
        if (!GameManager.isWater)
        {
            if (animator.GetBool("Walking"))             // 걸을때
                animator.SetTrigger("Walk_Fire");

            else if (animator.GetBool("Crouching"))      // 앉을때
                animator.SetTrigger("Crouch_Fire");

            else
                animator.SetTrigger("Idle_Fire");       // 가만히 있을때
        }
    }

    public float GetAccuracy()          // 사격자세에 따른 총의 정확도를 얻는 함수
    {
        if (animator.GetBool("Walking"))             // 걸을때
            gunAccuracy = 0.06f;
        else if (animator.GetBool("Crouching"))      // 앉을때
            gunAccuracy = 0.015f;
        else if (theGunController.GetFineSightMode())    // 정조준시
            gunAccuracy = 0.001f;
        else
            gunAccuracy = 0.035f;                         // 가만히 있을때

        return gunAccuracy;

    }
}
