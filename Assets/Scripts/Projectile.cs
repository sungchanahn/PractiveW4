using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public GameObject rangedEnemy;
    RangedEnemy enemy;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        enemy = rangedEnemy.GetComponent<RangedEnemy>();
    }

    private void Start()
    {
        Vector3 direction = DirectionToPlayer();
        RotationToTarget(direction);
        rb.velocity = direction * speed;
        //rb.AddForce(direction * speed, ForceMode.Impulse);
    }

    private void RotationToTarget(Vector3 direction)
    {
        float rotY = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, rotY, 0);
    }

    private Vector3 DirectionToPlayer()
    {
        return (CharacterManager.Instance.Player.transform.position - transform.position).normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CharacterManager.Instance.Player.Condition.TakePhysicalDamage(enemy.damage);
        }
        else if (other.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}