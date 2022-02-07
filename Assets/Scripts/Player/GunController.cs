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
        // �� ���� �ʱ�ȭ
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

    // ������ �õ�
    public bool Reload()
    {
        if (state == State.Reloading || gunCurrentBulletCount >= gunReloadBulletCount)
        {
            return false;
        }

        StartCoroutine(ReloadRoutine());
        return true;
    }

    // ���� ������ ó���� ����
    IEnumerator ReloadRoutine()
    {
        if (gunCarryBulletCount > 0) //�������� �� ����ִ� �Ѿ��� �ִٸ�
        {
            state = State.Reloading;

            gunAudioPlayer.PlayOneShot(reloadClip);

            //animator.SetTrigger("Reload"); //�ִϸ��̼� ����� ���� Ʈ����

            gunCarryBulletCount += gunCurrentBulletCount; //������ �ϸ� �ϴ� ���� �����Ǿ��ִ� �Ѿ��� ����ִ� �Ѿ˿� �ְ�
            gunCurrentBulletCount = 0; //����ִ� �Ѿ��� 0�߷�

            yield return new WaitForSeconds(gunReloadTime); //���� ���� ������ �ð���ŭ ���

            if (gunCarryBulletCount >= gunReloadBulletCount) //���� ���� �������� �Ѿ˺��� ����ִ� �Ѿ��� ���ٸ�
            {
                gunCurrentBulletCount = gunReloadBulletCount; //�����ϱ� �������� �Ѿ˸�ŭ�� �������ϰ�
                gunCarryBulletCount -= gunReloadBulletCount; //���������� �׸�ŭ ����
            }
            else //�������� �Ѿ��� �ʹ� ���ٸ�
            {
                gunCurrentBulletCount = gunCarryBulletCount; //�ϴ� ��� �ִ� ��ŭ��
                gunCarryBulletCount = 0; //����ִ� ��ŭ ���������ϱ� ������ �Ѿ��� 0
            }

            // ���� ���� ���¸� �߻� �غ�� ���·� ����
            state = State.Ready;
        }
    }

}