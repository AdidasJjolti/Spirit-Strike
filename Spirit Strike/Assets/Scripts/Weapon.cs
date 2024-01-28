using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] int _damage = 1;
    Collider _collider;

    void Awake()
    {
        _collider = gameObject.GetComponent<Collider>();
        _collider.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(_damage);
            StartCoroutine(DisableCollider());
        }
    }

    IEnumerator DisableCollider()
    {
        yield return new WaitForSeconds(0.3f);
        _collider.enabled = false;
        yield break;
    }
}
