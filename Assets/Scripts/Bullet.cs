using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damage;
    private float speed;
    private float range;
    private LayerMask targetMask;
    private Vector2 direction;

    public void Setup(int _damage, float _speed, float _range, LayerMask _targetMask, Vector2 _direction)
    {
        damage = _damage;
        speed = _speed;
        range = _range;
        targetMask = _targetMask;
        direction = _direction;
    }

    private void Start()
    {
        Invoke("DestroySelf", range);
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, range, targetMask);
        if(hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Enemy"))
                hitInfo.collider.GetComponent<Enemy>().TakeDamage(damage);
            if (hitInfo.collider.CompareTag("Player"))
                hitInfo.collider.GetComponent<PlayerController>().TakeDamage(damage);
            if (hitInfo.collider.CompareTag("Bullet")) //so its possible to destroy enemy bullets with your own
                Destroy(hitInfo.collider.gameObject);
            
            DestroySelf();
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

}
