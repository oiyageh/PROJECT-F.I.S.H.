using UnityEngine;

//This forces any GameObject using this script to also have a Light component
//if it doesnt exist then unity will add one automartically
[RequireComponent(typeof(Light))]
public class SmoothFlicker : MonoBehaviour
{
    //A variable to store a reference to the Light component on this object
    private Light lightSource;

    public float baseIntensity = 1.5f;
    public float flickerAmount = 0.5f;
    public float flickerSpeed = 5f;

    void Start()
    {
        lightSource = GetComponent<Light>();
    }

    void Update()
    {
        //generates smooth random values between 0 and 1
        //Multiplying by flickerSpeed makes the noise change faster
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);
        lightSource.intensity = baseIntensity + (noise - 0.5f) * flickerAmount;
    }
}