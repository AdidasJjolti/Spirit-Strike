using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : Skill
{
    Rigidbody _rigid;
    SkillStat _stat;


    public SkillStat Stat
    {
        get
        {
            return _stat;
        }
    }

    List<Enemy> _poisonousMonsters = new List<Enemy>();

    void Awake()
    {
        // 준비 상태, 쿨타임, 대미지, 타겟수, 스킬 범위, 스킬 사거리, 투사체 속도, HP 회복량 정의
        _stat = new SkillStat
        {
            _isReady = true,
            _coolDown = 25.0f,
            _damage = 20,               // 초당 대미지
            _damageTarget = 9999,
            _duration = 5.1f,           // 5초 동안 유지
            _castRange = 5.0f,
            _projectileSpeed = 10.0f,   // 사실상 큰 의미 없음
            _healAmount = 0
        };
    }

    void OnEnable()
    {
        StartCoroutine(SpellPoison());
        StartCoroutine(DealPoisonDamage());
    }

    // duration동안 스킬이 유지
    IEnumerator SpellPoison()
    {
        yield return new WaitForSeconds(_stat._duration);
        Destroy(this.gameObject);
    }

    // ToDo : 도트 대미지 적용 여부 체크
    IEnumerator DealPoisonDamage()
    {
        while(true)
        {
            yield return new WaitForSeconds(1.0f);
            foreach(var monster in _poisonousMonsters)
            {
                monster.TakeDamage(_stat._damage);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().IsPoisonous = true;
            _poisonousMonsters.Add(other.GetComponent<Enemy>());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().IsPoisonous = false;
            _poisonousMonsters.Remove(other.GetComponent<Enemy>());
        }
    }
}
