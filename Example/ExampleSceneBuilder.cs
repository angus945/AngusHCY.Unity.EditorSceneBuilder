using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AngusHCY.EditorSceneBuilder
{
    public static class ExampleSceneBuilder
    {
        private const string SceneDirectory = "Assets/_GeneratedScenes/Examples";
        private const string ScenePath = SceneDirectory + "/Example_GeneratedScene.unity";

        [MenuItem("Tools/Examples/Build Standalone Example Scene", priority = 100)]
        public static void Build()
        {
            EnsureFolder(SceneDirectory);

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            Camera camera = CreateCamera();
            CreateDirectionalLight();
            CreateGround();
            CreateDemoObjects();
            CreateSpawnMarker();
            CreateLabelRoot();

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.Refresh();

            Debug.Log(
                "[ExampleSceneBuilder] Scene generated successfully.\n" +
                $"Scene Path: {ScenePath}\n" +
                "\nWhat this scene contains:\n" +
                "  1. Main Camera\n" +
                "  2. Directional Light\n" +
                "  3. Ground Plane\n" +
                "  4. A few primitive demo objects\n" +
                "  5. A spawn marker object");
        }

        private static Camera CreateCamera()
        {
            GameObject cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            cameraObject.transform.position = new Vector3(0f, 6f, -10f);
            cameraObject.transform.rotation = Quaternion.Euler(20f, 0f, 0f);

            Camera camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.Skybox;
            camera.fieldOfView = 60f;
            camera.nearClipPlane = 0.1f;
            camera.farClipPlane = 1000f;

            return camera;
        }

        private static void CreateDirectionalLight()
        {
            GameObject lightObject = new GameObject("Directional Light");
            lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

            Light light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.1f;
        }

        private static void CreateGround()
        {
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.position = Vector3.zero;
            ground.transform.localScale = new Vector3(2f, 1f, 2f);
        }

        private static void CreateDemoObjects()
        {
            CreatePrimitive(
                PrimitiveType.Cube,
                "Demo_Cube_A",
                new Vector3(-2f, 0.5f, 0f),
                new Vector3(1f, 1f, 1f));

            CreatePrimitive(
                PrimitiveType.Cube,
                "Demo_Cube_B",
                new Vector3(2f, 0.5f, 1.5f),
                new Vector3(1f, 2f, 1f));

            CreatePrimitive(
                PrimitiveType.Sphere,
                "Demo_Sphere",
                new Vector3(0f, 0.75f, 3f),
                new Vector3(1.5f, 1.5f, 1.5f));

            CreatePrimitive(
                PrimitiveType.Capsule,
                "Demo_Capsule",
                new Vector3(0f, 1f, -2.5f),
                new Vector3(1f, 2f, 1f));
        }

        private static void CreateSpawnMarker()
        {
            GameObject spawnMarker = new GameObject("SpawnPoint");
            spawnMarker.transform.position = new Vector3(0f, 0f, -4f);

            GameObject visual = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            visual.name = "SpawnPoint_Visual";
            visual.transform.SetParent(spawnMarker.transform);
            visual.transform.localPosition = new Vector3(0f, 0.1f, 0f);
            visual.transform.localScale = new Vector3(0.5f, 0.1f, 0.5f);

            Collider collider = visual.GetComponent<Collider>();
            if (collider != null)
            {
                Object.DestroyImmediate(collider);
            }
        }

        private static void CreateLabelRoot()
        {
            GameObject root = new GameObject("GeneratedScene_Root");

            CreateEmptyChild(root.transform, "Environment");
            CreateEmptyChild(root.transform, "Gameplay");
            CreateEmptyChild(root.transform, "UI_Placeholder");
        }

        private static GameObject CreatePrimitive(
            PrimitiveType primitiveType,
            string objectName,
            Vector3 position,
            Vector3 scale)
        {
            GameObject gameObject = GameObject.CreatePrimitive(primitiveType);
            gameObject.name = objectName;
            gameObject.transform.position = position;
            gameObject.transform.localScale = scale;

            return gameObject;
        }

        private static GameObject CreateEmptyChild(Transform parent, string objectName)
        {
            GameObject gameObject = new GameObject(objectName);
            gameObject.transform.SetParent(parent);
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
            gameObject.transform.localScale = Vector3.one;

            return gameObject;
        }

        private static void EnsureFolder(string assetPath)
        {
            if (AssetDatabase.IsValidFolder(assetPath))
            {
                return;
            }

            string[] segments = assetPath.Split('/');
            string currentPath = segments[0];

            for (int index = 1; index < segments.Length; index++)
            {
                string nextPath = currentPath + "/" + segments[index];
                if (!AssetDatabase.IsValidFolder(nextPath))
                {
                    AssetDatabase.CreateFolder(currentPath, segments[index]);
                }

                currentPath = nextPath;
            }
        }
    }
}
