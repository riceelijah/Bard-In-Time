using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smooth = 5f;
    public float catchUp = 1f; // extra smooth per unit of distance, so the further behind it is the harder it chases
    public Vector2 maxOffset = new Vector2(6f, 3.5f); // hard leash: the target never gets further than this from center

    void LateUpdate()
    {
        Vector2 cam = transform.position;
        Vector2 goal = target.position;

        float t = (smooth + catchUp * Vector2.Distance(cam, goal)) * Time.deltaTime;
        cam = Vector2.Lerp(cam, goal, t);

        // a fast enough fall can still outrun any lerp, so clamp as the guarantee
        cam.x = Mathf.Clamp(cam.x, goal.x - maxOffset.x, goal.x + maxOffset.x);
        cam.y = Mathf.Clamp(cam.y, goal.y - maxOffset.y, goal.y + maxOffset.y);

        transform.position = new Vector3(cam.x, cam.y, transform.position.z);
    }
}
