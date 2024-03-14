using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootFire : Skill
{
    Vector3 _player;
    Vector3 _target;
    Rigidbody _rigid;

    // Ȱ��ȭ�Ǹ� �÷��̾�κ��� Ÿ�� ���� �������� ����ü �߻�
    void OnEnable()
    {
        // �غ� ����, ��Ÿ��, �����, Ÿ�ټ�, ��ų ����, ��ų ��Ÿ�, ����ü �ӵ�, HP ȸ���� ����
        SetSkill(true, 4.0f, 20, 1, 0.0f, 5.0f, 10.0f, 0.0f);
        Debug.Log($"��ų ���ݷ��� {_damage}��");

        _player = FindObjectOfType<PlayerControl>().transform.position;
        _target = FindObjectOfType<PlayerControl>().TargetEnemy.transform.position;
        _rigid = GetComponent<Rigidbody>();

        Vector3 dir = (_target - _player).normalized;
        _rigid.AddForce(dir * _projectileSpeed, ForceMode.Impulse);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(_damage);
            Destroy(this.gameObject);
        }
    }
}
