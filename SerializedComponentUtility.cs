using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace AngusHCY.EditorSceneBuilder
{
    internal static class SerializedComponentUtility
    {
        internal static void SetObjectReference(Component target, string fieldName, UnityEngine.Object value)
        {
            SerializedProperty property = GetProperty(target, fieldName);
            property.objectReferenceValue = value;
            Apply(target, property.serializedObject);
        }

        internal static void SetString(Component target, string fieldName, string value)
        {
            SerializedProperty property = GetProperty(target, fieldName);
            property.stringValue = value;
            Apply(target, property.serializedObject);
        }

        internal static void SetBool(Component target, string fieldName, bool value)
        {
            SerializedProperty property = GetProperty(target, fieldName);
            property.boolValue = value;
            Apply(target, property.serializedObject);
        }

        internal static void SetFloat(Component target, string fieldName, float value)
        {
            SerializedProperty property = GetProperty(target, fieldName);
            property.floatValue = value;
            Apply(target, property.serializedObject);
        }

        internal static void SetInt(Component target, string fieldName, int value)
        {
            SerializedProperty property = GetProperty(target, fieldName);
            property.intValue = value;
            Apply(target, property.serializedObject);
        }

        internal static void SetVector2(Component target, string fieldName, Vector2 value)
        {
            SerializedProperty property = GetProperty(target, fieldName);
            property.vector2Value = value;
            Apply(target, property.serializedObject);
        }

        internal static void SetEnum<TEnum>(Component target, string fieldName, TEnum value)
            where TEnum : struct
        {
            SerializedProperty property = GetProperty(target, fieldName);
            property.intValue = Convert.ToInt32(value);
            Apply(target, property.serializedObject);
        }

        private static SerializedProperty GetProperty(Component target, string fieldName)
        {
            SerializedObject serializedObject = new SerializedObject(target);
            SerializedProperty property = serializedObject.FindProperty(fieldName);

            if (property == null)
            {
                throw new InvalidOperationException(
                    $"Field '{fieldName}' was not found on component '{target.GetType().Name}'.");
            }

            return property;
        }

        private static void Apply(Component target, SerializedObject serializedObject)
        {
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(target);
            MarkSceneDirty(target);
        }

        private static void MarkSceneDirty(Component target)
        {
            if (!target.gameObject.scene.IsValid())
            {
                return;
            }

            EditorSceneManager.MarkSceneDirty(target.gameObject.scene);
        }
    }
}
