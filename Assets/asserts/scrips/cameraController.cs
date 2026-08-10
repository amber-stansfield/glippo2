using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class cameraController : MonoBehaviour
{
    private Camera camera;
    [SerializeField] Transform player;
    [SerializeField] InputActionReference Look;

    [SerializeField] float sensX, sensY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camera = Camera.main;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        print(Look.action.ReadValue<Vector2>().y);
        player.transform.localEulerAngles = new Vector3(0,player.transform.localEulerAngles.y + (Look.action.ReadValue<Vector2>().x * sensX),0);
        camera.transform.localEulerAngles = new Vector3(eulerClamp(camera.transform.localEulerAngles.x - (Look.action.ReadValue<Vector2>().y * sensY),-75.0f,75.0f), 0,0);
    }



    private float eulerClamp(float value, float min, float max)
    {
        if (value > 180)
        {
            value -= 360;
        }
        else if (value < -180)
        {
            value += 360;
        }

        if (value > max)
        {
            value = max;
        }
        if (value < min)
        {
            value = min;
        }
        return value;
    }
}
