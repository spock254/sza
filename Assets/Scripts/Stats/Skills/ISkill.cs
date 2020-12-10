using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{
    void AddExp(float exp);
    CraftType GetCraftType();
    int GetSkillLvl();
    float GetSkillExp();
    float ExpToNextLvl();
    float GetMinCurrentLvlExp();
}
