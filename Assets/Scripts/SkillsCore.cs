using Skills.Data;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "SkillsSystem", menuName = "Services/Skills System")]
    public class SkillsCore : ScriptableObject
    {
        [SerializeField] private SkillsPointsStorage _points;
        [SerializeField] private SkillsTreeState _skillsTreeState;
        [SerializeField] private SkillsTree _skillsTree;

        public IPointsInfo SkillsPointsInfo => _points;
        public ISkillsStateInfo SkillsStateInfo => _skillsTreeState;

        public void ResetState()
        {
            _points.ResetState();
            _skillsTree.Init();
            _skillsTreeState.InitState(_skillsTree);
        }

        public bool CanForgetSkill(Skill skill)
        {
            return _skillsTreeState.CanForgetSkill(skill);
        }

        public bool CanLearnSkill(Skill skill)
        {
            return _points.CanLearnSkill(skill) && _skillsTreeState.CanLearnSkill(skill);
        }

        public void LearnSkill(Skill skill)
        {
            if (CanLearnSkill(skill))
            {
                _points.LearnSkill(skill);
                _skillsTreeState.LearnSkill(skill);
            }
        }

        public void ForgetSkill(Skill skill)
        {
            if (CanForgetSkill(skill))
            {
                _points.ForgetSkill(skill);
                _skillsTreeState.ForgetSkill(skill);
            }
        }

        public void ForgetAllSkills()
        {
            _points.ForgetAllSkills();
            _skillsTreeState.ForgetAllSkills();
        }

        public void AddPoints(int amount)
        {
            _points.EarnPoints(amount);
        }
    }
}