using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private new Rigidbody rigidbody;
    AudioSource boostAudioSource;
    bool boostPlay;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        boostAudioSource = GetComponent<AudioSource>();
        boostPlay = false;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        HandleBoostInput();
        HandleRotationInput();

    }

    private void HandleRotationInput()
    {
        /* 
                 * Rotate around Z-axis (forward) if specified button is pressed
                 * Rotation is set at 30 degrees per second
                 * Vector3.forward * Time.deltaTime = 1 degree per second
                 * Time.deltaTime => amount of seconds the previous frame took to render, 
                 * using this makes sure speed op rotation stays equal on all framerates
                 */
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate((Vector3.forward * Time.deltaTime) * 30);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate((-Vector3.forward * Time.deltaTime) * 30);
        }
    }

    private void HandleBoostInput()
    {
        // Boost Forward if space, Z or Arrow Up are pressed
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Z))
        {
            rigidbody.AddRelativeForce(Vector3.up);

            if (!boostAudioSource.isPlaying)
            {
                boostAudioSource.Play();
            }
        }
        else
            boostAudioSource.Stop();
    }
}
