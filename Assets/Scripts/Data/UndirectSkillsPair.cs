namespace Skills.Data
{
    [System.Serializable]
    public class UndirectSkillsPair : SkillsPair
    {
        public UndirectSkillsPair(Skill itemA, Skill itemB) : base(itemA, itemB)
        {
        }

        public override bool Equals(object item)
        {
            return item is UndirectSkillsPair pair &&
                (pair.ItemA == ItemA && pair.ItemB == ItemB ||
                pair.ItemA == ItemB && pair.ItemB == ItemA);
        }

        public override int GetHashCode()
        {
            return ItemA.GetHashCode() + ItemB.GetHashCode();
        }
    }
}