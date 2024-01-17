using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleRotate : MonoBehaviour
{

    public float rotSpeed = 30;
    public float rotStart = 0;
    // Start is called before the first frame update
    void Start()
    {
        transform.RotateAround(Vector3.zero, Vector3.up, rotStart);
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(Vector3.zero, Vector3.up, rotSpeed*Time.deltaTime);
    }
}
