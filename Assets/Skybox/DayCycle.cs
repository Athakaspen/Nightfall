using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour
{

    [SerializeField] private Transform sun;
    [SerializeField] private Transform moon;
    [SerializeField] private float dayDuration = 16f; //seconds
    private float m_dayProgress = 0f; // range 0-1
    private int m_dayCount = 0;
    private float m_dayDegrees = 210f; 
    // how many degrees out of 360 count as daytime

    public float DayProgress{
        get{
            return m_dayProgress;
        }
    }
    public float DayCount{
        get{
            return m_dayCount;
        }
    }

    public bool isNight{
        get{
            return m_dayProgress*360f>90f+m_dayDegrees/2 && m_dayProgress*360f<90f-m_dayDegrees/2+360f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // update sun/moon positions
        m_dayProgress += Time.deltaTime / dayDuration;
        sun.transform.rotation = Quaternion.Euler(360f*m_dayProgress, 0f, 0f);
        moon.transform.rotation = Quaternion.Euler(360f*(m_dayProgress+.5f), 0f, 0f);

        // update day count
        if (m_dayProgress>1) {
            m_dayProgress-=1;
            m_dayCount += 1;
        }
    }
}
