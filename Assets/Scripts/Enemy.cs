using UnityEngine;

public class Enemy : Character
{
    private PlayerController player;
    private Vector2 bulletDirection;

    private void FixedUpdate()
    {
        if (player == null)
            player = FindObjectOfType<PlayerController>();
        var playerIsLeft = player.transform.position.x < transform.position.x;
        var direction = playerIsLeft ? -1 : 1;
        bulletDirection = playerIsLeft ? Vector2.left : Vector2.right;
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
    }

    private void Update()
    {
        if (rangedAttackSpeed != 0 && bulletPrefab != null && attackCD != 0)
        {
            if (attackTimer <= 0)
            {
                var bullet = Instantiate(bulletPrefab, transform);
                bullet.GetComponent<Bullet>().Setup(damage, rangedAttackSpeed, rangedAttackRange, enemyMask, bulletDirection);
                attackTimer = attackCD;
            }
            else
            {
                attackTimer -= Time.deltaTime;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Contact damage takes the full health of the enemy but destroys the enemy
        if (collision.collider.CompareTag("Player"))
        {
            var player = collision.collider.GetComponent<PlayerController>();
            player.TakeDamage(health);

            if(player.CanHit)
                Death();
        }
    }
}
