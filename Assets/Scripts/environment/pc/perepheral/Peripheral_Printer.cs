using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peripheral_Printer : MonoBehaviour, IPeripheral
{
    public string DeviseDescription()
    {
        return "printer";
    }
}
