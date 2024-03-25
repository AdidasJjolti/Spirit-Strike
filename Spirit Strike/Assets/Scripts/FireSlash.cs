using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSlash : Skill
{
    Vector3 _player;

    protected SkillStat _stat;

    public SkillStat Stat
    {
        get
        {
            return _stat;
        }
    }

    [SerializeField] GameObject[] _projectiles;
    protected bool _isDistanceLimit;

    void Awake()
    {
        // 준비 상태, 쿨타임, 대미지, 타겟수, 스킬 범위, 스킬 사거리, 투사체 속도, HP 회복량 정의
        _stat = new SkillStat
        {
            _isReady = true,
            _coolDown = 6.0f,
            _damage = 20,
            _damageTarget = 1,
            _damageArea = 0.0f,
            _castRange = 5.0f,
            _projectileSpeed = 10.0f,
            _healAmount = 0
        };
    }

    // 활성화되면 3방향으로 투사체 발사
    public virtual void OnEnable()
    {
        Debug.Log($"스킬 공격력은 {_stat._damage}야");

        Transform playerTr = FindObjectOfType<PlayerControl>().transform;
        _player = playerTr.position;
        Vector3 dir0 = playerTr.forward;
        //Debug.Log($"플레이어 전방 좌표는 {dir0}");

        _projectiles[0].GetComponent<Rigidbody>().AddForce(dir0 * _stat._projectileSpeed, ForceMode.Impulse);

        var quaternion1 = Quaternion.Euler(0, 15f, 0);
        Vector3 dir1 = quaternion1 * dir0;
        //Debug.Log($"오른쪽으로 {dir1}");
        _projectiles[1].GetComponent<Rigidbody>().AddForce(dir1 * _stat._projectileSpeed, ForceMode.Impulse);

        var quaternion2 = Quaternion.Euler(0, 345f, 0);
        Vector3 dir2 = quaternion2 * dir0;
        //Debug.Log($"왼쪽으로 {dir2}");
        _projectiles[2].GetComponent<Rigidbody>().AddForce(dir2 * _stat._projectileSpeed, ForceMode.Impulse);
    }

    public virtual void Update()
    {
        if(_isDistanceLimit)
        {
            Destroy(gameObject);
        }
    }
}
