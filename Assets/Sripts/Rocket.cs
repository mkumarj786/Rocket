using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField]float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    private new Rigidbody rigidbody;
    AudioSource audioSource;

    enum State { Alive,Dying,Transceding}
    State state = State.Alive;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        
    }

    // Update is called once per frame
    void Update()
    {

        if (state==State.Alive)
        {

            Thrust();
            Rotate();
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive)
        {
            return;
        }


        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //do nothing
                print("OK");
                break;

            case "Finish":
                print("Hit Finish");
                state = State.Transceding;
                Invoke("LoadNext",1f);
                break;

            default:
                state = State.Dying;
                print("Dead");
                Invoke("LoadFirst", 1);
                break;

        }
    }

    private void LoadNext()
    {
        SceneManager.LoadScene(1);
    }
    private void LoadFirst()
    {
        SceneManager.LoadScene(0);
    }

    private void Thrust()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            rigidbody.AddRelativeForce(Vector3.up*mainThrust);
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }


    }

    private void Rotate()
    {
        float rotationFrame = rcsThrust * Time.deltaTime;

        rigidbody.freezeRotation = true; 

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward*rotationFrame);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward*rotationFrame);
        }

        rigidbody.freezeRotation = false;

    }


}
