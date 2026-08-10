using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    [SerializeField] InputActionReference Move, Sprint, Interact;
    [SerializeField] float moveSpeed;
    [SerializeField] float sprintModifier;
    [SerializeField] float interactionRange;
    [SerializeField] LayerMask interactionLayer;


    private GameObject currentHighlightedObject;
    private Camera camera;
    CharacterController controller;
    private float targetSpeed;
    private Vector2 Dir;
    private bool sprintPressed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Dir = Move.action.ReadValue<Vector2>();

        sprintPressed = Sprint.action.ReadValue<float>() > 0.5f ? true : false;
        
        sprintCheck();
        

        Vector3 targetDir = ((this.transform.forward * Dir.y + this.transform.right * Dir.x) * targetSpeed) * Time.deltaTime;
        Vector3 moveDir = Vector3.zero;
        
        moveDir = Vector3.Lerp(moveDir, targetDir,0.7f);
        controller.Move(moveDir);


        interaction();

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


    void sprintCheck()
    {
        if (sprintPressed)
        {
            targetSpeed = moveSpeed * sprintModifier;
        }
        else
        {
            targetSpeed = moveSpeed;
        }
    }


    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawRay(this.transform.position,this.transform.forward);
    //}
}
