using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class lookAtPlayer : MonoBehaviour
{
    [SerializeField] bool xLock, yLock, zLock;
    private Transform player;
    private Coroutine goob;
    private float startX, startY, startZ;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startX = this.transform.localRotation.x;
        startY = this.transform.localRotation.y;
        startZ = this.transform.localRotation.z;

        player = Camera.main.transform;
        Invoke(nameof(StartLookAtPlayer), 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private IEnumerator LookAtPlayerCoroutine()
    {

        this.transform.LookAt(player);
        float xVal = this.transform.localRotation.x;
        float yVal = this.transform.localRotation.y;
        float zVal = this.transform.localRotation.z;

        if (xLock)
        {
            xVal = startX;
        }
        if (yLock)
        {
            yVal = startY;
        }
        if (zLock)
        {
            zVal = startZ;
        }

        this.transform.localRotation = new Quaternion(xVal, yVal, zVal, this.transform.localRotation.w);

        

        yield return new WaitForSeconds(0.1f);

        
        StartCoroutine(LookAtPlayerCoroutine());
    }

    void StartLookAtPlayer()
    {
        StartCoroutine(LookAtPlayerCoroutine());
    }
}
