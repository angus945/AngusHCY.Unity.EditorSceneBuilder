using UnityEngine;

namespace AngusHCY.EditorSceneBuilder.Example
{
    public class ExampleEntityConfig : ScriptableObject
    {
        [Header("Identity")]
        public string displayName;

        [Header("Stats")]
        public float moveSpeed;
        public int maxHealth;

        [Header("Visuals")]
        public Sprite sprite;
        public Color color = Color.white;
    }
}
