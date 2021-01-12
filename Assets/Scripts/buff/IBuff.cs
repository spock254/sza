using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuff
{
    void Buff();
    void Debuff();
    void SetRebuff(IBuff rebuff);
    bool IsBuffed();
}
