using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Skill
{
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
        // 준비 상태, 쿨타임, 대미지, 타겟수, 스킬 범위, 스킬 사거리, 투사체 속도, HP 회복량 정의
        _stat = new SkillStat
        {
            _isReady = true,
            _coolDown = 30.0f,
            _damage = 0,               // 초당 대미지
            _damageTarget = 0,
            _duration = 1f,           // 1초 동안 유지
            _castRange = 0f,
            _projectileSpeed = 0f,   // 사실상 큰 의미 없음
            _healAmount = 20
        };
    }

    void OnEnable()
    {
        StartCoroutine(SpellHeal());
    }

    // duration동안 스킬 이펙트가 유지
    IEnumerator SpellHeal()
    {
        yield return new WaitForSeconds(_stat._duration);
        Destroy(this.gameObject);
    }

    void HealPlayer()
    {
        PlayerControl player = FindObjectOfType<PlayerControl>();

        if(player == null)
        {
            return;
        }

        player.Heal(_stat._healAmount);
    }

    public override SkillStat GetStat()
    {
        Debug.Log("Heal에서 구현함");
        return Stat;
    }
}
