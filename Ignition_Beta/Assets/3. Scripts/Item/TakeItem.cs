using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class TakeItem : MonoBehaviour
{
    [Tooltip("0: Pistol, 1: Rifle, 2: Shotgun")]
    [SerializeField]
    GameObject[] itemPrefab; // 0: Pistol, 1: Rifle, 2: Shotgun

    [SerializeField]
    Hand leftHand, rightHand; // �÷��̾� �޼�, ������ 

    GameObject currentObject; // ���� ��� �ִ� ������Ʈ

    GameObject spawn; // ��ȯ�� ������

    //[SerializeField]
    //int itemCount = 15;

    /// <summary>
    /// ��� �ִ� �ѿ� �˸��� ������ ��ȯ
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(1);
        if (other.CompareTag("Hand"))
        {
            if (leftHand.currentAttachedObject == null && rightHand.currentAttachedObject == null) return; // ��� �� ��� ���� ���� ��
            if (leftHand.currentAttachedObject != null && rightHand.currentAttachedObject != null) return; // ��� �� ��� ���� ��

            if (leftHand.currentAttachedObject != null) // �޼տ� ��� �ִ� ������Ʈ�� ���� ��
                currentObject = leftHand.currentAttachedObject; // currentObject�� ��� �ִ� ������Ʈ �߰�
                
            else if (rightHand.currentAttachedObject != null) // �����տ� ��� �ִ� ������Ʈ�� ���� ��
                currentObject = rightHand.currentAttachedObject; // currentObject�� ��� �ִ� ������Ʈ �߰�

            switch (currentObject.tag) // ��� �ִ� ������Ʈ�� �±׸� switch ������ Ǯ��
            {
                case "Pistol": // ������ ��
                    break;
                case "Rifle": // ������ ��
                    // źâ ��ȯ �� spawn�� �߰�
                    spawn = Instantiate(itemPrefab[1], other.transform.position, Quaternion.identity, other.transform);
                    break;
                case "Shotgun": // ������ ��
                    break;
                default: 
                    break;
            }
            foreach (var mesh in spawn.GetComponentsInChildren<MeshRenderer>())
                mesh.enabled = false;
            foreach (var canvas in spawn.GetComponentsInChildren<Canvas>())
                canvas.enabled = false;
            spawn.GetComponent<Rigidbody>().isKinematic = true; // spawn�� Rigidbody�� ���� �� isKenematic �ѱ�
        }
    }

    /// <summary>
    /// ���ڿ��� ���� ���� �� ����
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (currentObject != null && other.CompareTag("Hand"))
        {
            if (leftHand.currentAttachedObject != null && rightHand.currentAttachedObject != null) // ��� �� ��� ���� ��
            {
                foreach (var mesh in spawn.GetComponentsInChildren<MeshRenderer>())
                    mesh.enabled = true;
                foreach (var canvas in spawn.GetComponentsInChildren<Canvas>())
                    canvas.enabled = true;
                spawn.GetComponent<Rigidbody>().isKinematic = false; // isKenematic ����
                spawn.transform.SetParent(null); // spawn�� �θ� ����
            }
            else Destroy(spawn); // �ƴϸ� Destroy
        }
    }

    //private void HandHoverUpdate(Hand hand) �̰� ��ü����
    //{
    //    GrabTypes grab = hand.GetGrabStarting();
    //    bool isgrab = hand.IsGrabEnding(spawn);
    //    if (grab == GrabTypes.Grip && !isgrab)
    //    {

    //    }
    //}
}
