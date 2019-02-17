using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;

    float movementFactor;
    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // todo protect against period = 0 (=> NaN)
        float cycles = Time.time / period; // grows continually from 0

        const float tau = Mathf.PI * 2; // about 6.28
        float rawSinWave = Mathf.Sin(tau * cycles); // goes from -1 to +1


        movementFactor = rawSinWave / 2f + 0.5f; // devide -1,+1 by 2 (= -0.5,+0.5), add +0.5 (= 0,+1)
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
        
    }
}
