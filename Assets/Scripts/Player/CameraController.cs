using UnityEngine;

namespace Player
{
    public class CameraController : MonoBehaviour
    {
        private float xRotation = 0f;
        public float mouseSensitivity = 100f;
        public Transform cameraView;

        void Start()
        {
        
        }

        void Update()
        {
            var mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            var mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            
            cameraView.Rotate(Vector3.up * mouseX);

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }
}
