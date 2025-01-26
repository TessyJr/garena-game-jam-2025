using UnityEngine;

public class FloatUpDown : MonoBehaviour
{
    public float floatSpeed = 2f; // Speed of the float motion
    public float floatAmplitude = 0.5f; // Amplitude of the float motion

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
