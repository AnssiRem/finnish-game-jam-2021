using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointTarget : MonoBehaviour
{
    Player player = null;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void LateUpdate()
    {
        transform.rotation = player.TargetRotation;
    }
}
