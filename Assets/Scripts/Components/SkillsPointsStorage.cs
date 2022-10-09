using System;
using Skills.Data;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "SkillsPointsStorage", menuName = "Services/Skills Points Storage")]
    public class SkillsPointsStorage : ScriptableObject, IPointsInfo
    {
        [SerializeField] private Points _points;

        int IPointsInfo.Amount => _points.FreePoints;

        public event Action AmountChanged;

        public void ResetState()
        {
            _points.ResetPoints();
        }

        public bool CanLearnSkill(Skill skill)
        {
            return _points.CanUsePoints(skill.Cost);
        }

        public void LearnSkill(Skill skill)
        {
            _points.UsePoints(skill.Cost);
            AmountChanged?.Invoke();
        }

        public void ForgetSkill(Skill skill)
        {
            _points.ReturnPoints(skill.Cost);
            AmountChanged?.Invoke();
        }

        public void ForgetAllSkills()
        {
            _points.ResetUsedPoints();
            AmountChanged?.Invoke();
        }

        public void EarnPoints(int amount)
        {
            _points.AddPoints(amount);
            AmountChanged?.Invoke();
        }
    }
}