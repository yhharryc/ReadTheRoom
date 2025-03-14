using UnityEngine;
using System;
using System.Reflection;

[CreateAssetMenu(menuName = "AbilitySystem/AttributeReference")]
public class AttributeReference : ScriptableObject
{
    [SerializeField]
    public string typeName;

#if UNITY_EDITOR
    [SerializeField] public UnityEditor.MonoScript attributeSetScript;
#endif

    [SerializeField] public string attributeFieldName;

    public bool IsValid()
    {
#if UNITY_EDITOR
        return attributeSetScript != null && !string.IsNullOrEmpty(attributeFieldName);
#else
        return !string.IsNullOrEmpty(attributeFieldName);
#endif
    }

    /// <summary>
    /// Grabs the System.Type either from MonoScript (editor only) or from the stored typeName at runtime.
    /// </summary>
    public Type GetAttributeSetType()
    {
#if UNITY_EDITOR
        if (attributeSetScript != null) {
            return attributeSetScript.GetClass();
        }
        return null;
#else
        if (!string.IsNullOrEmpty(typeName)) {
            return Type.GetType(typeName);
        }
        return null;
#endif
    }

    /// <summary>
    /// In OnValidate, if we have a valid MonoScript, we copy its AssemblyQualifiedName into typeName.
    /// This ensures we have a fallback at runtime (in a build) to reflect the correct class.
    /// </summary>
    private void OnValidate()
    {
#if UNITY_EDITOR
        if (attributeSetScript != null)
        {
            var t = attributeSetScript.GetClass();
            if (t != null)
            {
                typeName = t.AssemblyQualifiedName;
            }
        }
#endif
    }

    public FieldInfo GetFieldInfo()
    {
        var type = GetAttributeSetType();
        if (type == null) return null;

        return type.GetField(attributeFieldName, BindingFlags.Public | BindingFlags.Instance);
    }
}
