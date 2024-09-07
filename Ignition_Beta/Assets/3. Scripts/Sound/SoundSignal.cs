using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SoundSignal : MonoBehaviour
{
    [SerializeField, Tooltip("�νĽ�Ű�� ���� ������Ʈ")]
    private AudioSource audioSource;
    [SerializeField, Tooltip("Pivot Transfoem")]
    private Transform followObject;
    [SerializeField, Tooltip("�Ҹ� ũ�� (���� �ν��ϴ� �켱����)")]
    private int soundVolume;
    [SerializeField]
    private LayerMask layer;

    private Collider[] colliders;
    private float audioMaxDistance;

    private void Awake()
    {
    }
    void Start()
    {
        audioMaxDistance = audioSource.maxDistance;
        StartCoroutine(SoundSignalToTheEnemy());
    }

    IEnumerator SoundSignalToTheEnemy()
    {
        while (true)
        {
            yield return null;
            if (!audioSource.isPlaying) yield break;
            colliders = Physics.OverlapSphere(followObject.position, audioMaxDistance, layer);
            Debug.Log(1);
            if (colliders.Length > 0)
            {
                foreach (Collider col in colliders)
                {
                    if (col.TryGetComponent<EnemyMove>(out var h))
                    {
                        h.Change(soundVolume, followObject);
                    }
                }
            }
        }
    }

    //private void OnDrawGizmos() // �׽�Ʈ �� ������ ��
    //{
    //    audioMaxDistance = audioSource.maxDistance;
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, audioMaxDistance);
    //}
}









//�� �� T1��?????????????????????????????????? �̰� ����� ������;;
//�� �� T1��?????????????????????????????????? �̰� ����� ������;;
//�� �� T1��?????????????????????????????????? �̰� ����� ������;;
//�� �� T1��?????????????????????????????????? �̰� ����� ������;;
//�� �� T1��?????????????????????????????????? �̰� ����� ������;;
//�� �� T1��?????????????????????????????????? �̰� ����� ������;;