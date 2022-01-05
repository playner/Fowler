using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunType { AR, SN, SG }
public class GunInfo
{
    public GunType gunType;
    public string gunName;
    public float gunRange;
    public float gunAccuracy;
    public float gunFireRate;
    public float gunReloadTime;
    public float gunDamage;
    public int gunReloadBulletCount;
    public int gunCurrentBulletCount;
    public int gunMaxBulletCount;
    public int gunCarryBulletCount;
    public float gunRetroActionForce;
    public float gunRetroActionFineSightForce;
    public Vector3 gunFineSightOriginPos;
    public Animator gunAnimator;
    public ParticleSystem gunMuzzleFlash;
    public AudioSource gunFireSound;
    
}

public interface Gun
{
    GunInfo GetGunInfoInit { get; }
    GunInfo gunInfo { get; set; }
    void GunFire();
}
