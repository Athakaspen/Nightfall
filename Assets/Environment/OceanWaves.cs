using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanWaves : MonoBehaviour
{
    public float tidalShift = 0.15f;
    public float randomShift = 0.02f;
    public float baseHeight = 1.25f;
    public DayNightCycle dayNightCycle;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, baseHeight + Mathf.Cos(dayNightCycle.time*4*Mathf.PI)*tidalShift + Mathf.Sin(dayNightCycle.time*35.9f*Mathf.PI)*randomShift, transform.position.z);
    }
}
