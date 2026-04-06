using AngusHCY.EditorSceneBuilder;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AngusHCY.EditorSceneBuilder.Example
{
    public static class ExampleComponentSceneBuilder
    {
        private const string SceneDirectory = "Assets/_GeneratedScenes/Examples";
        private const string AssetDirectory = "Assets/_GeneratedScenes/Examples/ComponentDemo";
        private const string ScenePath = SceneDirectory + "/Example_ComponentScene.unity";

        [MenuItem("Tools/Examples/Build Component Demo Scene", priority = 102)]
        public static void Build()
        {
            AssetFolderUtility.EnsureFolders(SceneDirectory, AssetDirectory);

            Scene scene = SceneBuildUtility.CreateEmptyScene();

            SceneObjectFactory.CreateOrthographicCamera(
                "Main Camera",
                new Vector3(0f, 0f, -10f),
                orthographicSize: 6f,
                backgroundColor: new Color(0.08f, 0.08f, 0.15f));

            Sprite circleSprite = GeneratedSpriteAssetUtility.GetOrCreateShapeSprite(SpriteShape.Circle);
            Sprite diamondSprite = GeneratedSpriteAssetUtility.GetOrCreateShapeSprite(SpriteShape.Diamond);
            Sprite triangleSprite = GeneratedSpriteAssetUtility.GetOrCreateShapeSprite(SpriteShape.Triangle);

            ExampleEntityConfig playerConfig = CreateEntityConfig(
                "PlayerConfig", "Player", circleSprite, Color.green, moveSpeed: 5f, maxHealth: 100);

            ExampleEntityConfig enemyConfig = CreateEntityConfig(
                "EnemyConfig", "Enemy", diamondSprite, Color.red, moveSpeed: 3f, maxHealth: 50);

            ExampleEntityConfig patrolConfig = CreateEntityConfig(
                "PatrolConfig", "Patrol", triangleSprite, Color.cyan, moveSpeed: 2f, maxHealth: 30);

            BuildEntity(
                "Player", new Vector3(0f, -2f, 0f), playerConfig,
                speedMultiplier: 1f, isActive: true, patrolRange: Vector2.zero, sortingOrder: 5);

            BuildEntity(
                "Enemy_A", new Vector3(-3f, 1f, 0f), enemyConfig,
                speedMultiplier: 1.2f, isActive: true, patrolRange: new Vector2(2f, 0f), sortingOrder: 3);

            BuildEntity(
                "Enemy_B", new Vector3(3f, 2f, 0f), enemyConfig,
                speedMultiplier: 0.8f, isActive: true, patrolRange: new Vector2(0f, 3f), sortingOrder: 3);

            BuildEntity(
                "Patrol_A", new Vector3(-5f, -1f, 0f), patrolConfig,
                speedMultiplier: 1f, isActive: false, patrolRange: new Vector2(4f, 0f), sortingOrder: 2);

            EditorDirtyUtility.MarkSceneDirty(scene);
            SceneBuildUtility.SaveScene(scene, ScenePath);

            Debug.Log(
                "[ExampleComponentSceneBuilder] Component demo scene generated.\n" +
                $"Scene Path: {ScenePath}\n" +
                $"Assets: {AssetDirectory}\n" +
                "\nDemonstrates:\n" +
                "  - ScriptableObjectAssetUtility: create ExampleEntityConfig assets\n" +
                "  - SerializedComponentUtility: assign config + set serialized fields\n" +
                "  - GeneratedSpriteAssetUtility: shape sprites (Circle, Diamond, Triangle)\n" +
                "  - SpriteRenderer.color tinting on white sprites");
        }

        private static ExampleEntityConfig CreateEntityConfig(
            string assetName,
            string displayName,
            Sprite sprite,
            Color color,
            float moveSpeed,
            int maxHealth)
        {
            string assetPath = $"{AssetDirectory}/{assetName}.asset";

            ExampleEntityConfig config = ScriptableObjectAssetUtility.LoadOrCreateAsset(
                assetPath,
                () =>
                {
                    ExampleEntityConfig newConfig = ScriptableObject.CreateInstance<ExampleEntityConfig>();
                    newConfig.displayName = displayName;
                    newConfig.moveSpeed = moveSpeed;
                    newConfig.maxHealth = maxHealth;
                    newConfig.sprite = sprite;
                    newConfig.color = color;
                    return newConfig;
                });

            return config;
        }

        private static void BuildEntity(
            string name,
            Vector3 position,
            ExampleEntityConfig config,
            float speedMultiplier,
            bool isActive,
            Vector2 patrolRange,
            int sortingOrder)
        {
            GameObject go = SceneObjectFactory.CreateSpriteObject(
                name, position, config.sprite, sortingOrder);

            go.GetComponent<SpriteRenderer>().color = config.color;

            ExampleEntity entity = go.AddComponent<ExampleEntity>();

            SerializedComponentUtility.SetObjectReference(entity, "config", config);
            SerializedComponentUtility.SetFloat(entity, "speedMultiplier", speedMultiplier);
            SerializedComponentUtility.SetBool(entity, "isActive", isActive);
            SerializedComponentUtility.SetVector2(entity, "patrolRange", patrolRange);
        }
    }
}
