using System.Collections.Generic;

namespace Skills.Data
{
    public interface ISkillsStateInfo
    {
        ISkillState GetSkillState(Skill skill);
        IList<Skill> GetSkills();
        IList<UndirectSkillsPair> GetSkillsConnections();
    }
}