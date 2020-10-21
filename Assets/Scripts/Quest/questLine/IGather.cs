using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGather
{
    bool Gather(Item item, GameObject gatherPoint, int count = -1);
}
