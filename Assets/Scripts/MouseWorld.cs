using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    private static LayerMask floorLayerMask = 1 << 6;


    private void Update()
    {
        transform.position = GetMousePoisiion();
    }



    public static Vector3 GetMousePoisiion()
    {
        Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, floorLayerMask);
        return raycastHit.point;
    }
}
