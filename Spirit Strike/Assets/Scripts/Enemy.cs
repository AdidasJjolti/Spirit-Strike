using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] int _HP = 100;
    [SerializeField] PlayerControl _player;   // ���� ��ǥ���� ����� �÷��̾� ��ġ ����
    NavMeshAgent _agent;
    Rigidbody _rigid;

    public int HP
    {
        get
        {
            return _HP;
        }
    }

    void OnEnable()
    {
        if(_player == null)
        {
            _player = FindObjectOfType<PlayerControl>();
        }
    }

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // �÷��̾� ���󰡱�
        // �÷��̾�� ����� ��������� ���� ����
        if((transform.position - _player.transform.position).magnitude < 1.0f)
        {
            //_agent.SetDestination(transform.position);
            _agent.velocity = Vector3.zero;
        }
        else
        {
            _agent.SetDestination(_player.transform.position);
        }
        // �÷��̾� ����
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

    public void TakeDamage(int damage)
    {
        _HP -= damage;
        Debug.Log($"HP remaining : {_HP}");
    }
}
