using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable //������ ���� �� �ִ� ��� ����� ��ӹ޾ƾ��ϴ� �������̽�
{
    //�ش� �������̽��� ��ӹ��� ������ �����ؾ��ϴ� �޼ҵ�
    //�ش� �޼ҵ�� �Է����� ���ط�(damage), ���� ����(hitPoint), ���� ǥ���� ����(hitNormal)�� �޴´�.
    void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);
}
