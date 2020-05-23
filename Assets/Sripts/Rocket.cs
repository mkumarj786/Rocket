
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField]float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip finishLevel;
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] ParticleSystem mainEngine_particle;
    [SerializeField] ParticleSystem death_particle;
    [SerializeField] ParticleSystem finishLevel_particle;

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

            RespondToThrust();
            RespondToRotate(); 
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
                Finish_Sequence();
                break;

            default:
                Death_Srquence();
                break;

        }
    }

    private void Death_Srquence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        death_particle.Play();
        Invoke("LoadFirst", levelLoadDelay);
    }

    private void Finish_Sequence()
    {
        state = State.Transceding;
        audioSource.Stop();
        audioSource.PlayOneShot(finishLevel);
        finishLevel_particle.Play();
        Invoke("LoadNext", levelLoadDelay);
    }

    private void LoadNext()
    {
        SceneManager.LoadScene(1);
    }
    private void LoadFirst()
    {
        SceneManager.LoadScene(0);
    }

    private void RespondToThrust()
    {

        if (Input.GetKey(KeyCode.Space))
        {
           
                ApplyThrust();
        }
        else
        {
           
                audioSource.Stop();
            mainEngine_particle.Stop();
        }


    }

    private void ApplyThrust()
    {
        rigidbody.AddRelativeForce(Vector3.up * mainThrust );
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(mainEngine);
        mainEngine_particle.Play();
    }

    private void RespondToRotate()
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
