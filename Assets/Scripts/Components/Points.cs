using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "Points", menuName = "Data/Points")]
    public class Points : ScriptableObject
    {
        public int UsedPoints { get; private set; }
        public int AllPoints { get; private set; }
        public int FreePoints => AllPoints - UsedPoints;

        public void ResetPoints()
        {
            UsedPoints = AllPoints = 0;
        }

        public void ResetUsedPoints()
        {
            UsedPoints = 0;
        }

        public void AddPoints(int amount)
        {
            if (amount > 0)
            {
                AllPoints += amount;
            }
        }

        public void UsePoints(int amount)
        {
            if (CanUsePoints(amount))
            {
                UsedPoints += amount;
            }
        }

        public void ReturnPoints(int amount)
        {
            if (CanReturnPoints(amount))
            {
                UsedPoints -= amount;
            }
        }

        public bool CanUsePoints(int amount)
        {
            return amount >= 0 && FreePoints >= amount;
        }

        private bool CanReturnPoints(int amount)
        {
            return amount >= 0 && UsedPoints >= amount;
        }
    }
}