using System;
using UnityEditor;
using UnityEngine;

namespace AngusHCY.EditorSceneBuilder
{
    internal static class PrefabAssetUtility
    {
        internal static T LoadOrCreatePrefabComponent<T>(string prefabPath, Func<GameObject> createPrefabRootFunc) where T : Component
        {
            GameObject existingPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (existingPrefab != null)
            {
                T existingComponent = existingPrefab.GetComponent<T>();
                if (existingComponent != null)
                {
                    return existingComponent;
                }
            }

            GameObject prefabRoot = createPrefabRootFunc();
            GameObject savedPrefab = PrefabUtility.SaveAsPrefabAsset(prefabRoot, prefabPath);
            UnityEngine.Object.DestroyImmediate(prefabRoot);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

            GameObject reloadedPrefab = savedPrefab != null
                ? savedPrefab
                : AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            if (reloadedPrefab == null)
            {
                throw new InvalidOperationException($"Failed to reload prefab at '{prefabPath}'.");
            }

            T component = reloadedPrefab.GetComponent<T>();
            if (component == null)
            {
                throw new InvalidOperationException(
                    $"Prefab at '{prefabPath}' is missing component '{typeof(T).Name}'.");
            }

            return component;
        }
    }
}
