using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Character
{
    private int maxHP;
    [SerializeField] private int rangedDamage; //rightclick
    [SerializeField] private int blockValue; //down
    [SerializeField] private float dashCD; //space
    private float dashTimer;
    [SerializeField] private float dashSpeed;
    private float moveInput;
    private bool canHit = true;
    public bool CanHit => canHit;
    private bool isBlocking = false;

    [SerializeField] private float meleeAttackRange;
    [SerializeField] private Transform attackPos;
    [SerializeField] private GameObject shieldObj;
    [SerializeField] private GameObject puffPrefab;
    [SerializeField] private GameObject slashPrefab;


    private void Start()
    {
        maxHP = health;
        dashTimer = dashCD;
        GameManager.i.uiManager.SetupHealthBar(maxHP);
    }

    private void Update()
    {
        if(!isBlocking) //we disallow movement during block
            HandleMovement();
        UpdatePlayerFacing();
        HandleAttack();
        HandleBlocking();
    }

    private void HandleMovement()
    {
        moveInput = Input.GetAxis("Horizontal");
        var dash = HandleDash();

        rb.velocity = new Vector2(moveInput * moveSpeed * dash, rb.velocity.y);
    }

    private float HandleDash()
    {
        if (dashTimer <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Instantiate(puffPrefab, transform.position, Quaternion.identity);
                var image = GetComponent<SpriteRenderer>();
                var color = image.color;
                color.a = 0.5f;
                image.color = color;
                canHit = false;
                GetComponent<BoxCollider2D>().excludeLayers = enemyMask;

                dashTimer = dashCD;
                Invoke("AllowGetHit", 0.5f);
                return dashSpeed;
            }
        }
        else
        {
            dashTimer -= Time.deltaTime;
        }
        GameManager.i.uiManager.UpdateDashCD(dashTimer);
        return 1;
    }

    private void AllowGetHit()
    {
        canHit = true;
        GetComponent<BoxCollider2D>().excludeLayers = default;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void HandleAttack()
    {
        if (attackTimer <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var slash = Instantiate(slashPrefab, attackPos);
                Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPos.position, meleeAttackRange, enemyMask);
                for (int i = 0; i < enemiesHit.Length; i++)
                {
                    if (enemiesHit[i].CompareTag("Enemy"))
                        enemiesHit[i].GetComponent<Enemy>().TakeDamage(damage);
                    else
                        Destroy(enemiesHit[i]);
                }
                attackTimer = attackCD;
                Destroy(slash, 0.5f);
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                var bullet = Instantiate(bulletPrefab, attackPos);
                bullet.GetComponent<Bullet>().Setup(rangedDamage, rangedAttackSpeed, rangedAttackRange, enemyMask, Vector2.right);
                attackTimer = attackCD;
            }
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
        GameManager.i.uiManager.UpdateAttackCD(attackTimer / attackCD);
    }

    private void HandleBlocking()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            isBlocking = true;
            shieldObj.SetActive(true);
        }
        if(Input.GetKeyUp(KeyCode.LeftAlt))
        {
            isBlocking = false;
            shieldObj.SetActive(false);
        }

    }

    public override void TakeDamage(int damage)
    {
        if (!canHit) return;

        int damageModifier = isBlocking ? blockValue : 0;
        if (damageModifier < 0) damageModifier = 0; //dont want to deal negative damage

        base.TakeDamage(damage - damageModifier);

        GameManager.i.uiManager.UpdateHealthBar(health);
    }

    private void UpdatePlayerFacing()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (transform.position.x < mousePos.x) //You are to the left of the mouse, Face Right
        {
            transform.localScale = Vector2.one;
        }
        else
        {
            transform.localScale = new Vector2(-1, 1);
        }
    }

    private void OnDrawGizmosSelected() //Just to see the attack range when we select the player in editor
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, meleeAttackRange);
    }

    public override void Death()
    {
        GameManager.i.RevivePlayer();
        base.Death();
    }
}
