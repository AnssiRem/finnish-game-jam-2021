using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is the same as PointNorth. Don't tell anyone U+1F92B U+1F31A.
public class PointUp : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }
}
