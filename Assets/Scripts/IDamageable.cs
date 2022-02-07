using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable //데미지 입을 수 있는 모든 대상이 상속받아야하는 인터페이스
{
    //해당 인터페이스를 상속받은 대상들이 구현해야하는 메소드
    //해당 메소드는 입력으로 피해량(damage), 맞은 지점(hitPoint), 맞은 표면의 방향(hitNormal)을 받는다.
    void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);
}
