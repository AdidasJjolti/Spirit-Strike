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

    [SerializeField] GameObject _firePrefab;
    bool _isSkillReady = true;
    [SerializeField] float _rayDistance = 1.5f;

    [SerializeField] LayerMask _targetLayer;
    [SerializeField] ObjectManager _objManager;
    [SerializeField] PlayerDataManager _dataManager;


    void Awake()
    {
        _dataManager = new PlayerDataManager();
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
            //UnityEngine.Debug.Log("Ÿ�� ���� Ž��");
        }
        else
        {
            if (_isSkillReady && Vector3.Distance(transform.position, _targetEnemy.transform.position) <= 5.0f)
            {
                // ��ų �غ�Ǹ� ��ų ��Ÿ������� �����Ͽ� ��ų ���
                // �⺻ ���ݺ��� �켱 üũ
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

    // �������� NavAgent�� �̵��� �������� �ʵ��� �ӵ��� 0���� ����
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

    // ��ų ��� �Լ� �ӽ� ����
    // ToDo : ���� ��ų�� �߿��� ��� ������ ��ų ������ �޾� �ش� ��ų���� ����ϵ��� ����
    void UseSkill()
    {
        if (_attackDelay < (float)100 / _dataManager.AtkSpeed)
        {
            return;
        }

        transform.LookAt(_targetEnemy.transform);

        _isIdle = false;
        _isWalking = false;

        _animator.SetBool("isWalking", _isWalking);
        _animator.SetBool("isIdle", _isIdle);

        _animator.SetTrigger("isAttacking");

        Instantiate(_firePrefab, transform);

        if (_targetEnemy.HP <= 0)
        {
            // �ӽ÷� ���� óġ �� 20��ŭ ����ġ ȹ��
            _dataManager.GetExp(20);
            _targetEnemy = null;
        }

        _attackDelay = 0;
    }

    // �⺻ ���� �� ����� �Լ�
    void Attack()
    {
        if (_attackDelay < (float)100 / _dataManager.AtkSpeed)
        {
            return;
        }

        UnityEngine.Debug.Log($"���� �ӵ��� {(float)100 / _dataManager.AtkSpeed}�̾�.");

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
                _targetEnemy.TakeDamage(_dataManager.Attack);

                // Ÿ�� ���Ͱ� ������ ���� Ÿ���� �����ϱ� ���� null�� ���� �� ���� Ÿ�� ���� Ž��
                if (_targetEnemy.HP <= 0)
                {
                    // �ӽ÷� ���� óġ �� 20��ŭ ����ġ ȹ��
                    _dataManager.GetExp(20);
                    _targetEnemy = null;
                }
            }
        }

        _attackDelay = 0;
    }

    void FindNearestEnemy()
    {
        Enemy targetEnemy = null;
        float diff = 100f;

        // ToDo : Raycast ��ſ� �� ���� �ִ� ���Ͱ� ������ ������ �̸� ������ ����Ʈ�� ���͸� �߰��ϰ� ���⿡�� �Ÿ��� Ž���ϵ��� ����
        // ToDo : ������Ʈ Ǯ, ���Ǿ�ĳ��Ʈ, �Ǵ� �ٸ� Ž�� ������ ����Ͽ� �׽�Ʈ�غ���
        // ��� : ������Ʈ Ǯ�� Ž�� �� ���Ǿ�ĳ��Ʈ���� 1.5�� ~ 2�� ���� �� ������ �˻� ����

        #region
        // ���Ǿ�ĳ��Ʈ�� Ÿ�� ���� Ž��
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
        //UnityEngine.Debug.Log($"Ž���� �ɸ� �ð��� {watch.ElapsedMilliseconds} + ms");
        #endregion

        #region
        // ������Ʈ Ǯ���� Ÿ�� ���� Ž��
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
        //UnityEngine.Debug.Log($"Ž���� �ɸ� �ð��� {watch.ElapsedMilliseconds} + ms");
        #endregion
    }

    public void TakeDamage(int damage)
    {
        _dataManager.Hp -= damage;
        //Debug.Log($"�ƾ�! {_hp}");
    }
}