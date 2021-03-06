﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ISwitchItem
{
    void SwitchItem(Item itemToSwitch, Button hand);
    Item GetNeedItem();
    string GetISwitchName();
}
