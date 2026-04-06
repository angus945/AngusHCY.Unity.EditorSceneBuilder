using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AngusHCY.EditorSceneBuilder
{
    public enum SpriteShape
    {
        Square,
        Circle,
        Triangle,
        Diamond,
        Capsule,
        Ring,
    }

    public static class GeneratedSpriteAssetUtility
    {
        private const string SharedSpriteDirectory = "Assets/z_Gen/Sprites";
        private const int TextureSize = 64;
        private const float PixelsPerUnit = 64f;

        public static Sprite GetOrCreateShapeSprite(SpriteShape shape)
        {
            string assetPath = GetAssetPath(shape);

            Sprite existing = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
            if (existing != null)
            {
                return existing;
            }

            AssetFolderUtility.EnsureFolder(SharedSpriteDirectory);

            Texture2D texture = new Texture2D(TextureSize, TextureSize, TextureFormat.RGBA32, false);
            Color[] pixels = GenerateShapePixels(shape, TextureSize);
            texture.SetPixels(pixels);
            texture.Apply();

            byte[] pngData = texture.EncodeToPNG();
            string fullPath = Path.Combine(Application.dataPath, "..", assetPath);
            File.WriteAllBytes(fullPath, pngData);

            UnityEngine.Object.DestroyImmediate(texture);

            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceSynchronousImport);

            TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(assetPath);
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.spritePixelsPerUnit = PixelsPerUnit;
            importer.filterMode = FilterMode.Bilinear;
            importer.SaveAndReimport();

            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
            if (sprite == null)
            {
                throw new InvalidOperationException($"Failed to load generated sprite at '{assetPath}'.");
            }

            return sprite;
        }

        private static string GetAssetPath(SpriteShape shape)
        {
            return $"{SharedSpriteDirectory}/Shape_{shape}.png";
        }

        private static Color[] GenerateShapePixels(SpriteShape shape, int size)
        {
            Color[] pixels = new Color[size * size];
            float center = (size - 1) * 0.5f;
            float radius = center;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    bool inside = IsInsideShape(shape, x, y, size, center, radius);
                    pixels[y * size + x] = inside ? Color.white : Color.clear;
                }
            }

            return pixels;
        }

        private static bool IsInsideShape(SpriteShape shape, int x, int y, int size, float center, float radius)
        {
            float dx = x - center;
            float dy = y - center;

            switch (shape)
            {
                case SpriteShape.Square:
                    return true;

                case SpriteShape.Circle:
                    return dx * dx + dy * dy <= radius * radius;

                case SpriteShape.Triangle:
                    float normalizedX = (float)x / (size - 1);
                    float normalizedY = (float)y / (size - 1);
                    float halfWidth = normalizedY * 0.5f;
                    return normalizedX >= 0.5f - halfWidth && normalizedX <= 0.5f + halfWidth;

                case SpriteShape.Diamond:
                    return Mathf.Abs(dx) + Mathf.Abs(dy) <= radius;

                case SpriteShape.Capsule:
                    float capsuleHalfHeight = radius * 0.5f;
                    float capsuleRadius = radius * 0.5f;
                    if (dy > capsuleHalfHeight)
                    {
                        return dx * dx + (dy - capsuleHalfHeight) * (dy - capsuleHalfHeight) <= capsuleRadius * capsuleRadius;
                    }
                    if (dy < -capsuleHalfHeight)
                    {
                        return dx * dx + (dy + capsuleHalfHeight) * (dy + capsuleHalfHeight) <= capsuleRadius * capsuleRadius;
                    }
                    return Mathf.Abs(dx) <= capsuleRadius;

                case SpriteShape.Ring:
                    float distSq = dx * dx + dy * dy;
                    float outerRadius = radius;
                    float innerRadius = radius * 0.65f;
                    return distSq <= outerRadius * outerRadius && distSq >= innerRadius * innerRadius;

                default:
                    return true;
            }
        }
    }
}
