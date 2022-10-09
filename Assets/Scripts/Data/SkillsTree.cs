using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Skills.Data
{
    [CreateAssetMenu(fileName ="SkillsTree", menuName = "Data/Skills Tree")]
    public class SkillsTree : ScriptableObject
    {
        [SerializeField] private List<Skill> _skills;
        [SerializeField] private List<UndirectSkillsPair> _connections;
        [field: SerializeField] public Skill BaseSkill { get; private set; }


        private HashSet<SkillsPair> _directedConnections;

        public IList<Skill> Skills => _skills;
        public IList<UndirectSkillsPair> SkillsConnections => _connections;

        public void Init()
        {
            FormValidSkills();
            RemovePairsWithInvalidSkills();
            FormDirectConnections();
        }

        public IEnumerable<Skill> GetPairsFromSkill(Skill skill)
        {
            foreach (SkillsPair pair in _directedConnections)
            {
                if (pair.ItemA == skill)
                {
                    yield return pair.ItemB;
                }
            }
        }

        public IEnumerable<Skill> GetPairsToSkill(Skill skill)
        {
            foreach (SkillsPair pair in _directedConnections)
            {
                if (pair.ItemB == skill)
                {
                    yield return pair.ItemA;
                }
            }
        }

        private void FormValidSkills()
        {
            var skills = new HashSet<Skill>(_skills);
            skills.Add(BaseSkill);
            skills.Remove(null);
            if (BaseSkill == null)
            {
                BaseSkill = skills.First();
            }
            _skills = skills.ToList();
            AssignSkillsUniqueIds();
        }

        private void AssignSkillsUniqueIds()
        {
            foreach (Skill skill in _skills)
            {
                foreach (Skill addSkill in _skills)
                {
                    if (skill.Id == addSkill.Id && skill != addSkill)
                    {
                        skill.RegenerateId();
                        break;
                    }
                }
            }
        }

        private void RemovePairsWithInvalidSkills()
        {
            for (int i = _connections.Count - 1; i >= 0; i--)
            {
                if (!HasSkillsInTree(_connections[i]) ||
                    _connections[i].ItemA == _connections[i].ItemB)
                {
                    _connections.RemoveAt(i);
                }
            }
        }

        private bool HasSkillsInTree(SkillsPair connection)
        {
            return _skills.Contains(connection.ItemA) && _skills.Contains(connection.ItemB);
        }

        private void FormDirectConnections()
        {
            var connections = new HashSet<UndirectSkillsPair>(_connections);
            _connections = connections.ToList();
            _directedConnections = new HashSet<SkillsPair>();
            var currentChain = new List<Skill>();
            FormDirectConnections(BaseSkill, currentChain);
        }

        private void FormDirectConnections(Skill skill, List<Skill> usedSkills, Skill exceptSkill = null)
        {
            usedSkills.Add(skill);
            foreach (Skill pairedSkill in GetSkillPairs(skill, exceptSkill))
            {
                if (pairedSkill != BaseSkill && !usedSkills.Contains(pairedSkill))
                {
                    _directedConnections.Add(new SkillsPair(skill, pairedSkill));
                    FormDirectConnections(pairedSkill, new List<Skill>(usedSkills), skill);
                }
            }
        }

        private IEnumerable<Skill> GetSkillPairs(Skill skill, Skill exceptSkill = null)
        {
            foreach (SkillsPair connection in _connections)
            {
                if (connection.Contains(skill) && 
                    (exceptSkill == null || !connection.Contains(exceptSkill)))
                {
                    yield return connection.ItemA == skill ? connection.ItemB : connection.ItemA;
                }
            }
        }
    }
}