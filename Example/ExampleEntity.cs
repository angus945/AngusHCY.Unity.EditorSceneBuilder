using UnityEngine;

namespace AngusHCY.EditorSceneBuilder.Example
{
    public class ExampleEntity : MonoBehaviour
    {
        [SerializeField] private ExampleEntityConfig config;
        [SerializeField] private float speedMultiplier = 1f;
        [SerializeField] private bool isActive = true;
        [SerializeField] private Vector2 patrolRange;

        public ExampleEntityConfig Config => config;
        public float EffectiveSpeed => config != null ? config.moveSpeed * speedMultiplier : 0f;
        public bool IsActive => isActive;
        public Vector2 PatrolRange => patrolRange;
    }
}
