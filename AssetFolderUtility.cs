using System.IO;
using UnityEditor;
using UnityEngine;

namespace AngusHCY.EditorSceneBuilder
{
    internal static class AssetFolderUtility
    {
        internal static void EnsureFolders(params string[] assetPaths)
        {
            for (int index = 0; index < assetPaths.Length; index++)
            {
                EnsureFolder(assetPaths[index]);
            }

            AssetDatabase.Refresh();
        }

        internal static void EnsureFolder(string assetPath)
        {
            string fullPath = Path.Combine(Application.dataPath, "..", assetPath);

            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
        }
    }
}
