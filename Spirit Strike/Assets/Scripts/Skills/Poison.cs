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
            _damage = 5,                // �ʴ� �����
            _damageTarget = 9999,
            _duration = 4.9f,           // 5�� ���� ����
            _castRange = 5.0f,
            _projectileSpeed = 10.0f,   // ��ǻ� ū �ǹ� ����
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

    // duration���� ��ų�� ����
    IEnumerator SpellPoison()
    {
        yield return new WaitForSeconds(_stat._duration);
        //StopCoroutine(DealPoisonDamage());
        Destroy(this.gameObject);
    }

    // 1�ʸ��� ���� �� ���Ϳ��� ��Ʈ ����� ����
    IEnumerator DealPoisonDamage()
    {
        float elapsedTime = 0.0f;
        int time = 1;

        while (elapsedTime < _stat._duration)
        {
            Debug.Log($"{time}��° �����");

            yield return new WaitForSeconds(1.0f);
            elapsedTime += 1.0f;

            foreach (var monster in _poisonousMonsters)
            {
                // �̹� ���� ���͸� üũ�Ϸ��� �� �� ����Ʈ���� ����
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
        Debug.Log("Poison���� ������");
        return Stat;
    }
}
