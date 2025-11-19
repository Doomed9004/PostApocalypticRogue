using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirection : MonoBehaviour
{
    private Reader reader;
	Vector3 curDir;
    Camera mainCamera;
    [SerializeField]LayerMask layerMask;
    public Vector3 MouseAimDir
    {
        get
        {
            Ray ray = mainCamera.ScreenPointToRay(reader.InputMousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000,layerMask))
            {
                Vector3 dir = hit.point - transform.position;
                return Vector3.ProjectOnPlane(dir, Vector3.up).normalized;
            }
            else
            {
                return Vector3.zero;
            }
        }
    } 
    
    private void Start()
    {
        reader = FindObjectOfType<Reader>();
        mainCamera = FindObjectOfType<Camera>();
    }

    private bool hasDirInput = false;
    private void Update()
    {
        hasDirInput = false;
		if(reader.InputDirV3!=Vector3.zero && !reader.Aim)
        {
            hasDirInput = true;
            curDir = reader.InputDirV3;
        }
        if (reader.Aim)
        {
            //Debug.Log(MouseAimDir);
            transform.rotation = Quaternion.LookRotation(MouseAimDir);
            curDir = MouseAimDir;
        }
        else
        {
            if (hasDirInput)
            {
                transform.rotation = Quaternion.LookRotation(reader.InputDirV3);
            }
            else if(curDir != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(curDir);
            }
            
        }
    }
}
