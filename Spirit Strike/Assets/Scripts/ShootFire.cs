using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootFire : Skill
{
    Vector3 _player;
    Vector3 _target;
    Rigidbody _rigid;

    // 활성화되면 플레이어로부터 타겟 몬스터 방향으로 투사체 발사
    void OnEnable()
    {
        // 준비 상태, 쿨타임, 대미지, 타겟수, 스킬 범위, 스킬 사거리, 투사체 속도, HP 회복량 정의
        SetSkill(true, 4.0f, 20, 1, 0.0f, 5.0f, 10.0f, 0.0f);
        Debug.Log($"스킬 공격력은 {_damage}야");

        _player = FindObjectOfType<PlayerControl>().transform.position;
        _target = FindObjectOfType<PlayerControl>().TargetEnemy.transform.position;
        _rigid = GetComponent<Rigidbody>();

        Vector3 dir = (_target - _player).normalized;
        _rigid.AddForce(dir * _projectileSpeed, ForceMode.Impulse);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(_damage);
            Destroy(this.gameObject);
        }
    }
}
