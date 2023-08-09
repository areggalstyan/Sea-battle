using System;
using UnityEngine;

public class PlayerRaycastInput : MonoBehaviour
{
    private const float RaycastMaxDistance = 100;
    private Camera _camera;

    protected virtual void Awake()
    {
        _camera = Camera.main;
    }

    protected void Raycast(Action<Vector3> action)
    {
        if (!Input.GetMouseButton(0)) return;
        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out var hit, RaycastMaxDistance)) return;
        action.Invoke(hit.transform.position);
    }
}