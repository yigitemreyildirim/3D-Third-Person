using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public Transform player;
    public float viewDistance = 8f;
    public float crouchViewDistance = 4f;
    public float viewAngle = 60f;
    public LayerMask obstacleMask;

    public Material normalMaterial;
    public Material alertMaterial;

    private Renderer rend;
    private ThirdPersonMovement playerMovement;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (player != null)
            playerMovement = player.GetComponent<ThirdPersonMovement>();
    }

    void Update()
    {
        if (player == null || playerMovement == null) return;

        float currentViewDistance = playerMovement.isCrouching ? crouchViewDistance : viewDistance;

        Vector3 toPlayer = (player.position - transform.position).normalized;
        float dist = Vector3.Distance(transform.position, player.position);

        if (Vector3.Angle(transform.forward, toPlayer) < viewAngle / 2f)
        {
            bool blocked = Physics.Raycast(transform.position + Vector3.up * 0.5f, toPlayer, dist, obstacleMask);
            if (!blocked && dist <= currentViewDistance)
            {
                rend.material = alertMaterial; // görüyor
                return;
            }
        }

        rend.material = normalMaterial; // görmüyor
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector3 L = Quaternion.Euler(0, -viewAngle / 2f, 0) * transform.forward;
        Vector3 R = Quaternion.Euler(0, viewAngle / 2f, 0) * transform.forward;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + L * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + R * viewDistance);
    }
}
