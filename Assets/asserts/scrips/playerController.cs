using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    [SerializeField] InputActionReference Move, Sprint, Interact, Jump;
    [SerializeField] LayerMask interactionLayer, floorLayer;
    [SerializeField] float moveSpeed;
    [SerializeField] float sprintModifier;
    [SerializeField] float interactionRange;
    [SerializeField] float cameraBobBounds, cameraBobSpeed, sprintBobModifier;
    [SerializeField] float jumpStrength;
    [SerializeField] bool isJumping;

    private CharacterController controller;
    private GameObject currentHighlightedObject;
    private Camera camera;
    private Vector2 Dir;
    private float cameraInitPosY, targetCameraBobSpeed;
    private float targetSpeed;
    private bool sprintPressed,isMoving;
    private bool jumpPressed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        camera = Camera.main;
        cameraInitPosY = camera.transform.localPosition.y;
        controller.attachedRigidbody.useGravity = true;
    }

    // Update is called once per frame
    void Update()
    {
        Dir = Move.action.ReadValue<Vector2>();

        isJumping = !Physics.Raycast(camera.transform.position, Vector3.down, 1.9f,floorLayer);

        jumpPressed = Jump.action.WasPressedThisFrame();

        isMoving = Mathf.Abs(controller.velocity.x + controller.velocity.y + controller.velocity.z) >= 0.01f ? true : false;

        jumpCheck();

        sprintCheck();

        cameraBob();

        interaction();

        Vector3 targetDir = ((this.transform.forward * Dir.y + this.transform.right * Dir.x -this.transform.up * 2.0f) * targetSpeed) * Time.deltaTime;
        Vector3 moveDir = Vector3.zero;
        
        moveDir = Vector3.Lerp(moveDir, targetDir,0.7f);
        controller.Move(moveDir);



    }



    void jumpCheck()
    {
        if (jumpPressed && !isJumping)
        {
            //controller.Move(Vector3.up * jumpStrength);
            StartCoroutine(jumpThread());
        }
        else if (isJumping)
        {
           
        }
    }

    private IEnumerator jumpThread()
    {
        Vector3 jumpAmount = Vector3.zero;
        Vector3 targetJump = Vector3.up * jumpStrength;
        float timeStart = Time.time;
        while (Time.time < timeStart + 0.2f /*|| (isJumping && Time.time < timeStart + 0.03f)*/)
        {
            jumpAmount = Vector3.Lerp(jumpAmount, targetJump, 0.7f);
            controller.Move(jumpAmount * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        
    }


    void interaction()
    {
        RaycastHit hit;
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        if (Physics.Raycast(ray,out hit, interactionRange, interactionLayer))
        {
            currentHighlightedObject = hit.transform.gameObject;
            currentHighlightedObject.GetComponent<objectScrip>().highlight();
            if (Interact.action.WasPressedThisFrame())
            {
                currentHighlightedObject.GetComponent<objectScrip>().interact();
            }
        }
        else if (currentHighlightedObject != null)
        {
            currentHighlightedObject.GetComponent<objectScrip>().endHighlight();
            currentHighlightedObject = null;
        }
    }



    void cameraBob()
    {
        float newCameraPos;
        if (isMoving && !isJumping)
        {
            newCameraPos = cameraInitPosY + Mathf.Sin(Time.time * targetCameraBobSpeed) * cameraBobBounds;
        }
        else
        {
            newCameraPos= cameraInitPosY;
        }

        camera.transform.localPosition = Vector3.Lerp(camera.transform.localPosition, new Vector3(camera.transform.localPosition.x, newCameraPos, camera.transform.localPosition.z), 0.1f);
    }


    void sprintCheck()
    {
        sprintPressed = Sprint.action.ReadValue<float>() >= 0.5f ? true : false;

        targetSpeed = sprintPressed ? moveSpeed * sprintModifier : moveSpeed;

        targetCameraBobSpeed = sprintPressed ? cameraBobSpeed * sprintBobModifier : cameraBobSpeed;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawRay(this.transform.position,this.transform.forward);
    //}
}
