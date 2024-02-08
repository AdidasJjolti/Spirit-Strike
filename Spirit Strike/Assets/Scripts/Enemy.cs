using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] int _HP = 2;
    [SerializeField] PlayerControl _player;   // 공격 목표물로 사용할 플레이어 위치 저장
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
        // 플레이어 따라가기
        // 플레이어와 충분히 가까워지면 추적 정지
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

        // 플레이어 공격
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

    public void TakeDamage(int damage)
    {
        _HP -= damage;
        Debug.Log($"현재 체력은 : {_HP}");


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
