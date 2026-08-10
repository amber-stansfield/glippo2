using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class objectScrip : MonoBehaviour
{
    UnityEngine.Color objectColour;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objectColour = this.GetComponentInChildren<MeshRenderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void interact()
    {
        print("boog");
    }


    public void highlight()
    {
        foreach (MeshRenderer goob in this.GetComponentsInChildren<MeshRenderer>())
        {
            goob.material.color = UnityEngine.Color.white;
        }
    }
    public void endHighlight()
    {
        foreach (MeshRenderer goob in this.GetComponentsInChildren<MeshRenderer>())
        {
            goob.material.color = objectColour;
        }
    }

}
