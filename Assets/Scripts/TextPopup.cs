using UnityEngine;
using TMPro;
using System.Collections.Generic;

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

    public static void CreateTitlePopup(string text)
    {      
        Vector2 titlePos = new Vector2(0, 3);
        var popup = Instantiate(GameManager.i.uiManager.TitlePopup, titlePos, Quaternion.identity);
        popup.Setup(text);
    }

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    public void Setup(int damageAmount, bool isEnemy)
    {
        fadeTimer = 1;
        textMesh.text = damageAmount.ToString();

        if (isEnemy)
            textMesh.outlineColor = textMesh.color = GameManager.i.uiManager.EnemyTextColor;
        
        textColor = textMesh.color;

        textSize = textMesh.transform.localScale;
        textMesh.transform.localScale = Vector2.zero;
        textMesh.transform.LeanScale(textSize, 0.25f).setEaseOutBounce();
    }

    public void Setup(string text)
    {
        fadeTimer = 3;
        textMesh.text = text;

        textColor = textMesh.color;

        textSize = textMesh.transform.localScale;
        textMesh.transform.localScale = Vector2.zero;
        textMesh.transform.LeanScale(textSize, 0.25f).setEaseOutBounce();

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
