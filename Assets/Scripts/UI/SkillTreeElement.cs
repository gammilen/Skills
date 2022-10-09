using Skills.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SkillTreeElement : MonoBehaviour
    {
        [SerializeField] private Color _learned;
        [SerializeField] private Color _notLearned;
        [SerializeField] private Image _bg;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private GameObject _selection;
        [SerializeField] private Button _button;

        private ISkillState _skillState;

        public event Action<Skill> Clicked;

        private void Awake()
        {
            _button.onClick.AddListener(ProcessClick);
        }

        public void Setup(ISkillState skillState)
        {
            _skillState = skillState;
            _skillState.StateChanged += RedrawBackground;
            Redraw();
        }

        public void SetSelected(bool isSelected)
        {
            _selection.SetActive(isSelected);
        }

        private void Redraw()
        {
            _name.text = _skillState.Skill.ShortName;
            RedrawBackground();
        }

        private void RedrawBackground()
        {
            _bg.color = _skillState.IsLearned ? _learned : _notLearned;
        }

        private void ProcessClick()
        {
            Clicked?.Invoke(_skillState.Skill);
        }
    }
}