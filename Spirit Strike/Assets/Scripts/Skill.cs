using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ToDo : Skill 하위 클래스에서 GetStat 함수를 반드시 구현하도록 수정 필요
public class Skill : MonoBehaviour
{
    //protected bool _isReady;            // 사용 가능 체크, 쿨타임이 0가 되면 true

    //public bool IsReady
    //{
    //    get
    //    {
    //        return _isReady;
    //    }
    //}

    //protected float _coolDown;          // 스킬 재사용 대기 시간

    //public float CoolDown
    //{
    //    get
    //    {
    //        return _coolDown;
    //    }
    //}

    //protected int _damage;            // 적중 시 스킬 대미지

    //public int Damage
    //{
    //    get
    //    {
    //        return _damage;
    //    }
    //}

    //protected int _damageTarget;        // 스킬 최대 적중 몬스터 수, 1이면 단일 공격, 2 이상이면 다중 공격

    //public int DamageTarget
    //{
    //    get
    //    {
    //        return _damageTarget;
    //    }
    //}

    //protected float _damageArea;        // 스킬 효과 범위, 적중 시 좌표 기준으로 범위 안에 있는 몬스터 대상 공격, 단일 공격이면 체크하지 않음

    //public float DamageArea
    //{
    //    get
    //    {
    //        return _damageArea;
    //    }
    //}

    //protected float _castRange;         // 스킬 사용 거리

    //public float CastRange
    //{
    //    get
    //    {
    //        return _castRange;
    //    }
    //}

    //protected float _projectileSpeed;   // 스킬 투사체 이동 속도

    //public float ProjectileSpeed
    //{
    //    get
    //    {
    //        return _projectileSpeed;
    //    }
    //}

    //protected float _healAmount;        // 치유 스킬의 HP 회복량

    //public float HealAmount
    //{
    //    get
    //    {
    //        return _healAmount;
    //    }
    //}

    public struct SkillStat
    {
        public bool _isReady;              // 사용 가능 체크, 쿨타임이 0가 되면 true
        public float _coolDown;            // 스킬 재사용 대기 시간
        public int _damage;                // 적중 시 스킬 대미지
        public int _damageTarget;          // 스킬 최대 적중 몬스터 수, 1이면 단일 공격, 2 이상이면 다중 공격
        public float _damageArea;          // 스킬 효과 범위, 적중 시 좌표 기준으로 범위 안에 있는 몬스터 대상 공격, 단일 공격이면 체크하지 않음
        public float _castRange;           // 스킬 사용 거리
        public float _projectileSpeed;     // 스킬 투사체 이동 속도
        public int _healAmount;            // 치유 스킬의 HP 회복량

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
        Debug.Log("자식 클래스에서 구현하지 않음");
        SkillStat stat = new SkillStat();
        return stat;
    }

    protected virtual void RemoveSkill()
    {
        Destroy(gameObject);
    }
}
