using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnchor : MonoBehaviour
{
    public Transform playerCollider;  // �÷��̾� �ݶ��̴�
    public Transform head;

    void Update()
    {
        // HMD�� ��ġ�� �÷��̾� �ݶ��̴��� ��ġ�� ����ȭ
        Vector3 newPosition = new Vector3(head.position.x, playerCollider.position.y, head.position.z);
        playerCollider.position = newPosition;
    }
}
