using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace AngusHCY.EditorSceneBuilder
{
    internal static class SceneBuildUtility
    {
        internal static Scene CreateEmptyScene(NewSceneMode newSceneMode = NewSceneMode.Single)
        {
            return EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, newSceneMode);
        }

        internal static void SaveScene(Scene scene, string scenePath, bool refreshAssetDatabase = true)
        {
            EditorSceneManager.SaveScene(scene, scenePath);

            if (refreshAssetDatabase)
            {
                AssetDatabase.Refresh();
            }
        }
    }
}
