using Skills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PointsView : MonoBehaviour
    {
        [SerializeField] private SkillsViewModel _skills;
        [SerializeField] private TextMeshProUGUI _currentPointsIndicator;
        [SerializeField] private Button _increaseBtn;

        private void Start()
        {
            _increaseBtn.onClick.AddListener(Increase);
            _skills.SkillsPointsChanged += UpdateAmount;
            UpdateAmount();
        }

        private void Increase()
        {
            _skills.IncreasePoints();
        }

        public void UpdateAmount()
        {
            _currentPointsIndicator.text = _skills.SkillsPointsAmount.ToString();
        }
    }
}