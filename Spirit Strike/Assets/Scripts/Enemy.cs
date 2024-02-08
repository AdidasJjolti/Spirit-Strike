using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] int _HP = 2;
    [SerializeField] PlayerControl _player;   // ���� ��ǥ���� ����� �÷��̾� ��ġ ����
    NavMeshAgent _agent;
    Rigidbody _rigid;
    Animator _animator;

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
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        // �÷��̾� ���󰡱�
        // �÷��̾�� ����� ��������� ���� ����
        //if((transform.position - _player.transform.position).magnitude < 1.0f)
        //{
        //    _agent.velocity = Vector3.zero;
        //    _animator.SetBool("isMoving", false);
        //}
        //else
        //{
        //    _agent.SetDestination(_player.transform.position);
        //    _animator.SetBool("isMoving", true);
        //}

        _agent.SetDestination(_player.transform.position);
        _animator.SetBool("isMoving", true);

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
        Debug.Log($"���� ü���� : {_HP}");


        if(_HP <= 0)
        {
            StartCoroutine("Die");
        }
    }

    IEnumerator Die()
    {
        _animator.SetTrigger("Die");
        yield return new WaitForSecondsRealtime(1.0f);
        gameObject.SetActive(false);
        yield return null;
    }
}
