using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSlashBall : FireSlash
{
    [SerializeField] Vector3 _startPosition;

    // ToDo : 스킬이펙트가 일정 거리 이상 이동하면 제거되도록 수정
    public override void OnEnable()
    {
        base.OnEnable();
        _startPosition = transform.position;
        Debug.Log($"시작 지점은 {_startPosition}");
    }

    public override void Update()
    {
        base.Update();

        if(Vector3.Distance(_startPosition, transform.position) >= 5.0f)
        {
            Debug.Log($"거리는 {Vector3.Distance(_startPosition, transform.position)}야.");
            _isDistanceLimit = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(_stat._damage);
            Destroy(this.gameObject);
        }
    }
}
