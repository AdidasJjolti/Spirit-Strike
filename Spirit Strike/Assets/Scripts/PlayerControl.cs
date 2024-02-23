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

    //[SerializeField] int _hp = 10;
    //[SerializeField] int _level = 1;
    //[SerializeField] int _attack;

    Animator _animator;
    NavMeshAgent _agent;

    [SerializeField] float _attackSpeed = 1.0f;
    [SerializeField] float _attackDelay = 1.0f;
    [SerializeField] Enemy _targetEnemy;

    [SerializeField] float _rayDistance = 1.5f;

    [SerializeField] PlayerData _data;
    [SerializeField] PlayerExperienceData _expData;
    [SerializeField] int _curExp = 0;

    [SerializeField] LayerMask _targetLayer;
    [SerializeField] ObjectManager _objManager;

    void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _isIdle = true;

        _animator = GetComponent<Animator>();
        _animator.SetBool("isIdle", true);

        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _moveSpeed;

        _data = new PlayerData();
        _expData = new PlayerExperienceData();

        LoadPlayerExperienceDataFromJson();
        LoadPlayerDataFromJson();
    }

    void Update()
    {
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
            if (Vector3.Distance(transform.position, _targetEnemy.transform.position) <= 1.5f)
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
        if(_agent.isStopped)
        {
            _agent.isStopped = false;
        }

        _agent.SetDestination(_targetEnemy.transform.position);
        _agent.speed = _moveSpeed;

        _isIdle = false;
        _isWalking = true;

        _animator.SetBool("isWalking", _isWalking);
        _animator.SetBool("isIdle", _isIdle);
    }

    void Attack()
    {
        if (_attackDelay < _attackSpeed)
        {
            return;
        }

        transform.LookAt(_targetEnemy.transform);

        _isIdle = false;
        _isWalking = false;

        _animator.SetBool("isWalking", _isWalking);
        _animator.SetBool("isIdle", _isIdle);

        _animator.SetTrigger("isAttacking");

        Ray ray = new Ray(transform.position + new Vector3(0, 0.5f, 0), transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _rayDistance))
        {
            if (hit.collider.GetComponent<Enemy>() == _targetEnemy)
            {
                _targetEnemy.TakeDamage(_data._attack);

                // 타겟 몬스터가 죽으면 다음 타겟을 설정하기 위해 null로 변경 후 다음 타켓 몬스터 탐색
                if(_targetEnemy.HP <= 0)
                {
                    GetExp();
                    _targetEnemy = null;
                    //Debug.Log("타겟 몬스터 해제");
                }
            }
        }

        _attackDelay = 0;
    }

    void FindNearestEnemy()
    {
        Enemy targetEnemy = null;
        float diff = 100f;

        // ToDo : Raycast 대신에 맵 내에 있는 몬스터가 스폰될 때마다 미리 정의한 리스트에 몬스터를 추가하고 여기에서 거리를 탐색하도록 수정
        // ToDo : 오브젝트 풀, 스피어캐스트, 또는 다른 탐색 로직을 사용하여 테스트해보기
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
        _data._hp -= damage;
        //Debug.Log($"아얏! {_hp}");
    }


    void LoadPlayerDataFromJson()
    {
        string JsonString = File.ReadAllText(Application.dataPath + "/Resources/PlayerData.json");
        JsonData jsonData = JsonMapper.ToObject(JsonString);
        ParsingJsonQuest(jsonData, _expData._level);
    }

    // json 파일로부터 플레이어 레벨에 맞는 데이터 가져오기
    void ParsingJsonQuest(JsonData data, int level)
    {
        _data._level = (int)data[level - 1]["level"];
        _data._hp = (int)data[level - 1]["hp"];
        _data._attack = (int)data[level - 1]["attack"];
        _data._defence = (int)data[level - 1]["defence"];
        _data._dodge = (int)data[level - 1]["dodge"];
        _data._critical = (int)data[level - 1]["critical"];
        _data._atkSpeed = (int)data[level - 1]["atk_speed"];

        UnityEngine.Debug.Log($"현재 레벨은 1이고 공격력은 {_data._attack}");
    }

    void LoadPlayerExperienceDataFromJson()
    {
        string JsonString = File.ReadAllText(Application.dataPath + "/Resources/PlayerExperienceData.json");
        JsonData jsonData = JsonMapper.ToObject(JsonString);
        ParsingExpJsonQuest(jsonData);
    }

    // json 파일로부터 플레이어 레벨에 맞는 데이터 가져오기
    void ParsingExpJsonQuest(JsonData data)
    {
        _expData._level = (int)data[0]["level"];
        _expData._exp = (int)data[0]["exp"];


        UnityEngine.Debug.Log($"현재 레벨은 1이고 필요 경험치는 {_expData._exp}");
    }

    public void GetExp()
    {
        _curExp += 10;
    }
}