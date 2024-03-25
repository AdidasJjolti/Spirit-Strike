using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSlashBall : FireSlash
{
    [SerializeField] Vector3 _startPosition;

    // ToDo : ��ų����Ʈ�� ���� �Ÿ� �̻� �̵��ϸ� ���ŵǵ��� ����
    public override void OnEnable()
    {
        base.OnEnable();
        _startPosition = transform.position;
        Debug.Log($"���� ������ {_startPosition}");
    }

    public override void Update()
    {
        base.Update();

        if(Vector3.Distance(_startPosition, transform.position) >= 5.0f)
        {
            Debug.Log($"�Ÿ��� {Vector3.Distance(_startPosition, transform.position)}��.");
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
