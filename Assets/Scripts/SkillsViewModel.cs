using System;
using System.Collections.Generic;
using Skills.Data;
using UnityEngine;

namespace Skills
{
    public class SkillsViewModel : MonoBehaviour
    {
        [SerializeField] private SkillsCore _skills;

        private bool _canLearnSkill;
        private bool _canForgetSkill;
        private bool _hasEnoughPoints;

        public event Action SkillsPointsChanged;
        public event Action CurrentSkillChanged;

        public Skill CurrentSkill { get; private set; }
        public int SkillsPointsAmount => _skills.SkillsPointsInfo.Amount;

        public void Init()
        {
            _skills.ResetState();
            _skills.SkillsPointsInfo.AmountChanged += UpdatePoints;
        }

        public void SelectSkill(Skill skill)
        {
            if (CurrentSkill != null)
            {
                _skills.SkillsStateInfo.GetSkillState(CurrentSkill).StateChanged -= OnCurrentSkillStateChanged;
            }
            CurrentSkill = skill;
            _skills.SkillsStateInfo.GetSkillState(CurrentSkill).StateChanged += OnCurrentSkillStateChanged;
            UpdateCurrentSkillPointsAreEnough();
            UpdateSkillPossibleControls();
            CurrentSkillChanged?.Invoke();
        }

        public void IncreasePoints()
        {
            _skills.AddPoints(1);
        }

        public void UpdateSkillControls(ICurrentSkillControls skillControls)
        {
            skillControls.UpdateLearnControl(_canLearnSkill);
            skillControls.UpdateForgetControl(_canForgetSkill);
        }

        public void ProcessLearn()
        {
            _skills.LearnSkill(CurrentSkill);
        }

        public void ProcessForget()
        {
            _skills.ForgetSkill(CurrentSkill);
        }

        public void ProcessForgetAll()
        {
            _skills.ForgetAllSkills();
        }

        public IList<Skill> GetSkills()
        {
            return _skills.SkillsStateInfo.GetSkills();
        }

        public IList<UndirectSkillsPair> GetSkillsConnections()
        {
            return _skills.SkillsStateInfo.GetSkillsConnections();
        }

        public ISkillState GetSkillState(Skill skill)
        {
            return _skills.SkillsStateInfo.GetSkillState(skill);
        }

        private void OnCurrentSkillStateChanged()
        {
            UpdateSkillPossibleControls();
            CurrentSkillChanged?.Invoke();
        }

        private void UpdateSkillPossibleControls()
        {
            _canLearnSkill = _skills.CanLearnSkill(CurrentSkill);
            _canForgetSkill = _skills.CanForgetSkill(CurrentSkill);
        }

        private void UpdatePoints()
        {
            SkillsPointsChanged?.Invoke();
            bool hadEnoughPoints = _hasEnoughPoints;
            UpdateCurrentSkillPointsAreEnough();
            if (hadEnoughPoints != _hasEnoughPoints)
            {
                OnCurrentSkillStateChanged();
            }
        }

        private void UpdateCurrentSkillPointsAreEnough()
        {
            if (CurrentSkill != null)
            {
                _hasEnoughPoints = _skills.SkillsPointsInfo.Amount >= CurrentSkill.Cost;
            }
        }
    }
}