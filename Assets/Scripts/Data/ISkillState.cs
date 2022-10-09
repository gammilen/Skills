using System;

namespace Skills.Data
{
    public interface ISkillState
    {
        Skill Skill { get; }
        bool IsLearned { get; }
        event Action StateChanged;
    }
}