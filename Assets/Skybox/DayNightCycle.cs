using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public bool titleMode = false;
    public Metalogic metalogic;
    [Range(0.0f, 1.0f)]
    public float time;
    public int dayCount = 0;
    public float fullDayLength;
    public float startTime = 0.4f;
    private float timeRate;
    public Vector3 noon;

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;
    public AnimationCurve shadowIntensity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Stars")]
    public ParticleSystem stars;
    public AnimationCurve starIntensity;

    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMultiplier;
    public AnimationCurve reflectionsIntensityMultiplier;

    [Header("Gulls")]
    public SeagullFlock gulls;
    float[] gullSpawnTimes = new float[0];

    [Header("Crickets")]
    public AnimationCurve cricketVolume;
    AudioSource cricketSound;

    void Start(){
        if (!titleMode) {
            fullDayLength = PersistentData.dayLength;
            generateGullSpawnTimes();
        }
        timeRate = 1.0f/fullDayLength;
        time = startTime;
        cricketSound = GetComponent<AudioSource>();
    }

    void Update(){
        // Increment time
        float prev_time = time;
        time += timeRate*Time.deltaTime;

        if (!titleMode){
            if (prev_time < 0.35 && time > 0.35){
                metalogic.showDayAlert(dayCount+1);
                metalogic.clearCorpses();
            }
            else if (prev_time < 0.7 && time > 0.7){
                metalogic.showNightAlert();
                // spawn some gull sounds for tomorrow
                generateGullSpawnTimes();
            }
            foreach (float spawnTime in gullSpawnTimes){
                if (prev_time < spawnTime+.25 && time > spawnTime+.25){
                    spawnSeagulls();
                }
            }
        }

        if (time >= 1.0f){
            time = 0.0f;
            dayCount += 1;
        }
        
        // Light rotation
        sun.transform.eulerAngles = (time - 0.25f) * noon * 4.0f;
        moon.transform.eulerAngles = (time - 0.75f) * noon * 4.0f;

        // Light intensity
        sun.intensity = sunIntensity.Evaluate(time);
        moon.intensity = moonIntensity.Evaluate(time);

        //shadow intensity
        sun.shadowStrength = shadowIntensity.Evaluate(time);
        
        // change colors
        sun.color = sunColor.Evaluate(time);
        moon.color = moonColor.Evaluate(time);

        // enable / disable sun
        if(sun.intensity == 0 && sun.gameObject.activeInHierarchy)
            sun.gameObject.SetActive(false);
        else if (sun.intensity > 0 && !sun.gameObject.activeInHierarchy){
            sun.gameObject.SetActive(true);
        }

        // enable / disable moon
        if(moon.intensity == 0 && moon.gameObject.activeInHierarchy)
            moon.gameObject.SetActive(false);
        else if (moon.intensity > 0 && !moon.gameObject.activeInHierarchy){
            moon.gameObject.SetActive(true);
        }

        // Lighting and Reflections intensity
        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionsIntensityMultiplier.Evaluate(time);

        // Star opacity
        //if (!titleMode)
        stars.gameObject.GetComponent<Renderer>().material.color = new Color(1,1,1,starIntensity.Evaluate(time));

        // Cricket volume
        cricketSound.volume = cricketVolume.Evaluate(time);
    }

    IEnumerator executeAfterTime(Action func, float time)
    {
        yield return new WaitForSeconds(time);
        func();
    }
    void spawnSeagulls(){
        var newGulls = Instantiate(gulls, new Vector3(-180, 45, UnityEngine.Random.Range(-40,40)), Quaternion.identity);
        //newGulls.transform.LookAt(new Vector3(0,45,0));
        //newGulls.transform.Rotate(Vector3.up*-90f, Space.Self);
        newGulls.transform.RotateAround(Vector3.zero, Vector3.up, UnityEngine.Random.Range(0, 360));

    }
    void generateGullSpawnTimes(){
        // gullSpawnTimes = new float[1];
        // gullSpawnTimes[0] = 0.2f;
        // return;
        if (UnityEngine.Random.value > 0.7f){
            gullSpawnTimes = new float[1];
            gullSpawnTimes[0] = UnityEngine.Random.value * 0.3f;
        } else {
            gullSpawnTimes = new float[2];
            gullSpawnTimes[0] = UnityEngine.Random.value * 0.1f;
            gullSpawnTimes[1] = 0.15f + UnityEngine.Random.value * 0.15f;
        }
    }

}
