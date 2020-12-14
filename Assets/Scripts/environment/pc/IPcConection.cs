using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPcConection
{
    bool IsConected(PCController pcController, GameObject obj);
    void Connect(PCController pcController, GameObject obj);
    void Disconnect(PCController pcController, GameObject obj);
}
