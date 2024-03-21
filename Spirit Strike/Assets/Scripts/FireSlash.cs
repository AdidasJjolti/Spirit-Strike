using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSlash : Skill
{
    Vector3 _player;

    SkillStat _stat;

    public SkillStat Stat
    {
        get
        {
            return _stat;
        }
    }

    [SerializeField] GameObject[] _projectiles;

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
            _projectileSpeed = 1.0f,
            _healAmount = 0
        };
    }

    // Ȱ��ȭ�Ǹ� 3�������� ����ü �߻�
    void OnEnable()
    {
        Debug.Log($"��ų ���ݷ��� {_stat._damage}��");

        Transform playerTr = FindObjectOfType<PlayerControl>().transform;
        //_player = playerTr.position;

        //_projectiles[0].GetComponent<Rigidbody>().AddForce(playerTr.forward * _stat._projectileSpeed, ForceMode.Impulse);

        //_projectiles[1].transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 45f, 0));
        //_projectiles[1].GetComponent<Rigidbody>().AddForce(playerTr.forward * _stat._projectileSpeed, ForceMode.Impulse);

        //_projectiles[2].transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(-0, -45f, 0));
        //_projectiles[2].GetComponent<Rigidbody>().AddForce(Vector3.forward * _stat._projectileSpeed, ForceMode.Impulse);

        _player = playerTr.position;

        Vector3 dir0 = playerTr.forward;
        Debug.Log($"�÷��̾� ���� ��ǥ�� {dir0}");

        _projectiles[0].GetComponent<Rigidbody>().AddForce(dir0 * _stat._projectileSpeed, ForceMode.Impulse);

        var quaternion1 = Quaternion.Euler(0, 75f, 0);
        Vector3 dir1 = quaternion1 * dir0;
        _projectiles[1].GetComponent<Rigidbody>().AddForce(dir1 * _stat._projectileSpeed, ForceMode.Impulse);

        var quaternion2 = Quaternion.Euler(0, -75f, 0);
        Vector3 dir2 = quaternion2 * dir0;
        _projectiles[2].GetComponent<Rigidbody>().AddForce(dir2 * _stat._projectileSpeed, ForceMode.Impulse);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(_stat._damage);
            Destroy(this.gameObject);
        }
    }
}
