using Skills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SelectedSkillView : MonoBehaviour, ICurrentSkillControls
    {
        [SerializeField] private SkillsViewModel _skills;

        [SerializeField] private GameObject _panel;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _cost;

        [SerializeField] private Button _learnBtn;
        [SerializeField] private Button _forgetBtn;
        [SerializeField] private Button _forgetAllBtn;


        private void Awake()
        {
            _panel.gameObject.SetActive(false);
            _skills.CurrentSkillChanged += SetupSelectedSkill;
            _learnBtn.onClick.AddListener(_skills.ProcessLearn);
            _forgetBtn.onClick.AddListener(_skills.ProcessForget);
            _forgetAllBtn.onClick.AddListener(_skills.ProcessForgetAll);
        }

        private void SetupSelectedSkill()
        {
            bool active = _skills.CurrentSkill != null;
            _panel.SetActive(active);
            if (!active)
            {
                return;
            }
            _name.text = _skills.CurrentSkill.Name;
            _cost.text = _skills.CurrentSkill.Cost.ToString();
            _panel.SetActive(true);
            _skills.UpdateSkillControls(this);
        }

        void ICurrentSkillControls.UpdateLearnControl(bool isActive)
        {
            _learnBtn.gameObject.SetActive(isActive);
        }

        void ICurrentSkillControls.UpdateForgetControl(bool isActive)
        {
            _forgetBtn.gameObject.SetActive(isActive);
        }
    }
}