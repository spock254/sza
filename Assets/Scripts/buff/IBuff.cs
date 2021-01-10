using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuff
{
    void Buff();
    void Debuff(IBuff buff = null);
}
