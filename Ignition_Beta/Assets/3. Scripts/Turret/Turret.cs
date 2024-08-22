using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField]
    private float radius = 0f;
    [SerializeField]
    private LayerMask layer;
    [SerializeField]
    private Transform turretHead;
    [SerializeField]
    private Transform turretHeadPivot;
    [SerializeField]
    private Transform gunPointL;
    [SerializeField]
    private Transform gunPointR;
    [SerializeField]
    private List<Transform> points;

    public Collider[] colliders; //나중에 private으로 변경하기
    public Collider look_enemy; // ''

    private float lookDelay = 3f;

    private void Start()
    {
        StartCoroutine(GunFire());
    }

    void Update()
    {
        LookEnemy();

        colliders = Physics.OverlapSphere(transform.position, radius, layer);

        if (colliders.Length == 1)
            look_enemy = colliders[0];
        else if (colliders.Length > 0)
        {
            foreach (Collider col in colliders)
            {
                if (Vector3.Distance(transform.position, colliders[0].transform.position) > Vector3.Distance(transform.position, col.transform.position))
                {
                    for (int i = 0; i < colliders.Length; i++)
                    {
                        if (colliders[i] == col)
                        {
                            colliders[i] = colliders[0];
                            colliders[0] = col;
                        }
                    }
                }
            }
        }
    }

    void LookEnemy()
    {
        if (look_enemy != null && colliders.Length > 0)
        {
            look_enemy = colliders[0];

            Vector3 enemyPosition = look_enemy.transform.position;

            Vector3 vector = enemyPosition - turretHeadPivot.position;

            Quaternion turret_Y = transform.rotation;
            Quaternion turret_X = transform.rotation;
            turret_Y.y = Quaternion.LookRotation(vector).normalized.y;
            turret_X.x = Quaternion.LookRotation(vector).normalized.x;
            transform.rotation = Quaternion.Lerp(transform.rotation, turret_Y, Time.deltaTime * lookDelay);
            turretHead.rotation = Quaternion.Lerp(turretHead.rotation, turret_X, Time.deltaTime * lookDelay);
            Quaternion headRotation = turretHead.rotation;
            headRotation.x = Mathf.Clamp(turretHead.rotation.x, -45f, 45f);
        }
    }

    IEnumerator GunFire()
    {
        while (true)
        {
            if (look_enemy != null && colliders.Length > 0)
            {
                look_enemy = colliders[0];

                RaycastHit hit;
                float maxDistance = 10f;
                Vector3 forward = turretHead.forward;
                Vector3 dir = new Vector3(look_enemy.transform.position.x, look_enemy.transform.position.y - .75f, look_enemy.transform.position.z);

                for (int i = 0; i < points.Count; i++)
                {
                    if (points[i].name == "gunpoint_L" || points[i].name == "Trail") continue;
                    Debug.DrawRay(points[i].position, forward * maxDistance, Color.red);
                    if (Physics.Raycast(points[i].position, forward, out hit, maxDistance, layer))
                    {
                        Debug.Log("충돌");
                        points[i].GetChild(0).GetComponent<Rigidbody>().AddForce(points[i].forward * 50f * Time.deltaTime);
                        if (hit.transform.CompareTag("Enemy"))
                        {
                            if (hit.transform.TryGetComponent<IHitAble>(out var h))
                            {
                                Destroy(hit.transform.gameObject);
                            }
                        }
                        // yield return new WaitForSeconds(1f);
                    }
                }

                for (int i = 0; i < points.Count; i++)
                {
                    if (points[i].name == "gunpoint_R" || points[i].name == "Trail") continue;
                    Debug.DrawRay(points[i].position, forward * maxDistance, Color.red);
                    if (Physics.Raycast(points[i].position, forward, out hit, maxDistance, layer))
                    {
                        Debug.Log("충돌");
                        points[i].GetChild(0).GetComponent<Rigidbody>().AddForce(points[i].forward * 50f * Time.deltaTime);
                        // yield return new WaitForSeconds(5f);
                    }
                }
            }
            yield return null;
        }
    }

    private void OnDrawGizmos() // 테스트 후 지워도 됨
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
