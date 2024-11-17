using UnityEngine;

public class Move : MonoBehaviour
{
    private PythonReceiver pythonReceiver; // Reference to the PythonReceiver script
    private Rigidbody rb; // Rigidbody for controlling movement

    public float speed = 5f; // Speed of the cube

    void Start()
    {
        // Find the PythonReceiver script
        pythonReceiver = FindObjectOfType<PythonReceiver>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {   
    if (pythonReceiver != null)
        {
        // Get the current class and confidence
        string detectedClass = pythonReceiver.currentClass;
        float confidence = pythonReceiver.currentConfidence;

        if (detectedClass == "Class 2" && confidence > 0.9f)
            {
            rb.velocity = Vector3.forward * speed;
            Debug.Log("Moving forward: Class 2 detected with high confidence");
            }
        else if (detectedClass == "Class 1" && confidence > 0.9f)
            {
            rb.velocity = Vector3.zero;
            Debug.Log("Stopping: Class 1 detected with high confidence");
            }
        }
    }
}
