using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    [SerializeField]LayerMask layerMask;
    
    FadeObstacle _curObstacle;

    private void LateUpdate()
    {
        Vector3 dir = transform.forward;

        if (Physics.Raycast(transform.position, dir, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            FadeObstacle obstacle = hit.collider.GetComponent<FadeObstacle>();
            if (obstacle != null && obstacle != _curObstacle)
            {
                if (_curObstacle != null)
                {
                    Debug.Log("控制FadeIn");
                    _curObstacle.FadeIn();
                }

                Debug.Log("控制FadeOut");
                obstacle.FadeOut();
                _curObstacle = obstacle;
            }
        }
        else
        {
            if (_curObstacle != null)
            {
                _curObstacle.FadeIn();
                _curObstacle = null;
            }
        }
    }
}
