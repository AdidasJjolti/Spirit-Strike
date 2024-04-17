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

        // ���۰� �Բ� ���̾ �������� �ҷ��ͼ� �÷��̾� �ڽ����� ���
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
                    UnityEngine.Debug.Log($"��� ������ ��ų�� {(string)_skillManager.SkillDic[key]}��.");
                    _isSkillReady = true;

                    // ��� ������ ��ų Ÿ�Կ� �´� �������� _skillPrefab�� ���
                    _skillPrefab = _skillManager.LoadPrefab(key);
                    _skillType = key;
                    break;
                }
            }

            if(_skillPrefab == null)
            {
                UnityEngine.Debug.Log($"��� ������ ��ų�� ����.");
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

    //��ų�� �߿��� ��� ������ ��ų ������ �޾� �ش� ��ų���� ���
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

        // ����� ��ų�� ��Ÿ�Ӹ�ŭ ��� �Ұ� ���·� ����
        StartCoroutine(_skillManager.CountCoolDown(_skillType));

        if (_targetEnemy.HP <= 0)
        {
            // ToDo : ���� �����κ��� ȹ�� ����ġ �޾ƿ����� ����
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

        if(_targetEnemy == null)
        {
            FindNearestEnemy();
        }

        UnityEngine.Debug.Log($"���� �ӵ��� {(float)100 / _dataManager.AtkSpeed}�̾�.");

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

        // Raycast ��ſ� �� ���� �ִ� ���Ͱ� ������ ������ �̸� ������ ����Ʈ�� ���͸� �߰��ϰ� ���⿡�� �Ÿ��� Ž���ϵ��� ����
        // ������Ʈ Ǯ, ���Ǿ�ĳ��Ʈ, �Ǵ� �ٸ� Ž�� ������ ����Ͽ� �׽�Ʈ�غ���
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

    public void Heal(int healAmount)
    {
        _dataManager.Hp += healAmount;

        // �ִ� ü���� �Ѿ ȸ������ ����
        if(_dataManager.Hp >= _dataManager.MaxHP)
        {
            _dataManager.Hp = _dataManager.MaxHP;
        }
    }
}