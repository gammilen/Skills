using UnityEngine;

namespace Skills.Data
{
    [CreateAssetMenu(fileName = "Skill", menuName = "Data/Skill")]
    public class Skill : ScriptableObject
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string ShortName { get; private set; }
        [field: SerializeField] public int Cost { get; private set; }

        public void RegenerateId()
        {
            Id = new System.Guid().ToString();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            Cost = Mathf.Abs(Cost);
        }
#endif
    }
}