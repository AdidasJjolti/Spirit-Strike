using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using UnityEngine.UI;
using LitJson;
using System.IO;

public enum eMonster
{
    NONE = 0,
    GHOST,

    MAX
}

public class Enemy : MonoBehaviour
{
    [SerializeField] eMonster _type;

    [SerializeField] int _MaxHP;
    [SerializeField] int _HP;
    [SerializeField] int _attack;
    [SerializeField] int _defence;
    [SerializeField] int _dodge;
    [SerializeField] int _critical;
    [SerializeField] int _attackSpeed;
    int _exp;

    public int EXP
    {
        get
        {
            return _exp;
        }
    }


    [SerializeField] PlayerControl _player;   // ���� ��ǥ���� ����� �÷��̾� ��ġ ����
    NavMeshAgent _agent;
    Rigidbody _rigid;
    Animator _animator;

    [SerializeField] float _attackDelay;
    [SerializeField] ParticleSystem _atkEffect;
    [SerializeField] float _waitSec;

    [SerializeField] GameObject _hpBarPrefab;   // HP�� ������
    [SerializeField] Canvas _hpBarCanvas;       // HP�ٰ� ������ ĵ����
    [SerializeField] Slider _hpBarSlider;       // HP�ٰ� ���� �����̴� ������Ʈ
    GameObject _hpBarObj;                       // �� ������ �� �Բ� ������ HP�� ���ӿ�����Ʈ
    UIHPBar _hpBar;                             // ������� ���� �� �����̴� ���� ������ �Լ� ����� ����

    public int HP
    {
        get
        {
            return _HP;
        }
    }

    bool _isPoisonous;

    public bool IsPoisonous
    {
        get
        {
            return _isPoisonous;
        }

        set
        {
            _isPoisonous = value;
        }
    }

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _rigid = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        LoadEnemyData(_type);
        //Debug.Log($"������ ���ݷ��� {_attack}�̾�.");
        //Debug.Log($"������ ���ݼӵ��� {_attackSpeed}�̾�.");

        SetHPBar();
    }

    void OnEnable()
    {
        if (_player == null)
        {
            _player = FindObjectOfType<PlayerControl>();
        }

        _hpBarSlider.gameObject.SetActive(true);
    }

    void Update()
    {
        // �÷��̾� ���󰡱�
        // �÷��̾�� ����� ��������� ���� ����
        // �÷��̾� ����
        if (Vector3.Distance(transform.position, _player.transform.position) <= 1.2f)
        {
            if (_player != null)
            {
                _agent.isStopped = true;
                AttackPlayer();
            }
            else
            {
                _player = FindObjectOfType<PlayerControl>();
            }
        }
        else
        {
            Move();
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

    public void TakeDamage(int damage)
    {
        _HP -= Mathf.Max(damage - _defence, 0);
        _hpBar.ChangeValue(HP);

        if (_HP <= 0)
        {
            Debug.Log("���Ͱ� �׾����ϴ�.");
            StartCoroutine("Die");
        }
    }

    IEnumerator Die()
    {
        _player.DataManager.GetExp(_exp);
        _animator.SetTrigger("Die");
        Destroy(_hpBarObj);
        yield return new WaitForSecondsRealtime(1.0f);
        Destroy(this.gameObject);
        yield return null;
    }

    void Move()
    {
        _agent.SetDestination(_player.transform.position);
        _animator.SetBool("isMoving", true);
        _animator.SetBool("isAttacking", false);
    }

    void AttackPlayer()
    {
        //Debug.Log($"{gameObject.transform.name}�� �÷��̾� ����!");

        if(_animator == null)
        {
            Debug.LogError("Animator is Null");
            return;
        }

        if (_player == null)
        {
            Debug.LogError("Player is Null");
            return;
        }

        if (_attackDelay < (float) 100 / _attackSpeed)
        {
            _animator.SetBool("isAttacking", false);
            return;
        }

        transform.LookAt(_player.transform);
        _animator.SetBool("isMoving", false);
        _animator.SetBool("isAttacking", true);

        _player.TakeDamage(_attack);

        StartCoroutine("PlayEffect");
        _attackDelay = 0;
    }


    IEnumerator PlayEffect()
    {
        _atkEffect.gameObject.SetActive(true);
        yield return new WaitUntil(CheckAnimationState);

        _atkEffect.Play();

        yield break;
    }

    // ���� ���� ��ǰ� ����Ʈ ��� ������ ���߱� ���� _waitSec��ŭ ���
    bool CheckAnimationState()
    {
        return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= _waitSec;
    }

    void SetHPBar()
    {
        _hpBarCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        GameObject hpBar = Instantiate<GameObject>(_hpBarPrefab, _hpBarCanvas.transform);
        _hpBarObj = hpBar;

        _hpBarSlider = hpBar.GetComponent<Slider>();
        _hpBarSlider.maxValue = _MaxHP;
        _hpBarSlider.value = _MaxHP;

        var bar = hpBar.GetComponent<UIHPBar>();
        bar._transform = this.gameObject.transform;
        _hpBar = bar;
    }


    void LoadEnemyData(eMonster type)
    {
        string JsonString = File.ReadAllText(Application.dataPath + "/Resources/MonsterData.json");
        JsonData jsonData = JsonMapper.ToObject(JsonString);
        _HP = (int)jsonData[(int)type - 1]["hp"];
        _attack = (int)jsonData[(int)type - 1]["attack"];
        _defence = (int)jsonData[(int)type - 1]["defence"];
        _dodge = (int)jsonData[(int)type - 1]["dodge"];
        _critical = (int)jsonData[(int)type - 1]["critical"];
        _attackSpeed = (int)jsonData[(int)type - 1]["atk_speed"];
        _exp = (int)jsonData[(int)type - 1]["exp"];

        _MaxHP = _HP;
    }
}
