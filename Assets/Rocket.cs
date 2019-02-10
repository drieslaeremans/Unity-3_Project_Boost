using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 200f;
    [SerializeField] float mainThrust = 800f;

    private new Rigidbody rigidbody;
    AudioSource boostAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        boostAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        ThrustInput();
        HandleRotationInput();

    }

    void OnCollisionEnter(Collision collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                print("Friendly");
                break;
            case "Fuel":
                // do nothing
                print("Fuel");
                break;
            default:
                print("Dead");
                break;
        }
    }

    private void ThrustInput()
    {
        // Boost Forward if space, Z or Arrow Up are pressed
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Z))
        {
            float thrustThisFrame = mainThrust * Time.deltaTime;
            rigidbody.AddRelativeForce(Vector3.up * thrustThisFrame);

            if (!boostAudioSource.isPlaying)
            {
                boostAudioSource.Play();
            }
        }
        else
            boostAudioSource.Stop();
    }

    private void HandleRotationInput()
    {
        // Take manual control of rotation
        rigidbody.freezeRotation = true;

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        /* 
                 * Rotate around Z-axis (forward) if specified button is pressed
                 * Rotation is set at 30 degrees per second
                 * Vector3.forward * Time.deltaTime = 1 degree per second
                 * Time.deltaTime => amount of seconds the previous frame took to render, 
                 * using this makes sure speed op rotation stays equal on all framerates
                 */
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))
        {
            //transform.Rotate((Vector3.forward * Time.deltaTime) * 30);
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            //transform.Rotate((-Vector3.forward * Time.deltaTime) * 30);
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        // resume fysics control of rotation
        rigidbody.freezeRotation = false;
    }

    
}
