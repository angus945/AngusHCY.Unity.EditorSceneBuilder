using UnityEngine;

namespace AngusHCY.EditorSceneBuilder
{
    internal static class SceneObjectFactory
    {
        internal static Camera CreateOrthographicCamera(
            string name,
            Vector3 position,
            float orthographicSize,
            Color backgroundColor,
            string tagName = "MainCamera")
        {
            GameObject cameraObject = new GameObject(name);
            cameraObject.tag = tagName;
            cameraObject.transform.position = position;

            Camera camera = cameraObject.AddComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = orthographicSize;
            camera.backgroundColor = backgroundColor;

            return camera;
        }

        internal static GameObject CreateSpriteObject(
            string objectName,
            Vector3 position,
            Sprite sprite,
            int sortingOrder = 0)
        {
            GameObject gameObject = new GameObject(objectName);
            gameObject.transform.position = position;

            SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
            spriteRenderer.sortingOrder = sortingOrder;

            return gameObject;
        }

        internal static CircleCollider2D AddCircleCollider(
            GameObject target,
            bool isTrigger,
            float radius = 0.5f)
        {
            CircleCollider2D collider = target.AddComponent<CircleCollider2D>();
            collider.isTrigger = isTrigger;
            collider.radius = radius;
            return collider;
        }

        internal static Rigidbody2D AddRigidbody2D(
            GameObject target,
            RigidbodyType2D bodyType,
            float gravityScale = 0f,
            bool freezeRotation = true,
            CollisionDetectionMode2D collisionDetectionMode = CollisionDetectionMode2D.Discrete)
        {
            Rigidbody2D rigidbody2D = target.AddComponent<Rigidbody2D>();
            rigidbody2D.bodyType = bodyType;
            rigidbody2D.gravityScale = gravityScale;
            rigidbody2D.freezeRotation = freezeRotation;
            rigidbody2D.collisionDetectionMode = collisionDetectionMode;
            return rigidbody2D;
        }
    }
}
