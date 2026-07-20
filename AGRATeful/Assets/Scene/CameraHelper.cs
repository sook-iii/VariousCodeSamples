using UnityEngine;

[ExecuteAlways]
public class CameraHelper : MonoBehaviour
{

    public float moveSpeed;
    public float orbitDistance;
    public float orbitHeight;
    public float lookingAngle;

    void Update()
    {
        Vector3 heightlessPosition = Vector3.Normalize(new Vector3(transform.position.x, 0, transform.position.z) + (transform.right * Time.deltaTime * moveSpeed)) * orbitDistance;
        transform.position = new Vector3(heightlessPosition.x, orbitHeight, heightlessPosition.z);
        transform.forward = -Vector3.Normalize(transform.position);
        transform.localEulerAngles = new Vector3(lookingAngle, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }
}
