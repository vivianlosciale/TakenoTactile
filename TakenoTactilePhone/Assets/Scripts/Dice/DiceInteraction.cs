using UnityEngine;

public class DiceInteraction : MonoBehaviour
{

    public AudioSource soundManager;
    public AudioClip rollingDice;
    
    private float range = 5.0f;
    private float force = 17.5f;
    float cameraZDistance;

    float accelerometerUpdateInterval = 1.0f / 60.0f;
    // The greater the value of LowPassKernelWidthInSeconds, the slower the
    // filtered value will converge towards current input sample (and vice versa).
    float lowPassKernelWidthInSeconds = 1.0f;
    float shakeDetectionThreshold = 2.0f;

    float lowPassFilterFactor;
    Vector3 lowPassValue;

    public bool shookOnce = false;
    public bool stopShaking = false;
    
    private void Start()
    {
        stopShaking = false;
        shookOnce = false;
        cameraZDistance = Camera.main.WorldToScreenPoint(transform.position).z;
        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
        shakeDetectionThreshold *= shakeDetectionThreshold;
        lowPassValue = Input.acceleration;
    }

    /// <summary>
    /// When the phone is shook, rolls the dice
    /// </summary>
    private void OnShake()
    {
        soundManager.PlayOneShot(rollingDice);
        GetComponent<Rigidbody>().maxAngularVelocity = 15.0f;
        float x = transform.position.x;
        float y = transform.position.y;
        
        Vector3 acceleration = Input.acceleration;
        if (acceleration.x < 0)
        {
            x *= -1;
        }
        if (acceleration.y < 0)
        {
            y *= -1;
        }
        //Follow phone orientation ?
        Vector3 ScreenPosition = new Vector3(x * Time.deltaTime, y * Time.deltaTime, cameraZDistance);
        transform.position = Camera.main.ScreenToWorldPoint(ScreenPosition);
        //add rotation
        GetComponent<Rigidbody>().angularVelocity += new Vector3(Random.Range(-range, range), Random.Range(-range, range), Random.Range(-range, range));
        //add velocity from shake
        GetComponent<Rigidbody>().velocity=new Vector3(force, 0.0f, force);

    }

    void Update()
    {
        Vector3 acceleration = Input.acceleration;
        lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
        Vector3 deltaAcceleration = acceleration - lowPassValue;

        if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold && !stopShaking)
        {
            Debug.Log("Shake event detected at time "+Time.time);
            OnShake();
            shookOnce = true;
        }
        if(shookOnce && deltaAcceleration.sqrMagnitude < shakeDetectionThreshold)
        {
            GetComponent<Rigidbody>().useGravity = true;
            stopShaking = true;
        }
    }
}
