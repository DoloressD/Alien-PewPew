using UnityEngine;
using TMPro;

public enum TextPopupType
{
    Damage,
    Title // setting up for wave titles
}

public class TextPopup : MonoBehaviour
{
    public TextPopupType type;
    private TextMeshPro textMesh;
    private float fadeTimer;
    private Color textColor;
    private Vector2 textSize;

    public static TextPopup CreateDamagePopup (int damageAmount, Transform spawnTransform, bool isEnemy)
    {
        var popup = Instantiate(GameManager.i.uiManager.DamagePopup, spawnTransform.position, Quaternion.identity);
        popup.Setup(damageAmount, isEnemy);

        return popup;
    }

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    public void Setup(int damageAmount, bool isEnemy = false)
    {
        fadeTimer = 1;

        if (isEnemy)
            textMesh.outlineColor = textMesh.color = GameManager.i.uiManager.EnemyTextColor;
        
        textColor = textMesh.color;

        textSize = textMesh.transform.localScale;
        textMesh.transform.localScale = Vector2.zero;
        textMesh.transform.LeanScale(textSize, 0.25f).setEaseOutBounce();

        if (type == TextPopupType.Damage)
            textMesh.text = damageAmount.ToString();
    }

    private void Update()
    {
        if (type == TextPopupType.Damage)
        {
            float moveYSpeed = 3f;
            transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;
        }

        fadeTimer -= Time.deltaTime;
        if(fadeTimer <= 0)
        {
            float fadeSpeed = 3f;
            textColor.a -= fadeSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
                Destroy(gameObject);
        }
    }
}
