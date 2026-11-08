using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
[Header("Target")]
[SerializeField] private Transform target;

[Header("Follow Settings")]
[SerializeField] private float smoothSpeed = 8f;

[Header("Camera Bounds")]
[SerializeField] private bool useBounds = true;

[SerializeField]
private Vector2 minBounds = new(-30f, -30f);

[SerializeField]
private Vector2 maxBounds = new(30f, 30f);

private Camera cam;

private void Awake()
{
cam = GetComponent<Camera>();
}

private void LateUpdate()
{
if (target == null)
return;

Vector3 targetPosition = new Vector3(
target.position.x,
target.position.y,
transform.position.z
);

if (useBounds)
{
float cameraHeight = cam.orthographicSize;

float cameraWidth =
cameraHeight * cam.aspect;

float minX = minBounds.x + cameraWidth;
float maxX = maxBounds.x - cameraWidth;

float minY = minBounds.y + cameraHeight;
float maxY = maxBounds.y - cameraHeight;

if (minX <= maxX)
{
targetPosition.x = Mathf.Clamp(
targetPosition.x,
minX,
maxX
);
}
else
{
targetPosition.x =
(minBounds.x + maxBounds.x) * 0.5f;
}

if (minY <= maxY)
{
targetPosition.y = Mathf.Clamp(
targetPosition.y,
minY,
maxY
);
}
else
{
targetPosition.y =
(minBounds.y + maxBounds.y) * 0.5f;
}
}

float interpolation =
1f - Mathf.Exp(
-smoothSpeed * Time.deltaTime
);

transform.position = Vector3.Lerp(
transform.position,
targetPosition,
interpolation
);
}
}