using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    private Vector3 _direction;
    private Rigidbody _rd;
    public float Speed;

    void Start()
    {
        float dirX = Random.Range(-1f, 1f);
        float dirY = Random.Range(-0.4f, 0.4f);
        float dirZ = Random.Range(0f, 1f) * -1f;
        _direction = new Vector3(dirX, dirY, dirZ).normalized;
        //Vector3.Normalize(_direction);
        _rd = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        _rd.velocity = _direction * Speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) return;
        if (collision.gameObject.CompareTag("EnemyAttack")) return;

        Vector3 normal = collision.GetContact(0).normal;
        _direction = Vector3.Reflect(_direction, normal).normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Destroy(gameObject);
    }

    public void SetDirection(Vector3 dir) { _direction = dir.normalized; }
}
