using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public enum State
    {
        Ready,
        Empty,
        Reloading
    }

    public State state { get; private set; }

    public Transform fireTransform;
    public Transform emptyShellTransform;

    public ParticleSystem muzzleFlashEffect;
    public ParticleSystem shellEjectEffect;

    private AudioSource gunAudioPlayer;
    public AudioClip shotClip;
    public AudioClip reloadClip;
    public Animator animator;

    public float gunDamage;
    public float gunRange;
    public float gunAccuracy;
    public float gunFireRate;
    public float gunReloadTime;
    public float gunLastFireTime;

    public int gunReloadBulletCount = 5;
    public int gunCurrentBulletCount = 5;
    public int gunMaxBulletCount = 20;
    public int gunCarryBulletCount = 20;

    void Awake()
    {
        gunAudioPlayer = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // 총 상태 초기화
        gunCurrentBulletCount = gunReloadBulletCount;
        state = State.Ready;
        gunLastFireTime = 0;
    }

    public void Fire()
    {
        if (state == State.Ready && Time.time >= gunLastFireTime + gunFireRate)
        {
            gunLastFireTime = Time.time;

            Shot();
        }
    }

    private void Shot()
    {
        RaycastHit hit;

        Vector3 hitPosition = Vector3.zero;

        if (Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, gunRange))
        {
            IDamageable target = hit.collider.GetComponent<IDamageable>();

            if (target != null)
            {
                target.OnDamage(gunDamage, hit.point, hit.normal);
            }

            hitPosition = hit.point;
        }

        else
        {
            hitPosition = fireTransform.position + fireTransform.forward * gunRange;
        }

        ShotEffect();

        gunCurrentBulletCount--;

        if (gunCurrentBulletCount <= 0)
        {
            state = State.Empty;
        }
    }

    private void ShotEffect()
    {
        muzzleFlashEffect.Play();

        shellEjectEffect.Play();

        gunAudioPlayer.PlayOneShot(shotClip);
    }

    // 재장전 시도
    public bool Reload()
    {
        if (state == State.Reloading || gunCurrentBulletCount >= gunReloadBulletCount)
        {
            return false;
        }

        StartCoroutine(ReloadRoutine());
        return true;
    }

    // 실제 재장전 처리를 진행
    IEnumerator ReloadRoutine()
    {
        if (gunCarryBulletCount > 0) //재장전할 때 들고있는 총알이 있다면
        {
            state = State.Reloading;

            gunAudioPlayer.PlayOneShot(reloadClip);

            //animator.SetTrigger("Reload"); //애니메이션 재생을 위한 트리거

            gunCarryBulletCount += gunCurrentBulletCount; //재장전 하면 일단 현재 장전되어있는 총알을 들고있는 총알에 넣고
            gunCurrentBulletCount = 0; //들고있는 총알은 0발로

            yield return new WaitForSeconds(gunReloadTime); //현재 총의 재장전 시간만큼 대기

            if (gunCarryBulletCount >= gunReloadBulletCount) //현재 총의 재장전할 총알보다 들고있는 총알이 많다면
            {
                gunCurrentBulletCount = gunReloadBulletCount; //많으니까 재장전할 총알만큼만 재장전하고
                gunCarryBulletCount -= gunReloadBulletCount; //나머지에서 그만큼 빼줌
            }
            else //재장전할 총알이 너무 적다면
            {
                gunCurrentBulletCount = gunCarryBulletCount; //일단 들고 있는 만큼만
                gunCarryBulletCount = 0; //들고있는 만큼 장전했으니까 나머지 총알은 0
            }

            // 총의 현재 상태를 발사 준비된 상태로 변경
            state = State.Ready;
        }
    }

}