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

    float _hAxis;
    float _vAxis;
    Vector3 _moveDir;
    float _moveSpeed = 3.0f;

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

            _agent.isStopped = true;
            FindNearestEnemy();
            Debug.Log("Ÿ�� ���� Ž��");
        }
        else
        {
            if (Vector3.Distance(transform.position, _targetEnemy.transform.position) <= 1.5f)
            {
                if (_targetEnemy != null)
                {
                    //_agent.velocity = Vector3.zero;
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
                _targetEnemy.TakeDamage(_damage);

                // Ÿ�� ���Ͱ� ������ ���� Ÿ���� �����ϱ� ���� null�� ���� �� ���� Ÿ�� ���� Ž��
                if(_targetEnemy.HP <= 0)
                {
                    _targetEnemy = null;
                    Debug.Log("Ÿ�� ���� ����");
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
        RaycastHit[] targets = Physics.SphereCastAll(transform.position, 100f, Vector3.forward, 0f, _targetLayer, QueryTriggerInteraction.UseGlobal);

        foreach (var target in targets)
        {
            if(target.transform.GetComponent<Enemy>().HP <= 0)
            {
                continue;
            }

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