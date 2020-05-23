using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{

    [SerializeField] Vector3 movementVector = new Vector3(10f,10f,10f);
    [SerializeField] float periods = 2f;

    [Range(0, 1)] [SerializeField] float movementFactor;
   Vector3 statingPoint;

    // Start is called before the first frame update
    void Start()
    {
        statingPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(periods <= Mathf.Epsilon) { return; }
        float cycles = Time.time / periods; //grows continuslly from 0
        const float tau = Mathf.PI * 2;//about 6.28
        float rawSinWave = Mathf.Sin(cycles * tau);
        movementFactor = rawSinWave / 2f + 0.5f;



        Vector3 offset = movementFactor * movementVector;
        transform.position = statingPoint + offset;
        
    }
}
