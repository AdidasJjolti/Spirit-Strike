using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] Vector3 _offset;
    Renderer _obstacleRenderer;

    void Update()
    {
        transform.position = _player.transform.position + _offset;

        float distance = Vector3.Distance(transform.position, _player.position);
        Vector3 direction = (_player.position - transform.position).normalized;
        RaycastHit hit;

        Debug.DrawRay(transform.position, direction * 100, Color.red);

        if(Physics.Raycast(transform.position, direction * 100, out hit, distance))
        {
            if(hit.transform.GetComponent<Enemy>())
            {
                return;
            }

            _obstacleRenderer = hit.transform.GetComponentInChildren<Renderer>();
            if(_obstacleRenderer != null)
            {
                Material mat = _obstacleRenderer.material;
                Color matColor = mat.color;
                matColor.a = 0.5f;
                mat.color = matColor;
            }
        }
    }
}
