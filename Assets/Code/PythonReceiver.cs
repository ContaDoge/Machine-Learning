using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class PythonReceiver : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;
    private byte[] buffer = new byte[1024];
    public string currentClass = ""; // To store the class received from Python
    public float currentConfidence = 0.0f; // To store the confidence received from Python

    void Start()
    {
        try
        {
            // Connect to the Python server
            client = new TcpClient("127.0.0.1", 65432); // Match HOST and PORT from Python
            stream = client.GetStream();
            Debug.Log("Connected to Python server");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error connecting to Python server: {e.Message}");
        }
    }

    void Update()
    {
        if (stream != null && stream.DataAvailable)
        {
            // Read data from the Python server
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            // Parse the message
            string[] parts = message.Split(',');
            if (parts.Length == 2)
            {
                currentClass = parts[0].Trim(); // Store the class name
                currentConfidence = float.Parse(parts[1]); // Store the confidence value
                Debug.Log($"Received Class: {currentClass}, Confidence: {currentConfidence}");
            }
            Debug.Log($"Raw message received from Python: {message}");
            Debug.Log($"Parsed Class: {currentClass}, Confidence: {currentConfidence}");
        }
    }

    void OnApplicationQuit()
    {
        // Clean up the connection on exit
        if (stream != null) stream.Close();
        if (client != null) client.Close();
    }
}
