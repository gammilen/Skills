using Skills;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private SkillsViewModel _skills;

    private void Awake()
    {
        _skills.Init();
    }
}