using UnityEngine;

public class CannonController : MonoBehaviour
{
    public Transform canonRotate;
    public Transform firePoint;
    public GameObject projectilePrefab;
    public LineRenderer lineRenderer;
    public float rotationSpeed = 10f;
    public float projectileSpeed = 10f;
    public float speedChangeRate = 1f;
    public int resolution = 50;

    private void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        canonRotate.Rotate(Vector3.right * verticalInput * rotationSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.W))
        {
            projectileSpeed += speedChangeRate * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            projectileSpeed -= speedChangeRate * Time.deltaTime;
            projectileSpeed = Mathf.Max(0, projectileSpeed);
        }

        DrawTrajectory();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireCannon();
        }
    }

    void DrawTrajectory()
    {
        lineRenderer.positionCount = resolution;
        Vector3[] points = new Vector3[resolution];
        Vector3 startPosition = firePoint.position;
        Vector3 startVelocity = firePoint.forward * projectileSpeed;
        float flightTime = CalculateFlightTime(startVelocity);

        for (int i = 0; i < resolution; i++)
        {
            float t = i / (float)(resolution - 1) * flightTime;
            Vector3 displacement = startVelocity * t + 0.5f * Physics.gravity * t * t;
            points[i] = startPosition + displacement;
        }

        lineRenderer.SetPositions(points);
    }

    float CalculateFlightTime(Vector3 velocity)
    {
        float verticalSpeed = velocity.y;
        return (verticalSpeed + Mathf.Sqrt(verticalSpeed * verticalSpeed - 2 * Physics.gravity.y * firePoint.position.y)) / -Physics.gravity.y;
    }

    void FireCannon()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        Vector3 launchVelocity = firePoint.forward * projectileSpeed;
        rb.velocity = launchVelocity;
    }
}
