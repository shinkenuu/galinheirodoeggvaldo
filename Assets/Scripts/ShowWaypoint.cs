using UnityEngine;

public class ShowWaypoint : MonoBehaviour
{

    public void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "waypoint.png");
    }
}
