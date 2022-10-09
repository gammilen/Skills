using UnityEngine;

namespace Skills.Data
{
    [System.Serializable]
    public class SkillsPair
    {
        [field: SerializeField] public Skill ItemA;
        [field: SerializeField] public Skill ItemB;

        public SkillsPair(Skill itemA, Skill itemB)
        {
            ItemA = itemA;
            ItemB = itemB;
        }

        public bool Contains(Skill item)
        {
            return ItemA == item || ItemB == item;
        }
    }
}