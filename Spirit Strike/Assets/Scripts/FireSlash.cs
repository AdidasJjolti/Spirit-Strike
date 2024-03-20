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
        // 준비 상태, 쿨타임, 대미지, 타겟수, 스킬 범위, 스킬 사거리, 투사체 속도, HP 회복량 정의
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

    // 활성화되면 3방향으로 투사체 발사
    void OnEnable()
    {
        Debug.Log($"스킬 공격력은 {_stat._damage}야");

        _player = FindObjectOfType<PlayerControl>().transform.position;
        _projectiles[0].GetComponent<Rigidbody>().AddForce(Vector3.forward * _stat._projectileSpeed, ForceMode.Impulse);

        _projectiles[1].transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, -45f));
        _projectiles[1].GetComponent<Rigidbody>().AddForce(Vector3.forward * _stat._projectileSpeed, ForceMode.Impulse);

        _projectiles[2].transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, 45f));
        _projectiles[2].GetComponent<Rigidbody>().AddForce(Vector3.forward * _stat._projectileSpeed, ForceMode.Impulse);
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
