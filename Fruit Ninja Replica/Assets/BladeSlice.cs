using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeSlice : MonoBehaviour {

    bool isCutting = false;
    Rigidbody2D rb;
    Camera cam;
    public GameObject bladeTrailPrefab;
    GameObject currentBladeTrail;
    CircleCollider2D c2d;
    Vector2 prevPosition;
    public float minCuttingVelocity = 0.001f;
    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        c2d = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCutting();
        }

        else if (Input.GetMouseButtonUp(0))
        {
            StopCutting();
        }

        if (isCutting)
        {
            UpdateCut();
        }
    }

    void StartCutting()
    {
        isCutting = true;
        currentBladeTrail = Instantiate(bladeTrailPrefab, transform);
        prevPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        c2d.enabled = false;
    }
    void StopCutting()
    {
        isCutting = false;
        currentBladeTrail.transform.SetParent(null);
        Destroy(currentBladeTrail, 2f);
        c2d.enabled = false;
    }

    void UpdateCut()
    {        
       
        Vector2 newPosition=cam.ScreenToWorldPoint(Input.mousePosition);
        rb.position = newPosition;

        float velocity = (newPosition - prevPosition).magnitude * Time.deltaTime;
        if (velocity>minCuttingVelocity)
        {
            c2d.enabled = true;
        }

        else
        {
            c2d.enabled = false;
        }

        prevPosition = newPosition;

    }
}
