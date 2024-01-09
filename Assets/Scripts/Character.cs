using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected int damage;
    [SerializeField] protected int health;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected float rangedAttackRange;
    [SerializeField] protected float rangedAttackSpeed;
    public LayerMask enemyMask;

    protected Rigidbody2D rb;
    protected float attackTimer;
    public float attackCD;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;

        GetComponent<SpriteRenderer>().color = Color.red;
        Invoke("ResetColor", 0.25f);

        if (health <= 0)
        {
            health = 0;
            Death();
            return;
        }
    }
    
    void ResetColor()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public virtual void Death()
    {
        Destroy(this.gameObject);
    }
}