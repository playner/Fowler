using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    [SerializeField]
    private Gun currentGun;
    
    private bool isReload = false;

    

    // Start is called before the first frame update
    void Start()
    {
        //초기 건 Init
        currentGun = ChoiceGun.ReturnGunType(GunType.AR);
    }

    // Update is called once per frame
    void Update()
    {
        InputController();
    }

    void InputController()
    {
        if (Input.GetButton("Fire1"))
        {
            currentGun.GunFire();
        }
        else if (Input.GetButtonUp("Fire1"))
        {
           
        }
    }
    
}

#region TODO : 나중에 클래스로 따로 분리 부탁함

public class ChoiceGun
{
    public static Gun ReturnGunType(GunType gunType)
    {
        Gun gunTemp = null;
        switch(gunType)
        {
            case GunType.AR:
                gunTemp = new AR();
                gunTemp.gunInfo = gunTemp.GetGunInfoInit;
                break;
            case GunType.SG:
                gunTemp = new SG();
                gunTemp.gunInfo = gunTemp.GetGunInfoInit;
                break;
            case GunType.SN:
                gunTemp = new SN();
                gunTemp.gunInfo = gunTemp.GetGunInfoInit;
                break;
        }
        return gunTemp;
    }
}

public class  AR : Gun
{    
    //실제 정보 애가 전역변수요
    public GunInfo gunInfo
    {
        get; set;
    }
    public GunInfo GetGunInfoInit
    {
        get
        {
            GunInfo tempGunInfo = null;
            GunType gunType = GunType.AR;
            string gunName = "M4A1";
            float gunRange = 100;
            float gunAccuracy = 0.01f;
            float gunFireRate = 0.2f;
            float gunReloadTime = 2;
            float gunDamage = 15;
            int gunReloadBulletCount = 30;
            int gunCurrentBulletCount = 30;
            int gunMaxBulletCount = 150;
            int gunCarryBulletCount = 150;
            float gunRetroActionForce = 0.3f;
            float gunRetroActionFineSightForce = 0.7f;
            Vector3 gunFineSightOriginPos;
            Animator gunAnimator = Resources.Load<Animator>("Animation/Soldier");
            ParticleSystem gunMuzzleFlash = Resources.Load<ParticleSystem>("Prefabs/MuzzleFlash");
            AudioSource gunFireSound = Resources.Load<AudioSource>("Assets/Free Pack/Hand Gun 1");

            return tempGunInfo;
        }
    }

   public void GunFire()
   {
        if (gunInfo.gunFireRate > 0)
            gunInfo.gunFireRate--;
   
        gunInfo.gunCurrentBulletCount--;
        currentFireRate = gunInfo.gunFireRate;
        gunInfo.gunMuzzleFlash.Play();
   }
}
public class SN : Gun
{
    //실제 정보
    public GunInfo gunInfo
    {
        get;set;
    }
    //
    //총 정보 호출시 초기화
    public GunInfo GetGunInfoInit
    {
        get
        {
            GunInfo tempGunInfo = null;
            GunType gunType = GunType.SN;
            string gunName = "M24";
            float gunRange = 500;
            float gunAccuracy = 0.01f;
            float gunFireRate = 3;
            float gunReloadTime = 5;
            float gunDamage = 80;
            int gunReloadBulletCount = 5;
            int gunCurrentBulletCount = 5;
            int gunMaxBulletCount = 20;
            int gunCarryBulletCount = 20;
            float gunRetroActionForce = 0.3f;
            float gunRetroActionFineSightForce = 0.7f;
            Vector3 gunFineSightOriginPos ;
            Animator gunAnimator = Resources.Load<Animator>("Animation/Soldier");
            ParticleSystem gunMuzzleFlash = Resources.Load<ParticleSystem>("Prefabs/MuzzleFlash");
            AudioSource gunFireSound = Resources.Load<AudioSource>("Assets/Free Pack/Hand Gun 1");

            return tempGunInfo;
        }
    }
    public void GunFire()
    {
        //if (GunFireRate > 0)
        GetGunInfoInit.gunFireRate -= Time.deltaTime;
    }
}


public class SG : Gun
{
    //실제 정보
    public GunInfo gunInfo
    {
        get; set;
    }
    public GunInfo GetGunInfoInit { get;}
    public void GunFire()
    {
        //if (currentFireRate > 0)
         //   currentFireRate -= Time.deltaTime;
    }
}

#endregion