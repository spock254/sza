using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsInit : MonoBehaviour
{
    [HideInInspector]
    public ISkill cooking { get; private set; }

    private void Awake()
    {
        cooking = new Skill();
    }

}
