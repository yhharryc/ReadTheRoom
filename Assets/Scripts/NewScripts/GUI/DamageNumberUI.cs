using System.Collections;
using UnityEngine;
using TMPro;

public class DamageNumberUI : MonoBehaviour
{
    private TextMeshProUGUI damageText;

    [Header("Floating/Fading Settings")]
    [SerializeField] private float floatUpSpeed = 30f;
    [SerializeField] private float fadeDuration = 1.5f;

    private float timer;

    private void Awake()
    {
        damageText = GetComponent<TextMeshProUGUI>();
    }

    public void SetDamageValue(float damage, bool isCrit, EventContext context)
    {
        // Check if damage is 0 => blocked
        if (damage== 0f)
        {
            damageText.text = "BLOCKED";
            damageText.color = new Color(0.8f, 0.8f, 0.8f); // Slight grey color
            damageText.fontSize = 20; 
        }
        else
        {
            // Normal or Crit damage
            damageText.text = Mathf.RoundToInt(damage).ToString();

            if (isCrit)
            {
                damageText.color = Color.yellow;
                damageText.fontSize = 40;
                damageText.text += "!!";
            }
            else
            {
                damageText.color = Color.white;
                damageText.fontSize = 20;
            }
        }

        // You could also use context.HitData here if you want further customization
        // e.g., context.HitData.ElementType => change color, etc.
    }

    private void Update()
    {
        timer += Time.deltaTime;

        // Move upward each frame
        transform.Translate(Vector3.up * floatUpSpeed * Time.deltaTime);

        // Once we've passed the fadeDuration, destroy (or recycle) this object
        if (timer >= fadeDuration)
        {
            Destroy(gameObject);
        }
    }
}
