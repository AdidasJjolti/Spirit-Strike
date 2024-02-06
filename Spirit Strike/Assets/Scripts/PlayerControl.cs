using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour
{
    Rigidbody _rigid;
    [SerializeField] bool _isWalking;
    [SerializeField] bool _isIdle;
    [SerializeField] bool _isAttacking;

    float _hAxis;
    float _vAxis;
    Vector3 _moveDir;
    float _moveSpeed = 1f;

    Animator _animator;
    NavMeshAgent _agent;

    [SerializeField] float _attackSpeed = 1.0f;
    [SerializeField] float _attackDelay = 1.0f;
    [SerializeField] Enemy _targetEnemy;
    int _damage = 1;

    [SerializeField] float _rayDistance = 1.5f;

    [SerializeField] PlayerData _data;
    [SerializeField] LayerMask _targetLayer;

    public float attackSpeed
    {
        get
        {
            return _attackSpeed;
        }
    }

    void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _isIdle = true;

        _animator = GetComponent<Animator>();
        _animator.SetBool("isIdle", true);

        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _moveSpeed;

        //LoadPlayerDataFromJson();
    }

    void Update()
    {
        if (_targetEnemy == null)
        {
            _isIdle = true;
            _isWalking = false;

            _animator.SetBool("isWalking", _isWalking);
            _animator.SetBool("isIdle", _isIdle);

            _agent.velocity = Vector3.zero;
            FindNearestEnemy();
        }
        else
        {
            Move();
        }

        if(_targetEnemy == null)
        {
            return;
        }
        else
        {
            if (Vector3.Distance(transform.position, _targetEnemy.transform.position) <= 1.0f)
            {
                if (_targetEnemy != null)
                {
                    _agent.velocity = Vector3.zero;
                    Attack();
                }
            }
        }

        _attackDelay += Time.deltaTime;

        //_hAxis = Input.GetAxisRaw("Horizontal");
        //_vAxis = Input.GetAxisRaw("Vertical");
        //_moveDir = new Vector3(_hAxis, 0, _vAxis).normalized;

        //transform.position += _moveDir * _moveSpeed * Time.deltaTime;

        //if (_moveDir != Vector3.zero)
        //{
        //    _isIdle = false;
        //    _isWalking = true;

        //    _animator.SetBool("isWalking", _isWalking);
        //    _animator.SetBool("isIdle", _isIdle);
        //}
        //else
        //{
        //    _isIdle = true;
        //    _isWalking = false;

        //    _animator.SetBool("isWalking", _isWalking);
        //    _animator.SetBool("isIdle", _isIdle);
        //}

        //transform.LookAt(transform.position + _moveDir);

        //if (Input.GetKey(KeyCode.Space))
        //{
        //    Attack();
        //}
        //else
        //{
        //    _isAttacking = false;
        //    _animator.SetBool("isAttacking", _isAttacking);
        //}
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
        _agent.SetDestination(_targetEnemy.transform.position);

        _isIdle = false;
        _isWalking = true;

        _animator.SetBool("isWalking", _isWalking);
        _animator.SetBool("isIdle", _isIdle);
    }

    // ToDo : 애니메이션 동작에 코루틴을 적용하여 버벅이는 것처럼 보이는 현상 수정 필요
    void Attack()
    {
        if (_attackDelay < _attackSpeed)
        {
            return;
        }

        _isIdle = false;
        _isWalking = false;

        _animator.SetBool("isWalking", _isWalking);
        _animator.SetBool("isIdle", _isIdle);

        _isAttacking = true;
        _animator.SetBool("isAttacking", _isAttacking);

        _attackDelay = 0;

        Ray ray = new Ray(transform.position + new Vector3(0, 0.5f, 0), transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _rayDistance))
        {
            if (hit.collider.GetComponent<Enemy>() == _targetEnemy)
            {
                _targetEnemy.TakeDamage(_damage);

                // 타겟 몬스터가 죽으면 다음 타겟을 설정하기 위해 null로 변경
                if(_targetEnemy.HP <= 0)
                {
                    _targetEnemy = null;
                    _isAttacking = false;
                    _animator.SetBool("isAttacking", _isAttacking);
                }
            }
        }
    }

    void FindNearestEnemy()
    {
        Enemy targetEnemy = null;
        float diff = 100f;

        RaycastHit[] targets = Physics.SphereCastAll(transform.position, 100f, Vector3.forward, 0f, _targetLayer, QueryTriggerInteraction.UseGlobal);

        foreach (var target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos);

            if(curDiff < diff)
            {
                diff = curDiff;
                targetEnemy = target.transform.gameObject.GetComponent<Enemy>();
            }
        }

        _targetEnemy = targetEnemy;
    }

    void LoadPlayerDataFromJson()
    {
        string JsonString = File.ReadAllText(Application.dataPath + "/Resources/PlayerData.json");
        JsonData jsonData = JsonMapper.ToObject(JsonString);
        ParsingJsonQuest(jsonData);
    }

    void ParsingJsonQuest(JsonData data)
    {
        string level = data[0]["level"].ToString();
        string hp = data[0]["hp"].ToString();

        Debug.Log($"{level}, {hp}");
    }
}