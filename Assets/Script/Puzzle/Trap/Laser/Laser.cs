using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.MaterialProperty;
[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    [Header("Laser Setting")]
    public float laserRange = 50f;
    public LayerMask hitLayers;

    [SerializeField] private Type laserType;
    private LineRenderer lineRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        ShootLaser();
    }

    private void ShootLaser()
    {
        lineRenderer.SetPosition(0, transform.position);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, laserRange, hitLayers))
        {
            // ถ้าชนวัตถุ (ไม่ว่าจะเป็นกำแพงหรือศัตรู) เลเซอร์จะหยุดวาดแค่จุดที่ชน!
            lineRenderer.SetPosition(1, hit.point);

            PlayerSystem target = hit.collider.GetComponent<PlayerSystem>();
            PlayerRespawn respawnSystem = hit.collider.GetComponent<PlayerRespawn>();

            if (target != null)
            {
                if (laserType == Type.None || target.GetPlayerType() != laserType)
                {
                    //take damage or reset pos player
                    Debug.Log("Destroy");
                    respawnSystem.Respawn();
                }
            }
        }
        else
        {
            // ถ้าไม่ชนอะไรเลย ให้เลเซอร์พุ่งออกไปจนสุดระยะ
            lineRenderer.SetPosition(1, transform.position + transform.forward * laserRange);
        }
    }
}
