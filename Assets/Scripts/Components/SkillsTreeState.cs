using System.Collections.Generic;
using UnityEngine;
using Skills.Data;

namespace Skills
{
    [CreateAssetMenu(fileName = "SkillsTreeStatus", menuName = "Services/Skills Tree State")]
    public class SkillsTreeState : ScriptableObject, ISkillsStateInfo
    {
        private SkillsTree _skillsTree;
        private readonly Dictionary<Skill, SkillState> _skillsStates =
            new Dictionary<Skill, SkillState>();

        public void InitState(SkillsTree skillsTree)
        {
            _skillsTree = skillsTree;
            _skillsStates.Clear();
            foreach (Skill skill in _skillsTree.Skills)
            {
                var status = new SkillState(skill);
                if (skill == _skillsTree.BaseSkill)
                {
                    status.SetLearned(true);
                }
                _skillsStates.Add(skill, status);
            }
        }

        public IList<Skill> GetSkills()
        {
            _skillsTree.Init();
            return _skillsTree.Skills;
        }

        public IList<UndirectSkillsPair> GetSkillsConnections()
        {
            return _skillsTree.SkillsConnections;
        }

        public void ForgetAllSkills()
        {
            foreach (SkillState skill in _skillsStates.Values)
            {
                if (skill.Skill != _skillsTree.BaseSkill)
                {
                    skill.ResetState();
                }
            }
            foreach (SkillState skill in _skillsStates.Values)
            {
                skill.ForceStateChanged();
            }
        }

        public IEnumerable<Skill> GetLearnedSkills()
        {
            foreach (var skill in _skillsStates)
            {
                if (skill.Value.IsLearned)
                {
                    yield return skill.Key;
                }
            }
        }

        public ISkillState GetSkillState(Skill skill)
        {
            return GetState(skill);
        }

        public bool CanLearnSkill(Skill skill)
        {
            SkillState skillState = GetState(skill);
            if (skillState.IsLearned)
            {
                return false;
            }
            foreach (Skill pairedSkill in _skillsTree.GetPairsToSkill(skill))
            {
                SkillState pairedSkillStatus = GetState(pairedSkill);
                if (pairedSkillStatus.IsLearned)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CanForgetSkill(Skill skill)
        {
            SkillState skillState = GetState(skill);
            if (!skillState.IsLearned || skill == _skillsTree.BaseSkill)
            {
                return false;
            }
            foreach (Skill outSkill in skillState.OutPairs)
            {
                if (!HasConnectionToBase(outSkill, skill, skill))
                {
                    return false;
                }
            }
            return true;
        }

        public void ForgetSkill(Skill skill)
        {
            SkillState skillStatus = GetState(skill);
            skillStatus.SetLearned(false);
            foreach (Skill inSkill in skillStatus.InPairs)
            {
                GetState(inSkill).OutPairs.Remove(skill);
            }
            foreach (Skill outSkill in skillStatus.OutPairs)
            {
                GetState(outSkill).InPairs.Remove(skill);
            }
        }

        private bool HasConnectionToBase(Skill skill, Skill skillOut, Skill exceptSkillBase)
        {
            foreach (Skill inSkill in GetState(skill).InPairs)
            {
                if (inSkill == _skillsTree.BaseSkill ||
                    inSkill != skillOut && inSkill != exceptSkillBase &&
                    GetState(inSkill).IsLearned && HasConnectionToBase(inSkill, skill, exceptSkillBase))
                {
                    return true;
                }
            }
            return false;
        }

        public void LearnSkill(Skill skill)
        {
            SkillState status = GetState(skill);
            status.SetLearned(true);
            foreach (Skill pairedSkill in _skillsTree.GetPairsToSkill(skill))
            {
                SkillState pairedSkillStatus = GetState(pairedSkill);
                if (pairedSkillStatus.IsLearned)
                {
                    pairedSkillStatus.OutPairs.Add(skill);
                    status.InPairs.Add(pairedSkill);
                }
            }
            foreach (Skill pairedSkill in _skillsTree.GetPairsFromSkill(skill))
            {
                SkillState pairedSkillStatus = GetState(pairedSkill);
                if (pairedSkillStatus.IsLearned)
                {
                    pairedSkillStatus.InPairs.Add(skill);
                    status.OutPairs.Add(pairedSkill);
                }
            }
        }

        private SkillState GetState(Skill skill)
        {
            return _skillsStates[skill];
        }
    }
}