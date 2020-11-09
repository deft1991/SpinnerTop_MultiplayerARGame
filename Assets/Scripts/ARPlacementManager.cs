using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlacementManager : MonoBehaviour
{

    public Camera arCamera;
    public GameObject battleArenaGameObject;
    
    private ARRaycastManager _aRRaycastManager;
    private static List<ARRaycastHit> _raycastHits = new List<ARRaycastHit>();

    // Start and Awake work in similar ways
    // except that Awake is called first and,
    // unlike Start, will be called even if the script component is disabled.
    private void Awake()
    {
        _aRRaycastManager = GetComponent<ARRaycastManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // we will do raycasting
        // where we send ray from - also RaycastCenter_Image - image in center
        Vector3 centerOfScreen = new Vector3(Screen.width / 2, Screen.height / 2);

        // ray from the centre of the screen
        Ray ray = arCamera.ScreenPointToRay(centerOfScreen);
        
        // if ray is sent and it intersects with a detected plane
        if (_aRRaycastManager.Raycast(ray, _raycastHits, TrackableType.PlaneWithinPolygon))
        {
            // Intersection!
            // we need to get the pose of the hit , so hat we can place the battle arena
            // to the real life position
            
            // take first result is closest one
            Pose hitPose = _raycastHits[0].pose;

            Vector3 positionTobePlaced = hitPose.position;

            // place battle arena
            battleArenaGameObject.transform.position = positionTobePlaced;
        }
    }
}