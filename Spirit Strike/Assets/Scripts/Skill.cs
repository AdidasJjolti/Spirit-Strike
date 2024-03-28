using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ToDo : Skill ���� Ŭ�������� GetStat �Լ��� �ݵ�� �����ϵ��� ���� �ʿ�
public class Skill : MonoBehaviour
{
    //protected bool _isReady;            // ��� ���� üũ, ��Ÿ���� 0�� �Ǹ� true

    //public bool IsReady
    //{
    //    get
    //    {
    //        return _isReady;
    //    }
    //}

    //protected float _coolDown;          // ��ų ���� ��� �ð�

    //public float CoolDown
    //{
    //    get
    //    {
    //        return _coolDown;
    //    }
    //}

    //protected int _damage;            // ���� �� ��ų �����

    //public int Damage
    //{
    //    get
    //    {
    //        return _damage;
    //    }
    //}

    //protected int _damageTarget;        // ��ų �ִ� ���� ���� ��, 1�̸� ���� ����, 2 �̻��̸� ���� ����

    //public int DamageTarget
    //{
    //    get
    //    {
    //        return _damageTarget;
    //    }
    //}

    //protected float _damageArea;        // ��ų ȿ�� ����, ���� �� ��ǥ �������� ���� �ȿ� �ִ� ���� ��� ����, ���� �����̸� üũ���� ����

    //public float DamageArea
    //{
    //    get
    //    {
    //        return _damageArea;
    //    }
    //}

    //protected float _castRange;         // ��ų ��� �Ÿ�

    //public float CastRange
    //{
    //    get
    //    {
    //        return _castRange;
    //    }
    //}

    //protected float _projectileSpeed;   // ��ų ����ü �̵� �ӵ�

    //public float ProjectileSpeed
    //{
    //    get
    //    {
    //        return _projectileSpeed;
    //    }
    //}

    //protected float _healAmount;        // ġ�� ��ų�� HP ȸ����

    //public float HealAmount
    //{
    //    get
    //    {
    //        return _healAmount;
    //    }
    //}

    public struct SkillStat
    {
        public bool _isReady;              // ��� ���� üũ, ��Ÿ���� 0�� �Ǹ� true
        public float _coolDown;            // ��ų ���� ��� �ð�
        public int _damage;                // ���� �� ��ų �����
        public int _damageTarget;          // ��ų �ִ� ���� ���� ��, 1�̸� ���� ����, 2 �̻��̸� ���� ����
        public float _damageArea;          // ��ų ȿ�� ����, ���� �� ��ǥ �������� ���� �ȿ� �ִ� ���� ��� ����, ���� �����̸� üũ���� ����
        public float _castRange;           // ��ų ��� �Ÿ�
        public float _projectileSpeed;     // ��ų ����ü �̵� �ӵ�
        public int _healAmount;            // ġ�� ��ų�� HP ȸ����

        public SkillStat(bool ready, float coolTime, int damage, int Target, float area, float range, float speed, int heal)
        {
            _isReady = ready;
            _coolDown = coolTime;
            _damage = damage;
            _damageTarget = Target;
            _damageArea = area;
            _castRange = range;
            _projectileSpeed = speed;
            _healAmount = heal;
        }
    }

    public virtual SkillStat GetStat()
    {
        Debug.Log("�ڽ� Ŭ�������� �������� ����");
        SkillStat stat = new SkillStat();
        return stat;
    }

    protected virtual void RemoveSkill()
    {
        Destroy(gameObject);
    }
}
