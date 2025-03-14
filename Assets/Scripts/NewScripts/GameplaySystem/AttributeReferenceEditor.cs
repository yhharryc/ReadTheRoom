using UnityEngine;
using UnityEditor;
using System;
#if UNITY_EDITOR
[CustomEditor(typeof(AttributeReference))]
public class AttributeReferenceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Cast target to your ScriptableObject type
        var attrRef = (AttributeReference)target;

        // Display the MonoScript field
        EditorGUILayout.LabelField("Pick AttributeSet Script (Optional)", EditorStyles.boldLabel);
        MonoScript newScript = (MonoScript)EditorGUILayout.ObjectField(
            "Attribute Set Script",
            attrRef.attributeSetScript,
            typeof(MonoScript),
            false
        );

        // If the user changed the script, update the reference
        if (newScript != attrRef.attributeSetScript)
        {
            Undo.RecordObject(attrRef, "Change AttributeReference Script");
            attrRef.attributeSetScript = newScript;

#if UNITY_EDITOR
            // If assigned, store the AssemblyQualifiedName into typeName
            if (attrRef.attributeSetScript != null)
            {
                var t = attrRef.attributeSetScript.GetClass();
                if (t != null)
                {
                    attrRef.typeName = t.AssemblyQualifiedName;
                }
            }
#endif
        }

        EditorGUILayout.Space();

        // Show the typeName as a text field, so you can override or type manually
        EditorGUILayout.LabelField("Type Name (Runtime Fallback)", EditorStyles.boldLabel);
        string newTypeName = EditorGUILayout.TextField("Type Name", attrRef.typeName);
        if (newTypeName != attrRef.typeName)
        {
            Undo.RecordObject(attrRef, "Change AttributeReference TypeName");
            attrRef.typeName = newTypeName;
        }

        EditorGUILayout.Space();

        // Show the attributeFieldName text field
        EditorGUILayout.LabelField("Field Name", EditorStyles.boldLabel);
        string newFieldName = EditorGUILayout.TextField("Attribute Field Name", attrRef.attributeFieldName);
        if (newFieldName != attrRef.attributeFieldName)
        {
            Undo.RecordObject(attrRef, "Change AttributeReference FieldName");
            attrRef.attributeFieldName = newFieldName;
        }

        EditorGUILayout.Space();

        // Optionally display whether IsValid is true or false
        EditorGUILayout.LabelField("IsValid: " + attrRef.IsValid());

        // Apply any changed fields
        if (GUI.changed)
        {
            EditorUtility.SetDirty(attrRef);
        }
    }
}
#endif