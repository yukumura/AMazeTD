// Just add this script to your camera. It doesn't need any configuration.

using UnityEngine;
using UnityEngine.EventSystems;

public class TouchCamera : MonoBehaviour
{
    public float zoomOutMin = 15;
    public float zoomOutMax = 30;

    private float panSpeed = 0.01f;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    private void Update()
    {
        GetFingerMovement();
    }

    public void GetFingerMovement()
    {
        if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) return;

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            if (gameManager.GetTileSelected() != null)
                gameManager.DeselectTile();

            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            transform.Translate(-touchDeltaPosition.x * panSpeed, -touchDeltaPosition.y * panSpeed, 0);

            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, -4, 4),
                15f,
                Mathf.Clamp(transform.position.z, -16f, -8f)
                );
        }
        else if(Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            Zoom(difference * 0.1f);
        }
    }

    void Zoom(float increment)
    {
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView - increment, zoomOutMin, zoomOutMax);
    }
}
