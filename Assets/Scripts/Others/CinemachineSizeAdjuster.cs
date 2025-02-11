using Cinemachine;
using UnityEngine;

public class CinemachineSizeAdjuster : MonoBehaviour
{
    public PolygonCollider2D cameraBounds; // Assign this in the Inspector
    public CinemachineVirtualCamera virtualCamera;

    void Start()
    {
        AdjustCameraSize();
    }

    void AdjustCameraSize()
    {
        if (cameraBounds == null || virtualCamera == null) return;

        float screenAspect = (float)Screen.width / Screen.height;
        float maxWidth = cameraBounds.bounds.size.x;

        // Get the Camera component inside the Cinemachine Virtual Camera
        Camera cam = Camera.main;

        if (cam != null)
        {
            float newOrthoSize = maxWidth / (2f * screenAspect);
            virtualCamera.m_Lens.OrthographicSize = newOrthoSize;
        }
    }

    void Update()
    {
        AdjustCameraSize();
    }
}
