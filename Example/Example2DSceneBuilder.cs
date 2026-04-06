using UnityEngine;
using UnityEngine.SceneManagement;

namespace AngusHCY.EditorSceneBuilder.Example
{
    public static class Example2DSceneBuilder
    {
        private const string SceneDirectory = "Assets/_GeneratedScenes/Examples";
        private const string ScenePath = SceneDirectory + "/Example_2DScene.unity";

        [UnityEditor.MenuItem("Tools/Examples/Build 2D Example Scene", priority = 101)]
        public static void Build()
        {
            AssetFolderUtility.EnsureFolder(SceneDirectory);

            Scene scene = SceneBuildUtility.CreateEmptyScene();

            CreateCamera();
            CreateBackground();
            CreatePlayer();
            CreatePlatforms();
            CreateCollectibles();
            CreateBoundaryWalls();
            CreateSceneHierarchyRoot();

            EditorDirtyUtility.MarkSceneDirty(scene);
            SceneBuildUtility.SaveScene(scene, ScenePath);

            Debug.Log(
                "[Example2DSceneBuilder] 2D scene generated successfully.\n" +
                $"Scene Path: {ScenePath}\n" +
                "\nWhat this scene contains:\n" +
                "  1. Orthographic Camera (2D)\n" +
                "  2. Background (Square sprite)\n" +
                "  3. Player (Circle sprite) with Rigidbody2D + CircleCollider2D\n" +
                "  4. Platforms (Square sprite) with BoxCollider2D\n" +
                "  5. Collectible items (Diamond sprite)\n" +
                "  6. Boundary walls (Square sprite)");
        }

        private static void CreateCamera()
        {
            SceneObjectFactory.CreateOrthographicCamera(
                name: "Main Camera",
                position: new Vector3(0f, 0f, -10f),
                orthographicSize: 5f,
                backgroundColor: new Color(0.1f, 0.1f, 0.2f));
        }

        private static void CreateBackground()
        {
            Sprite bgSprite = GeneratedSpriteAssetUtility.GetOrCreateShapeSprite(SpriteShape.Square);

            GameObject bg = SceneObjectFactory.CreateSpriteObject(
                "Background", Vector3.zero, bgSprite, sortingOrder: -10);
            bg.transform.localScale = new Vector3(20f, 12f, 1f);

            SpriteRenderer renderer = bg.GetComponent<SpriteRenderer>();
            renderer.color = new Color(0.15f, 0.15f, 0.25f);
        }

        private static void CreatePlayer()
        {
            Sprite playerSprite = GeneratedSpriteAssetUtility.GetOrCreateShapeSprite(SpriteShape.Circle);

            GameObject player = SceneObjectFactory.CreateSpriteObject(
                "Player", new Vector3(0f, 1f, 0f), playerSprite, sortingOrder: 5);

            SpriteRenderer renderer = player.GetComponent<SpriteRenderer>();
            renderer.color = new Color(0.2f, 0.8f, 0.4f);

            SceneObjectFactory.AddRigidbody2D(
                player,
                RigidbodyType2D.Dynamic,
                gravityScale: 1f,
                freezeRotation: true,
                CollisionDetectionMode2D.Continuous);

            SceneObjectFactory.AddCircleCollider(player, isTrigger: false, radius: 0.5f);
        }

        private static void CreatePlatforms()
        {
            Sprite platformSprite = GeneratedSpriteAssetUtility.GetOrCreateShapeSprite(SpriteShape.Square);
            Color platformColor = new Color(0.4f, 0.35f, 0.3f);

            CreatePlatform("Platform_Ground", new Vector3(0f, -3f, 0f), new Vector3(12f, 1f, 1f), platformSprite, platformColor);
            CreatePlatform("Platform_Left", new Vector3(-3f, -1f, 0f), new Vector3(3f, 0.5f, 1f), platformSprite, platformColor);
            CreatePlatform("Platform_Right", new Vector3(3f, 0.5f, 0f), new Vector3(3f, 0.5f, 1f), platformSprite, platformColor);
            CreatePlatform("Platform_Top", new Vector3(0f, 2.5f, 0f), new Vector3(2f, 0.5f, 1f), platformSprite, platformColor);
        }

        private static void CreatePlatform(string name, Vector3 position, Vector3 scale, Sprite sprite, Color color)
        {
            GameObject platform = SceneObjectFactory.CreateSpriteObject(name, position, sprite, sortingOrder: 1);
            platform.transform.localScale = scale;
            platform.GetComponent<SpriteRenderer>().color = color;

            BoxCollider2D collider = platform.AddComponent<BoxCollider2D>();
            collider.size = Vector2.one;
        }

        private static void CreateCollectibles()
        {
            Sprite collectibleSprite = GeneratedSpriteAssetUtility.GetOrCreateShapeSprite(SpriteShape.Diamond);
            Color collectibleColor = new Color(1f, 0.85f, 0.2f);

            Vector3[] positions = new Vector3[]
            {
                new Vector3(-3f, 0f, 0f),
                new Vector3(0f, 3.5f, 0f),
                new Vector3(3f, 1.5f, 0f),
                new Vector3(-1f, -2f, 0f),
                new Vector3(2f, -2f, 0f),
            };

            for (int index = 0; index < positions.Length; index++)
            {
                GameObject collectible = SceneObjectFactory.CreateSpriteObject(
                    $"Collectible_{index}", positions[index], collectibleSprite, sortingOrder: 3);
                collectible.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                collectible.GetComponent<SpriteRenderer>().color = collectibleColor;

                SceneObjectFactory.AddCircleCollider(collectible, isTrigger: true, radius: 0.5f);
            }
        }

        private static void CreateBoundaryWalls()
        {
            Sprite wallSprite = GeneratedSpriteAssetUtility.GetOrCreateShapeSprite(SpriteShape.Square);
            Color wallColor = new Color(0.3f, 0.3f, 0.35f);

            CreateWall("Wall_Left", new Vector3(-6.5f, 0f, 0f), new Vector3(1f, 12f, 1f), wallSprite, wallColor);
            CreateWall("Wall_Right", new Vector3(6.5f, 0f, 0f), new Vector3(1f, 12f, 1f), wallSprite, wallColor);
            CreateWall("Wall_Top", new Vector3(0f, 6f, 0f), new Vector3(14f, 1f, 1f), wallSprite, wallColor);
        }

        private static void CreateWall(string name, Vector3 position, Vector3 scale, Sprite sprite, Color color)
        {
            GameObject wall = SceneObjectFactory.CreateSpriteObject(name, position, sprite, sortingOrder: 0);
            wall.transform.localScale = scale;
            wall.GetComponent<SpriteRenderer>().color = color;

            BoxCollider2D collider = wall.AddComponent<BoxCollider2D>();
            collider.size = Vector2.one;
        }

        private static void CreateSceneHierarchyRoot()
        {
            GameObject root = new GameObject("Scene2D_Root");

            CreateEmptyChild(root.transform, "Environment");
            CreateEmptyChild(root.transform, "Gameplay");
            CreateEmptyChild(root.transform, "UI");
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
    }
}
