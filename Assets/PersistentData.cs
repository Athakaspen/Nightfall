using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentData
{

    public static AnimationCurve easyCurve = AnimationCurve.Linear(0,1,1,3);
    public static AnimationCurve normalCurve = AnimationCurve.Linear(0,1.4f,1,5f);
    public static AnimationCurve lunaticCurve = AnimationCurve.Linear(0,3f,1,6.3f);

    public static float mouseSensitivity = .6f;
    public static float volume = .6f;

    public static int endDays = -1;
    public static int endNights = -1;

    public static string difficulty = "NONE!";
    public static float dayLength = 75;
    public static int repairCost = 10;
    public static float repairTime = 2f;
    public static int maxWood = 25;
    public static AnimationCurve difficultyCurve = normalCurve;
    

}
