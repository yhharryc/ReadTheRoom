using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("Faction Selection")]
    [SerializeField] private Faction faction = Faction.NONE;

    [Header("Bar UI")]
    [SerializeField] private Image fillImage;

    [Tooltip("If true, the fill will go from 1 to 0 instead of 0 to 1.")]
    [SerializeField] private bool reverseFill = false;

    private ICharacter character;

    [Tooltip("The name of the variable you are looking for. Needs to be one with a corresponding Max Attribute. For example, 'Health' and 'MaxHealth'")]
    [SerializeField] private string attributeName = "Health";

    private void Start()
    {
        switch (faction)
        {
            case Faction.PLAYER:
                // Find the PlayerCharacter in the scene
                var playerChar = FindObjectOfType<PlayerCharacter>();
                if (playerChar != null)
                {
                    character = playerChar as ICharacter;
                }
                else
                {
                    Debug.LogWarning("No PlayerCharacter found in scene.");
                }
                break;

            case Faction.ENEMY:
                // Look on this object or in its parent for an ICharacter
                character = GetComponentInParent<ICharacter>();
                if (character == null)
                {
                    Debug.LogWarning("No ICharacter found on this or parent object for ENEMY faction.");
                }
                break;

            default:
                Debug.LogWarning("Faction not set to PLAYER or ENEMY, cannot find ICharacter.");
                break;
        }
    }

    private void Update()
    {
        // If no character assigned, do nothing
        if (character == null) return;

        // Get the ability system from the character
        AbilitySystemComponent asc = character.GetAbilitySystemComponent();
        if (asc == null) return;

        // Get the current and max attribute values
        float attributeValue = asc.GetAttributeValue(attributeName, out bool foundAttribute);
        float maxAttributeValue = asc.GetAttributeValue("Max" + attributeName, out bool foundMaxAttribute);

        // If both attributes exist, update the UI
        if (foundAttribute && foundMaxAttribute && maxAttributeValue > 0f)
        {
            float fillAmount = attributeValue / maxAttributeValue;

            // If reverseFill is enabled, invert the fill
            if (reverseFill)
            {
                fillAmount = 1f - fillAmount;
            }

            fillImage.fillAmount = Mathf.Clamp01(fillAmount);
        }
    }
}
