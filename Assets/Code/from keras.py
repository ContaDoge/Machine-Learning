import socket
from tensorflow.keras.models import load_model
import cv2
import numpy as np
import os

# Set up the socket
HOST = '127.0.0.1'  # Localhost
PORT = 65432        # Port to listen on
server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server.bind((HOST, PORT))
server.listen(1)  # Allow 1 connection

print(f"Waiting for Unity to connect on {HOST}:{PORT}...")
conn, addr = server.accept()
print(f"Connected by {addr}")

# Load the model and labels
model_path = os.path.join(os.path.dirname(__file__), "keras_Model.h5")
labels_path = os.path.join(os.path.dirname(__file__), "labels.txt")
model = load_model(model_path, compile=False)
class_names = open(labels_path, "r").readlines()

# Initialize the webcam
camera = cv2.VideoCapture(0)

if not camera.isOpened():
    print("Error: Could not open webcam.")
    exit()

while True:
    ret, image = camera.read()
    if not ret:
        print("Error: Failed to capture image.")
        break

    # Resize and preprocess the image
    resized_image = cv2.resize(image, (224, 224), interpolation=cv2.INTER_AREA)
    input_image = np.asarray(resized_image, dtype=np.float32).reshape(1, 224, 224, 3)
    input_image = (input_image / 127.5) - 1  # Normalize to [-1, 1]

    # Make prediction
    prediction = model.predict(input_image)
    index = np.argmax(prediction)
    class_name = class_names[index].strip()
    confidence_score = prediction[0][index]

    # Send result to Unity
    message = f"{class_name},{confidence_score:.2f}"
    conn.sendall(message.encode('utf-8'))  # Send the message to Unity  
    print(f"Sending to Unity: Class {class_name}, Confidence: {confidence_score:.2f}")

    # Exit on Esc key press
    if cv2.waitKey(1) == 27:
        break

camera.release()
cv2.destroyAllWindows()
conn.close()