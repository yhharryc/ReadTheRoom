using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("Faction Selection")]
    [SerializeField] private Faction faction = Faction.NONE;

    [Header("Bar UI")]
    [SerializeField] private Image fillImage;

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
                // (assuming Enemy AI script implements ICharacter)
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
        AbilitySystemComponent asc = character.GetAbilitySystemComponent();
        if(asc==null) return;
        float attributeValue = asc.GetAttributeValue(attributeName,out bool foundAttribute);
        float maxAttributeValue = asc.GetAttributeValue("Max"+attributeName,out bool foundMaxAttribute);
        if(foundAttribute && foundMaxAttribute)
        {
            // Update fill from 0.0 to 1.0
            float fillAmount = attributeValue / maxAttributeValue;
            fillImage.fillAmount = Mathf.Clamp01(fillAmount);
        }
        //FIXME: should hook up to attribute change event so that it's not updating every tick. 
    }
}
