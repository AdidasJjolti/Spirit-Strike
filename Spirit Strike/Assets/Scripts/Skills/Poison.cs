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
            _damage = 5,                // 초당 대미지
            _damageTarget = 9999,
            _duration = 4.9f,           // 5초 동안 유지
            _castRange = 5.0f,
            _projectileSpeed = 10.0f,   // 사실상 큰 의미 없음
            _healAmount = 0
        };
    }

    void OnEnable()
    {
        StartCoroutine(DealPoisonDamage());
    }

    void OnDestroy()
    {
        StopCoroutine(DealPoisonDamage());
    }

    // duration동안 스킬이 유지
    IEnumerator SpellPoison()
    {
        yield return new WaitForSeconds(_stat._duration);
        //StopCoroutine(DealPoisonDamage());
        Destroy(this.gameObject);
    }

    // 1초마다 범위 내 몬스터에게 도트 대미지 적용
    IEnumerator DealPoisonDamage()
    {
        float elapsedTime = 0.0f;
        int time = 1;

        while (elapsedTime < _stat._duration)
        {
            Debug.Log($"{time}번째 대미지");

            yield return new WaitForSeconds(1.0f);
            elapsedTime += 1.0f;

            foreach (var monster in _poisonousMonsters)
            {
                // 이미 죽은 몬스터를 체크하려고 할 때 리스트에서 제거
                if (monster == null)
                {
                    continue;
                }
                else
                {
                    monster.TakeDamage(_stat._damage);
                }
            }

            time++;
        }

        Destroy(this.gameObject);
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

    public override SkillStat GetStat()
    {
        Debug.Log("Poison에서 구현함");
        return Stat;
    }
}
