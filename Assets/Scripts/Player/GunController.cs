using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun;

    private float currentFireRate;

    private bool isReload = false;

    // Start is called before the first frame update
    void Start()
    {
        currentGun.animator.SetTrigger("GameStart");
    }

    // Update is called once per frame
    void Update()
    {
        Fire();
    }

    void Fire()
    {
        if(currentFireRate > 0)
            currentFireRate -= Time.deltaTime;

        if(Input.GetButton("Fire1") && currentFireRate <=0 && !isReload)
        {
            currentGun.currentBulletCount--;
            currentFireRate = currentGun.fireRate;
            //currentGun.muzzleFlash.Play();
        }

        else if(Input.GetButtonUp("Fire1"))
        {

        }
    }
}
