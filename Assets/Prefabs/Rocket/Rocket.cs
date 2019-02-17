using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 200f;
    [SerializeField] float mainThrust = 800f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip successSound;
    [SerializeField] AudioClip deathSound;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;

    private new Rigidbody rigidbody;
    AudioSource audioSource;

    enum State {  Alive, Dying, Transcending };
    State state = State.Alive;

    private float transcendingTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if(!state.Equals(State.Dying))
        {
            HandleThrustInput();
            HandleRotationInput();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) return;

        switch(collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(successSound);
        successParticles.Play();
        Invoke("LoadNextLevel", transcendingTime);
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        deathParticles.Play();
        Invoke("LoadFirstLevel", transcendingTime);
    }

    private void LoadNextLevel()
    {
        successParticles.Stop();
        SceneManager.LoadScene(1);
        // todo allow for more than 2 levels
    }

    private void LoadFirstLevel()
    {
        deathParticles.Stop();
        SceneManager.LoadScene(0);
    }

    private void HandleThrustInput()
    {
        // Boost Forward if space, Z or Arrow Up are pressed
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Z))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidbody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }

        mainEngineParticles.Play();
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
