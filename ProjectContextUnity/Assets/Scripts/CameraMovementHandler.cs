using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovementHandler : MonoBehaviour {

    [SerializeField]
    private float dragSpeed = 0.7f;
    [SerializeField]
    private float zoomSpeed = 1;
    [SerializeField]
    private float dragSpeedMobile = 1;
    [SerializeField]
    private float zoomSpeedMobile;
    [SerializeField]
    private float minZoom;
    [SerializeField]
    private float maxZoom;
    [SerializeField]
    private Boundary boundaryX;
    [SerializeField]
    private Boundary boundaryY;

    private Vector3 dragOrigin;

    void Update() {
        if (GameVersion.Instance != null && GameVersion.Instance.Version == Version.Teacher)
            return;

#if UNITY_EDITOR
        PCInput();
#endif

#if UNITY_STANDALONE_WIN
        PCInput();
#endif

#if UNITY_ANDROID
        AndroidInput();
#endif
    }

    private void PCInput() {
        Camera.main.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);

        if (Input.GetMouseButtonDown(0))
            dragOrigin = Input.mousePosition;
        if (!Input.GetMouseButton(0))
            return;

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(pos.x * dragSpeed, pos.y * dragSpeed, 0);

        transform.Translate(-move, Space.World);

        pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, boundaryX.MIN, boundaryX.MAX);
        pos.y = Mathf.Clamp(transform.position.y, boundaryY.MIN, boundaryY.MAX);
        transform.position = pos;
    }

    private void AndroidInput() {
        // zoom:
        if (Input.touchCount == 2) {
            print("zoom");
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // ... change the orthographic size based on the change in distance between the touches.
            Camera.main.orthographicSize += deltaMagnitudeDiff * zoomSpeed;

            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);
            return;
        }

        // drag:
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved) {
            print("drag");
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            transform.Translate(-touchDeltaPosition.x * dragSpeedMobile * Time.deltaTime, -touchDeltaPosition.y * dragSpeedMobile * Time.deltaTime, 0);

            Vector3 pos = transform.position;
            pos.x = Mathf.Clamp(transform.position.x, boundaryX.MIN, boundaryX.MAX);
            pos.y = Mathf.Clamp(transform.position.y, boundaryY.MIN, boundaryY.MAX);
            transform.position = pos;
        }
    }
}

[System.Serializable]
public struct Boundary {
    public float MIN;
    public float MAX;
}