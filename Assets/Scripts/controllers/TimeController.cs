using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    int tics = 0;
    int hour = 0;
    int days = 0;

    EventController eventController;
    void Start()
    {
        eventController = Global.Component.GetEventController();

        StartCoroutine(Timeflow());
    }

    IEnumerator Timeflow() 
    {
        while (true) 
        {

            tics++;
            yield return new WaitForSeconds(1f);
            
            if (tics == Global.Timeflow.MAX_TICS) 
            {
                hour++;
                tics = 0;
            }

            if (hour == Global.Timeflow.MAX_HOURS) 
            {
                hour = 0;
                days++;
            }

            //Debug.Log("Hour " + hour + " tic " + tics);
        }
    }

    public bool Wait() 
    {
        return false;
    }

    public int GetTic() 
    {
        return tics;
    }

    public int GetHour() 
    {
        return hour;
    }

    public int GetDay() 
    {
        return days;
    }
}
