using UnityEngine;

    
    public class CameraTrack : MonoBehaviour
    {
        [SerializeField] private Transform _camera;

        private void Update()
        {
            transform.position = _camera.position;
        }
    }
