using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Skill
{
    Vector3 _player;
    Vector3 _target;
    Rigidbody _rigid;

    SkillStat _stat;

    public SkillStat Stat
    {
        get
        {
            return _stat;
        }
    }

    void Awake()
    {
        // �غ� ����, ��Ÿ��, �����, Ÿ�ټ�, ��ų ����, ��ų ��Ÿ�, ����ü �ӵ�, HP ȸ���� ����
        _stat = new SkillStat
        {
            _isReady = true,
            _coolDown = 4.0f,
            _damage = 20,
            _damageTarget = 1,
            _damageArea = 0.0f,
            _castRange = 5.0f,
            _projectileSpeed = 10.0f,
            _healAmount = 0
        };
    }

    // Ȱ��ȭ�Ǹ� �÷��̾�κ��� Ÿ�� ���� �������� ����ü �߻�
    void OnEnable()
    {
        Debug.Log($"��ų ���ݷ��� {_stat._damage}��");

        _player = FindObjectOfType<PlayerControl>().transform.position;
        _target = FindObjectOfType<PlayerControl>().TargetEnemy.transform.position;
        _rigid = GetComponent<Rigidbody>();

        Vector3 dir = (_target - _player).normalized;
        _rigid.AddForce(dir * _stat._projectileSpeed, ForceMode.Impulse);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(_stat._damage);
            Destroy(this.gameObject);
        }
    }

    public override SkillStat GetStat()
    {
        Debug.Log("Fireball���� ������");
        return Stat;
    }
}
