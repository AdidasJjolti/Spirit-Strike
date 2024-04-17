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
        // �غ� ����, ��Ÿ��, �����, Ÿ�ټ�, ��ų ����, ��ų ��Ÿ�, ����ü �ӵ�, HP ȸ���� ����
        _stat = new SkillStat
        {
            _isReady = true,
            _coolDown = 30.0f,
            _damage = 0,               // �ʴ� �����
            _damageTarget = 0,
            _duration = 1f,           // 1�� ���� ����
            _castRange = 0f,
            _projectileSpeed = 0f,   // ��ǻ� ū �ǹ� ����
            _healAmount = 20
        };
    }

    void OnEnable()
    {
        StartCoroutine(SpellHeal());
    }

    // duration���� ��ų ����Ʈ�� ����
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
        Debug.Log("Heal���� ������");
        return Stat;
    }
}
