using System;
using UnityEditor;
using UnityEngine;

namespace AngusHCY.EditorSceneBuilder
{
    internal static class ScriptableObjectAssetUtility
    {
        internal static T LoadOrCreateAsset<T>(string assetPath, Func<T> createAssetFunc)
            where T : ScriptableObject
        {
            T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset != null)
            {
                return asset;
            }

            asset = createAssetFunc();
            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

            T reloadedAsset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (reloadedAsset == null)
            {
                throw new InvalidOperationException($"Failed to reload asset at '{assetPath}'.");
            }

            return reloadedAsset;
        }
    }
}
