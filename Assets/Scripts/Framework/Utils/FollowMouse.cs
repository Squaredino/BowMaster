// =================================
// Namespaces.
// =================================

using System;
using UnityEngine;

// =================================
// Define namespace.
// =================================

// =================================
// Classes.
// =================================

//[ExecuteInEditMode]
[Serializable]

//[RequireComponent(typeof(TrailRenderer))]
public class FollowMouse : MonoBehaviour
{
    public float distanceFromCamera = 5.0f;
    // =================================
    // Nested classes and structures.
    // =================================

    // ...

    // =================================
    // Variables.
    // =================================

    // ...

    public float speed = 8.0f;

    // =================================
    // Functions.
    // =================================

    // ...

    private void Awake()
    {
    }

    // ...

    private void Start()
    {
    }

    // ...

    private void Update()
    {
        var mousePosition = Input.mousePosition;
        mousePosition.z = distanceFromCamera;

        var mouseScreenToWorld = Camera.main.ScreenToWorldPoint(mousePosition);

        var position = Vector3.Lerp(transform.position, mouseScreenToWorld, 1.0f - Mathf.Exp(-speed * Time.deltaTime));

        transform.position = position;
    }

    // ...

    private void LateUpdate()
    {
    }
    // =================================
    // End functions.

    // =================================
}

// =================================
// End namespace.
// =================================


// =================================
// --END-- //
// =================================