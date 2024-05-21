using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command
{
    internal Command() { }
    virtual internal void Execute() { }
} 

public class InputHandler : MonoBehaviour
{
    private const bool DEBUG = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (DEBUG)
        {
            // Check if clicked on object
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log(hit.transform);
                    if (hit.collider != null)
                    {
                        // Check if it is a destructable object
                        IDestructible destructible = hit.collider.GetComponent<IDestructible>();
                        if (destructible != null)
                        {
                            Debug.Log("Hit!");
                            destructible.TakeDamage(10f);
                        }
                    }
                }

                // Add a debug ray
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 1f);
            }
        }
    }
}
