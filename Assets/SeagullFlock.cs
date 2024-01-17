using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SeagullFlock : MonoBehaviour
{
    public float speed = 10;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime, Space.Self);
        if (transform.position.magnitude > 300f)
            Destroy(this.gameObject);
    }
}
