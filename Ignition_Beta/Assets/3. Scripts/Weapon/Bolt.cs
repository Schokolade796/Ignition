using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class Bolt : MonoBehaviour
{
    private Interactable interactable;
    private SpringJoint joint;
    private Rigidbody rb;
    private MagazineSystem magazineSystem;
    public Socket socket;
    public Gun gun;

    public GameObject round;
    public GameObject cartridge;
    public GameObject ejectPoint;
    private Vector3 originPosition;
    private Quaternion originRotation;
    public float endPositionValue;
    public int jointValue;
    public bool redyToShot;
    public bool boltRetraction;

    public float impulsePower;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        interactable = GetComponent<Interactable>();
        joint = GetComponent<SpringJoint>();
        originPosition = transform.localPosition;
        originRotation = transform.localRotation;
    }

    private void Start()
    {
        StartCoroutine("BoltAction");
    }

    IEnumerator BoltAction()
    {
        while (true)
        {
            //magazineSystem = gun.magazineSystem;
            // �븮���� ���� ȸ�� ���� = �ʱ� ȸ�� ����
            transform.localRotation = originRotation;
            // �븮���� ���� ��ġ = x,y�� �ʱ� ��ġ, z�� ������ ���� ��ġ
            transform.localPosition = new Vector3(originPosition.x, originPosition.y, transform.localPosition.z);
            joint.spring = jointValue; // ������ ����Ʈ �۵�
            if (interactable.attachedToHand != null) // �븮�踦 ��� ���� ��
                joint.spring = 0; // ������ ����Ʈ ����
            if (transform.localPosition.z >= originPosition.z - 0.01f) // �븮���� ���� ��ġ�� �ʱ� ��ġ - 0.01 ��ġ�� ��
            {
                joint.spring = 0; // ������ ����Ʈ ����
                transform.localPosition = originPosition; // �븮�� ���� ��ġ = �ʱ� ��ġ
                if (boltRetraction)
                {
                    redyToShot = true;
                }
            }
            //else
            //    redyToShot = false;
            if (transform.localPosition.z <= originPosition.z - endPositionValue)
            {
                transform.localPosition = new Vector3
                    (originPosition.x, originPosition.y, originPosition.z - endPositionValue);
                boltRetraction = true;
            }
            //if (magazineSystem.BulletCount <= 0 || magazineSystem == null) return;
            if (boltRetraction)
            {
                //magazineSystem.BulletCount -= 1; // �� �߻�� źâ�� �� �Ѿ� ���� -1
                round.SetActive(true);
                cartridge.SetActive(false);
            }
            yield return null;
        }
    }
    public void Shot()
    {
        rb.AddForce(Vector3.back * impulsePower, ForceMode.Impulse);
        round.SetActive(false);
        cartridge.SetActive(true);
        boltRetraction = false;
        redyToShot = false;
    }
}
