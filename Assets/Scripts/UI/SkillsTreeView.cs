using Skills;
using Skills.Data;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class SkillsTreeView : MonoBehaviour
    {
        [System.Serializable]
        private class SkillPosition
        {
            public Skill Skill;
            public Vector2 Position;
        }   
        
        [SerializeField] private SkillsViewModel _skills;
        [SerializeField] private SkillTreeElement _skillElementTemplate;
        [SerializeField] private ConnectionElement _connectionElementTemplate;
        [SerializeField] private List<SkillPosition> _currentTreeSkillsPositions;

        private Dictionary<Skill, SkillTreeElement> _skillsElements;

        private void Start()
        {
            SetupSkillsElementsPositions();
            CreateConnections();
            CreateSkillsElements();
            _skills.CurrentSkillChanged += SetupSelection;
        }

        [ContextMenu("Apply tree")]
        private void SetupSkillsElementsPositions()
        {
            var treeSkillsPositions = new List<SkillPosition>();
            foreach (Skill skill in _skills.GetSkills())
            {
                var treeSkillPos = new SkillPosition()
                {
                    Skill = skill
                };
                treeSkillsPositions.Add(treeSkillPos);
                foreach (SkillPosition skillPos in _currentTreeSkillsPositions)
                {
                    if (skillPos.Skill == skill)
                    {
                        treeSkillPos.Position = skillPos.Position;
                        break;
                    }
                }
            }
            _currentTreeSkillsPositions = treeSkillsPositions;
        }

        private void CreateSkillsElements()
        {
            _skillsElements = new Dictionary<Skill, SkillTreeElement>();
            foreach (Skill skill in _skills.GetSkills())
            {
                SetupSkillElement(skill, GetPosition(skill));
            }
        }

        private void CreateConnections()
        {
            foreach (UndirectSkillsPair connection in _skills.GetSkillsConnections())
            {
                ConnectionElement elem = Instantiate(_connectionElementTemplate, transform);
                elem.Setup(GetPosition(connection.ItemA).Position, GetPosition(connection.ItemB).Position);
            }
        }

        private SkillPosition GetPosition(Skill skill)
        {
            foreach (SkillPosition skillPos in _currentTreeSkillsPositions)
            {
                if (skillPos.Skill == skill)
                {
                    return skillPos;
                }
            }
            return null;
        }

        private void SetupSkillElement(Skill skill, SkillPosition skillPos)
        {
            SkillTreeElement skillElement = Instantiate(_skillElementTemplate, transform);
            skillElement.transform.localPosition = skillPos.Position;
            skillElement.Setup(_skills.GetSkillState(skill));
            skillElement.Clicked += ProcessSkillSelect;
            _skillsElements.Add(skill, skillElement);
        }

        private void ProcessSkillSelect(Skill skill)
        {
            _skills.SelectSkill(skill);
        }

        private void SetupSelection()
        {
            foreach (var elem in _skillsElements)
            {
                elem.Value.SetSelected(elem.Key == _skills.CurrentSkill);
            }
        }
    }
}