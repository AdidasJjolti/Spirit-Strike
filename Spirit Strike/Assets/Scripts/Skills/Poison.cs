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
        // �غ� ����, ��Ÿ��, �����, Ÿ�ټ�, ��ų ����, ��ų ��Ÿ�, ����ü �ӵ�, HP ȸ���� ����
        _stat = new SkillStat
        {
            _isReady = true,
            _coolDown = 25.0f,
            _damage = 5,               // �ʴ� �����
            _damageTarget = 9999,
            _duration = 5.1f,           // 5�� ���� ����
            _castRange = 5.0f,
            _projectileSpeed = 10.0f,   // ��ǻ� ū �ǹ� ����
            _healAmount = 0
        };
    }

    void OnEnable()
    {
        StartCoroutine(SpellPoison());
        StartCoroutine(DealPoisonDamage());
    }

    // duration���� ��ų�� ����
    IEnumerator SpellPoison()
    {
        yield return new WaitForSeconds(_stat._duration);
        Destroy(this.gameObject);
    }

    // 1�ʸ��� ���� �� ���Ϳ��� ��Ʈ ����� ����
    IEnumerator DealPoisonDamage()
    {
        while(true)
        {
            yield return new WaitForSeconds(1.0f);
            foreach(var monster in _poisonousMonsters)
            {
                // �̹� ���� ���͸� üũ�Ϸ��� �� �� ����Ʈ���� ����
                if(monster == null)
                {
                    _poisonousMonsters.Remove(monster);
                }

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

    public override SkillStat GetStat()
    {
        Debug.Log("Poison���� ������");
        return Stat;
    }
}
