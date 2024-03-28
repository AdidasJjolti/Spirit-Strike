using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSlash : Skill
{
    Vector3 _player;

    protected SkillStat _stat;

    public SkillStat Stat
    {
        get
        {
            return _stat;
        }
    }

    [SerializeField] GameObject[] _projectiles;
    protected bool _isDistanceLimit;

    void Awake()
    {
        // �غ� ����, ��Ÿ��, �����, Ÿ�ټ�, ��ų ����, ��ų ��Ÿ�, ����ü �ӵ�, HP ȸ���� ����
        _stat = new SkillStat
        {
            _isReady = true,
            _coolDown = 6.0f,
            _damage = 20,
            _damageTarget = 1,
            _damageArea = 0.0f,
            _castRange = 5.0f,
            _projectileSpeed = 10.0f,
            _healAmount = 0
        };
    }

    // Ȱ��ȭ�Ǹ� 3�������� ����ü �߻�
    protected virtual void OnEnable()
    {
        //Debug.Log("�θ� OnEnable");

        for(int i = 0; i < _projectiles.Length; i++)
        {
            _projectiles[i].SetActive(true);
        }

        Debug.Log($"��ų ���ݷ��� {_stat._damage}��");

        Transform playerTr = FindObjectOfType<PlayerControl>().transform;
        _player = playerTr.position;
        Vector3 dir0 = playerTr.forward;

        _projectiles[0].GetComponent<Rigidbody>().AddForce(dir0 * _stat._projectileSpeed, ForceMode.Impulse);

        var quaternion1 = Quaternion.Euler(0, 15f, 0);
        Vector3 dir1 = quaternion1 * dir0;
        _projectiles[1].GetComponent<Rigidbody>().AddForce(dir1 * _stat._projectileSpeed, ForceMode.Impulse);

        var quaternion2 = Quaternion.Euler(0, 345f, 0);
        Vector3 dir2 = quaternion2 * dir0;
        _projectiles[2].GetComponent<Rigidbody>().AddForce(dir2 * _stat._projectileSpeed, ForceMode.Impulse);
    }

    public override SkillStat GetStat()
    {
        Debug.Log("FireSlash���� ������");
        return Stat;
    }
}
