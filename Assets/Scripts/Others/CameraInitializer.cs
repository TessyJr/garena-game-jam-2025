using UnityEngine;
using Cinemachine;

public class CameraInitializer : MonoBehaviour
{
    [SerializeField] private float desiredOrthoSize = 5f;
    [SerializeField] private Vector3 desiredOffset = new Vector3(0, 1, 0);

    private CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        if (virtualCamera != null)
        {
            // Set the orthographic size if using an orthographic camera.
            if (virtualCamera.m_Lens.Orthographic)
            {
                virtualCamera.m_Lens.OrthographicSize = desiredOrthoSize;
            }

            // Access the Composer component and set its offset.
            CinemachineCameraOffset offset = GetComponent<CinemachineCameraOffset>();
            if (offset != null)
            {
                offset.m_Offset = desiredOffset;
            }
        }
    }
}
