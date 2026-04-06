using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AngusHCY.EditorSceneBuilder
{
    internal static class EditorDirtyUtility
    {
        internal static void MarkDirty(params Object[] objects)
        {
            for (int index = 0; index < objects.Length; index++)
            {
                Object current = objects[index];
                if (current == null)
                {
                    continue;
                }

                EditorUtility.SetDirty(current);
            }
        }

        internal static void MarkSceneDirty(Scene scene)
        {
            EditorSceneManager.MarkSceneDirty(scene);
        }

        internal static void MarkDirtyAndScene(Scene scene, params Object[] objects)
        {
            MarkDirty(objects);
            MarkSceneDirty(scene);
        }
    }
}
