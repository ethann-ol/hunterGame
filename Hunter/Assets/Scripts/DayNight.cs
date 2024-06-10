using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    private Light sun;
    [SerializeField]
    private float speed = 1f;

    void Start()
    {
        sun = GetComponent<Light>();
    }

    void Update()
    {
        sun.transform.Rotate(Vector3.right * speed * Time.deltaTime);
    }
}
