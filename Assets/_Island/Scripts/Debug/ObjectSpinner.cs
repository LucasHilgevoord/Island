using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpinner : MonoBehaviour
{
    [SerializeField] private Transform objectToSpin;
    [SerializeField] private Vector3 dir = new Vector3(0, 1, 0);
    [SerializeField] private float speed = 10;

    private void Start()
    {
        objectToSpin = objectToSpin == null ? transform : objectToSpin;
    }

    // Update is called once per frame
    void Update()
    {
        objectToSpin.Rotate(dir * speed * Time.deltaTime);    }
}
