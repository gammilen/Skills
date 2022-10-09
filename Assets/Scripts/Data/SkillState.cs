using System;
using System.Collections.Generic;

namespace Skills.Data
{
    public class SkillState : ISkillState
    {
        public readonly HashSet<Skill> InPairs = new HashSet<Skill>();
        public readonly HashSet<Skill> OutPairs = new HashSet<Skill>();
        public bool IsLearned { get; private set; }
        public Skill Skill { get; private set; }

        public event Action StateChanged;

        public SkillState(Skill skill)
        {
            Skill = skill;
        }

        public void SetLearned(bool state)
        {
            if (state == IsLearned)
            {
                return;
            }
            IsLearned = state;
            StateChanged?.Invoke();
        }

        public void ResetState()
        {
            IsLearned = false;
            InPairs.Clear();
            OutPairs.Clear();
        }

        public void ForceStateChanged()
        {
            StateChanged?.Invoke();
        }
    }
}