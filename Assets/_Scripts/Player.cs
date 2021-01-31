using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Camera cam = null;
    private Rigidbody rb = null;

    private Vector3 velocity = Vector3.zero;
    private Quaternion targetRotation = Quaternion.identity;

    private List<Places> visits = new List<Places>();

    [SerializeField] private float forwardAcceleration = 1f;
    [SerializeField] private float forwardMaxSpeed = 10f;
    [SerializeField] private float backwardMaxSpeed = 2f;
    [SerializeField] private float rotationSpeed = 32f;
    [SerializeField] private float wingResistance = 1f;
    [SerializeField] private float mouseSensitivity = 1f;

    [Header("References: ")]
    [SerializeField] private TextMesh objectiveText = null;
    [SerializeField] private TextMesh visitsText = null;
    [SerializeField] private GameObject exit = null;

    public Quaternion TargetRotation { get => targetRotation; }

    public void Visit(Places place)
    {
        visits.Add(place);

        visitsText.text = "---Visits---\n";

        if (visits.Contains(Places.opposite))
            visitsText.text += "<color=green>Opposite</color>\n";
        else
            visitsText.text += "Opposite\n";
        if (visits.Contains(Places.center))
            visitsText.text += "<color=green>Center</color>\n";
        else
            visitsText.text += "Center\n";
        if (visits.Contains(Places.west))
            visitsText.text += "<color=green>West</color>\n";
        else
            visitsText.text += "West\n";
        if (visits.Contains(Places.east))
            visitsText.text += "<color=green>East</color>";
        else
            visitsText.text += "East";

        if (visits.Count == System.Enum.GetNames(typeof(Places)).Length - 1)
        {
            objectiveText.text = "---Objective---\nReturn to\nyour starting\npoint and\nleave the cave.";
            exit.SetActive(true);
        }
    }

    private void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;

        targetRotation = transform.rotation;
    }

    private void Update()
    {
        float rotation = rotationSpeed * Time.deltaTime;

        // Thrusters
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            rb.velocity += transform.forward * forwardAcceleration * Input.GetAxis("Vertical") *
                Time.deltaTime;
        }

        // Roll
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            targetRotation = targetRotation *
                Quaternion.AngleAxis(-rotation * Input.GetAxisRaw("Horizontal"), Vector3.forward);
        }

        // Free look
        if (Input.GetMouseButton(1))
        {
            if (Input.GetAxis("Mouse X") != 0)
            {
                cam.transform.RotateAround(cam.transform.position, transform.up,
                    Input.GetAxis("Mouse X") * mouseSensitivity);
            }

            if (Input.GetAxis("Mouse Y") != 0)
            {
                cam.transform.RotateAround(cam.transform.position, cam.transform.right,
                    -Input.GetAxis("Mouse Y") * mouseSensitivity);
            }
        }
        // Turning intent
        else
        {
            if (Input.GetAxis("Mouse X") != 0)
            {
                targetRotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse X") * mouseSensitivity, Vector3.up);
            }

            if (Input.GetAxis("Mouse Y") != 0)
            {
                targetRotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * mouseSensitivity, -Vector3.right);
            }
        }

        // Reset after free look
        if (Input.GetMouseButtonUp(1))
        {
            cam.transform.localRotation = Quaternion.identity;
        }

        // Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void FixedUpdate()
    {
        float rotation = rotationSpeed * Time.fixedDeltaTime;

        // Turning
        Quaternion delta = Quaternion.Lerp(
            transform.localRotation,
            targetRotation,
            rotation / rotationSpeed);
        transform.localRotation = delta;

        // "Gliding" effect
        Vector3 velocityLocal = transform.InverseTransformDirection(rb.velocity);
        velocityLocal.x = Mathf.Lerp(velocityLocal.x, 0, Time.fixedDeltaTime * wingResistance);
        velocityLocal.y = Mathf.Lerp(velocityLocal.y, 0, Time.fixedDeltaTime * wingResistance);
        velocityLocal.z = Mathf.Clamp(velocityLocal.z, -backwardMaxSpeed, forwardMaxSpeed);
        rb.velocity = transform.TransformDirection(velocityLocal);

        // Movement
        transform.position = Vector3.MoveTowards(transform.position, transform.position + velocity, velocity.magnitude * Time.fixedDeltaTime);
    }
}
