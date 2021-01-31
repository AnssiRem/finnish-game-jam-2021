using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Places
{
    NONE,
    center,
    opposite,
    east,
    west
}

public class Guffin : MonoBehaviour
{
    private Material mat = null;
    private Light glow = null;

    private Vector3 startPos = Vector3.zero;

    [SerializeField] private Places place = 0;
    [SerializeField] private float rotateSpeed = 1f;
    [SerializeField] private float hoverSpeed = 1f;
    [SerializeField] private float hoverAmount = 1f;
    [SerializeField] private Color color = Color.white;

    private void Start()
    {
        mat = GetComponentInChildren<MeshRenderer>().materials[0];
        glow = GetComponentInChildren<Light>();

        mat.SetColor("_Color", color);
        glow.color = color;

        startPos = transform.position;
    }

    private void Update()
    {
        transform.position = startPos +
            Vector3.up * Mathf.Sin(Time.time * hoverSpeed) * hoverAmount;
        transform.rotation = Quaternion.AngleAxis(rotateSpeed * Time.deltaTime, Vector2.up) *
            transform.rotation;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Player>().Visit(this.place);
            Destroy(gameObject);
        }
    }
}
