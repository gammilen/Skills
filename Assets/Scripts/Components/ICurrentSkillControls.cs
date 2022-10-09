namespace Skills
{
    public interface ICurrentSkillControls
    {
        void UpdateLearnControl(bool isActive);
        void UpdateForgetControl(bool isActive);
    }
}