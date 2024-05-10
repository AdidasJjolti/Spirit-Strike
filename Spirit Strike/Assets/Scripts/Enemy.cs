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


    [SerializeField] PlayerControl _player;   // 공격 목표물로 사용할 플레이어 위치 저장
    NavMeshAgent _agent;
    Rigidbody _rigid;
    Animator _animator;

    [SerializeField] float _attackDelay;
    [SerializeField] ParticleSystem _atkEffect;
    [SerializeField] float _waitSec;

    [SerializeField] GameObject _hpBarPrefab;   // HP바 프리팹
    [SerializeField] Canvas _hpBarCanvas;       // HP바가 생성될 캔버스
    [SerializeField] Slider _hpBarSlider;       // HP바가 가진 슬라이더 컴포넌트
    GameObject _hpBarObj;                       // 적 생성될 때 함께 생성한 HP바 게임오브젝트
    UIHPBar _hpBar;                             // 대미지를 입을 때 슬라이더 값을 변경할 함수 연결용 변수

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
        //Debug.Log($"몬스터의 공격력은 {_attack}이야.");
        //Debug.Log($"몬스터의 공격속도는 {_attackSpeed}이야.");

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
        // 플레이어 따라가기
        // 플레이어와 충분히 가까워지면 추적 정지
        // 플레이어 공격
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

    // 물리력이 NavAgent의 이동을 방해하지 않도록 속도를 0으로 설정
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
            Debug.Log("몬스터가 죽었습니다.");
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
        //Debug.Log($"{gameObject.transform.name}이 플레이어 공격!");

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

    // 실제 공격 모션과 이펙트 재생 시점을 맞추기 위해 _waitSec만큼 대기
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
