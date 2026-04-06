using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace AngusHCY.EditorSceneBuilder
{
    public static class SceneBuildUtility
    {
        public static Scene CreateEmptyScene(NewSceneMode newSceneMode = NewSceneMode.Single)
        {
            return EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, newSceneMode);
        }

        public static void SaveScene(Scene scene, string scenePath, bool refreshAssetDatabase = true)
        {
            EditorSceneManager.SaveScene(scene, scenePath);

            if (refreshAssetDatabase)
            {
                AssetDatabase.Refresh();
            }
        }
    }
}
