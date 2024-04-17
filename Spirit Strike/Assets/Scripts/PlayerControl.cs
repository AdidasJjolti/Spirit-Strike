using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using UnityEngine.AI;
using System.Diagnostics;

public class PlayerControl : MonoBehaviour
{
    Rigidbody _rigid;
    [SerializeField] bool _isWalking;
    [SerializeField] bool _isIdle;

    float _hAxis;
    float _vAxis;
    Vector3 _moveDir;
    float _moveSpeed = 3.0f;

    Animator _animator;
    NavMeshAgent _agent;

    [SerializeField] float _attackSpeed;
    [SerializeField] float _attackDelay;
    [SerializeField] Enemy _targetEnemy;

    public Enemy TargetEnemy
    {
        get
        {
            return _targetEnemy;
        }
    }

    GameObject _skillPrefab;
    eSkill _skillType;

    [SerializeField] bool _isSkillReady;
    [SerializeField] float _rayDistance = 1.5f;

    [SerializeField] LayerMask _targetLayer;
    [SerializeField] ObjectManager _objManager;
    [SerializeField] PlayerDataManager _dataManager;

    SkillManager _skillManager;

    void Awake()
    {
        _dataManager = new PlayerDataManager();
        _skillManager = new SkillManager();
    }

    void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _isIdle = true;

        _animator = GetComponent<Animator>();
        _animator.SetBool("isIdle", true);

        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _moveSpeed;

        if (_dataManager != null)
        {
            _attackSpeed = (float)100 / _dataManager.AtkSpeed;
        }

        // 시작과 함께 파이어볼 프리팹을 불러와서 플레이어 자식으로 등록
        //GameObject prefab = _skillManager.LoadPrefab(eSkill.FIREBALL);
        //_skillPrefab = prefab;
    }

    void Update()
    {
        if(_skillPrefab == null)
        {
            foreach (var key in _skillManager.SkillReadyDic.Keys)
            {
                if (_skillManager.SkillReadyDic[key] == true)
                {
                    UnityEngine.Debug.Log($"사용 가능한 스킬은 {(string)_skillManager.SkillDic[key]}야.");
                    _isSkillReady = true;

                    // 사용 가능한 스킬 타입에 맞는 프리팹을 _skillPrefab에 사용
                    _skillPrefab = _skillManager.LoadPrefab(key);
                    _skillType = key;
                    break;
                }
            }

            if(_skillPrefab == null)
            {
                UnityEngine.Debug.Log($"사용 가능한 스킬이 없어.");
                _isSkillReady = false;
            }
        }

        if (_targetEnemy == null)
        {
            _isIdle = true;
            _isWalking = false;

            _animator.SetBool("isWalking", _isWalking);
            _animator.SetBool("isIdle", _isIdle);

            _agent.isStopped = true;
            FindNearestEnemy();
            //UnityEngine.Debug.Log("타겟 몬스터 탐색");
        }
        else
        {
            if (_isSkillReady && Vector3.Distance(transform.position, _targetEnemy.transform.position) <= 5.0f)
            {
                // 스킬 준비되면 스킬 사거리까지만 접근하여 스킬 사용
                // 기본 공격보다 우선 체크
                if (_attackDelay >= (float)100 / _dataManager.AtkSpeed)
                {
                    UseSkill();
                }
            }
            else if (!_isSkillReady && Vector3.Distance(transform.position, _targetEnemy.transform.position) <= 1.5f)
            {
                if (_targetEnemy != null)
                {
                    _agent.isStopped = true;
                    Attack();
                }
            }
            else
            {
                Move();
            }
        }

        _attackDelay += Time.deltaTime;
    }

    void FixedUpdate()
    {
        FreezeVelocity();
    }

    // 물리력이 NavAgent의 이동을 방해하지 않도록 속도를 0으로 설정
    void FreezeVelocity()
    {
        _rigid.velocity = Vector3.zero;
        _rigid.angularVelocity = Vector3.zero;
    }

    void Move()
    {
        if (_agent.isStopped)
        {
            _agent.isStopped = false;
        }

        _agent.SetDestination(_targetEnemy.transform.position);
        transform.LookAt(_targetEnemy.transform);
        _agent.speed = _moveSpeed;

        _isIdle = false;
        _isWalking = true;

        _animator.SetBool("isWalking", _isWalking);
        _animator.SetBool("isIdle", _isIdle);
    }

    //스킬셋 중에서 사용 가능한 스킬 정보를 받아 해당 스킬부터 사용
    void UseSkill()
    {
        if(!_isSkillReady || _skillPrefab == null)
        {
            return;
        }

        transform.LookAt(_targetEnemy.transform);

        if(_skillType != eSkill.HEAL)
        {
            if (Vector3.Distance(transform.position, _targetEnemy.transform.position) > 5.0f || _targetEnemy.HP <= 0)
            {
                return;
            }
        }

        _isIdle = false;
        _isWalking = false;

        SetAttackAnimation(_isWalking, _isIdle);

        var pos = transform.position;

        GameObject obj;

        switch(_skillType)
        {
            case eSkill.POISON:
            obj = Instantiate(_skillPrefab, new Vector3(pos.x, pos.y + 0.4f, pos.z + 3.0f), Quaternion.identity);
            break;

            case eSkill.HEAL:
            obj = Instantiate(_skillPrefab, new Vector3(pos.x, pos.y + 0.4f, pos.z), Quaternion.identity, transform);
            break;

            default:
            obj = Instantiate(_skillPrefab, new Vector3(pos.x, pos.y + 0.5f, pos.z), Quaternion.identity, transform);
                break;
        }

        _skillManager.GetCoolDown(obj, _skillType);
        _isSkillReady = false;
        _skillPrefab = null;

        // 사용한 스킬의 쿨타임만큼 사용 불가 상태로 변경
        StartCoroutine(_skillManager.CountCoolDown(_skillType));

        if (_targetEnemy.HP <= 0)
        {
            // ToDo : 몬스터 정보로부터 획득 경험치 받아오도록 수정
            // 임시로 몬스터 처치 시 20만큼 경험치 획득
            _dataManager.GetExp(20);
            _targetEnemy = null;
        }

        _attackDelay = 0;
    }

    // 기본 공격 시 사용할 함수
    void Attack()
    {
        if (_attackDelay < (float)100 / _dataManager.AtkSpeed)
        {
            return;
        }

        if(_targetEnemy == null)
        {
            FindNearestEnemy();
        }

        UnityEngine.Debug.Log($"공격 속도는 {(float)100 / _dataManager.AtkSpeed}이야.");

        transform.LookAt(_targetEnemy.transform);

        _isIdle = false;
        _isWalking = false;

        SetAttackAnimation(_isWalking, _isIdle);

        Ray ray = new Ray(transform.position + new Vector3(0, 0.5f, 0), transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _rayDistance))
        {
            if (hit.collider.GetComponent<Enemy>() == _targetEnemy)
            {
                _targetEnemy.TakeDamage(_dataManager.Attack);

                // 타겟 몬스터가 죽으면 다음 타겟을 설정하기 위해 null로 변경 후 다음 타켓 몬스터 탐색
                if (_targetEnemy.HP <= 0)
                {
                    // 임시로 몬스터 처치 시 20만큼 경험치 획득
                    _dataManager.GetExp(20);
                    _targetEnemy = null;
                }
            }
        }

        _attackDelay = 0;
    }

    void SetAttackAnimation(bool isWalking, bool isIdle)
    {
        if(_animator == null)
        {
            UnityEngine.Debug.LogError("Animator is Null");
            return;
        }

        _animator.SetBool("isWalking", isWalking);
        _animator.SetBool("isIdle", isIdle);

        if(!isWalking && !isIdle)
        {
            _animator.SetTrigger("isAttacking");
        }
    }

    void FindNearestEnemy()
    {
        Enemy targetEnemy = null;
        float diff = 100f;

        // Raycast 대신에 맵 내에 있는 몬스터가 스폰될 때마다 미리 정의한 리스트에 몬스터를 추가하고 여기에서 거리를 탐색하도록 수정
        // 오브젝트 풀, 스피어캐스트, 또는 다른 탐색 로직을 사용하여 테스트해보기
        // 결과 : 오브젝트 풀로 탐색 시 스피어캐스트보다 1.5배 ~ 2배 정도 더 빠르게 검색 가능

        #region
        // 스피어캐스트로 타겟 몬스터 탐색
        //Stopwatch watch = new Stopwatch();
        //watch.Start();
        //RaycastHit[] targets = Physics.SphereCastAll(transform.position, 100f, Vector3.forward, 0f, _targetLayer, QueryTriggerInteraction.UseGlobal);

        //foreach (var target in targets)
        //{
        //    if (target.transform.GetComponent<Enemy>().HP <= 0)
        //    {
        //        continue;
        //    }

        //    Vector3 myPos = transform.position;
        //    Vector3 targetPos = target.transform.position;
        //    float curDiff = Vector3.Distance(myPos, targetPos);

        //    if (curDiff < diff)
        //    {
        //        diff = curDiff;
        //        targetEnemy = target.transform.gameObject.GetComponent<Enemy>();
        //    }
        //}

        //_targetEnemy = targetEnemy;

        //watch.Stop();
        //UnityEngine.Debug.Log($"탐색에 걸린 시간은 {watch.ElapsedMilliseconds} + ms");
        #endregion

        #region
        // 오브젝트 풀에서 타켓 몬스터 탐색
        //Stopwatch watch = new Stopwatch();
        //watch.Start();

        foreach (var obj in _objManager._monsterList)
        {
            if (obj == null || obj.transform.GetComponent<Enemy>() == null || obj.transform.GetComponent<Enemy>().HP <= 0)
            {
                continue;
            }

            Vector3 myPos = transform.position;
            Vector3 targetPos = obj.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos);

            if (curDiff < diff)
            {
                diff = curDiff;
                targetEnemy = obj.transform.gameObject.GetComponent<Enemy>();
            }
        }

        _targetEnemy = targetEnemy;

        //watch.Stop();
        //UnityEngine.Debug.Log($"탐색에 걸린 시간은 {watch.ElapsedMilliseconds} + ms");
        #endregion
    }

    public void TakeDamage(int damage)
    {
        _dataManager.Hp -= damage;
        //Debug.Log($"아얏! {_hp}");
    }

    public void Heal(int healAmount)
    {
        _dataManager.Hp += healAmount;

        // 최대 체력을 넘어서 회복하지 않음
        if(_dataManager.Hp >= _dataManager.MaxHP)
        {
            _dataManager.Hp = _dataManager.MaxHP;
        }
    }
}