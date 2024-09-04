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
    public Gun gun;
    public Socket socket;

    public GameObject round;
    public GameObject cartridge;
    public GameObject ejectBullet;
    private Vector3 originPosition;
    private Quaternion originRotation;
    public float endPositionValue;
    public int jointValue;
    public bool redyToShot;
    private bool boltRetraction;
    public float impulsePower;

    Queue<GameObject> poolingObjectQueue = new Queue<GameObject>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        interactable = GetComponent<Interactable>();
        joint = GetComponent<SpringJoint>();
        originPosition = transform.localPosition;
        originRotation = transform.localRotation;
        Initialize(30);
    }

    private void Start()
    {
        StartCoroutine("BoltAction");
    }

    IEnumerator BoltAction()
    {
        while (true)
        {
            if (socket.IsMagazine)
                magazineSystem = gun.magazineSystem;
            else
                magazineSystem = null;
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
            else
                redyToShot = false;
            if (transform.localPosition.z <= originPosition.z - endPositionValue)
            {
                transform.localPosition = new Vector3
                    (originPosition.x, originPosition.y, originPosition.z - endPositionValue);
                cartridge.SetActive(false);
                if (magazineSystem != null && magazineSystem.BulletCount >= 0)
                    boltRetraction = true;
            }
            if (boltRetraction)
            {
                round.SetActive(true);
            }
            yield return null;
        }
    }
    public void Shot()
    {
        rb.AddForce(Vector3.back * impulsePower, ForceMode.Impulse);
        round.SetActive(false);
        cartridge.SetActive(true);
        GetObject();
        boltRetraction = false;
        redyToShot = false;
    }

    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject());
        }
    }

    private GameObject CreateNewObject()
    {
        var newObj = Instantiate(ejectBullet);
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public void GetObject()
    {
        if (poolingObjectQueue.Count > 0)
        {
            var obj = poolingObjectQueue.Dequeue();
            obj.transform.position = transform.position;
            obj.SetActive(true);
        }
        else
        {
            var newObj = CreateNewObject();
            newObj.transform.position = transform.position;
            newObj.SetActive(true);
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);
        poolingObjectQueue.Enqueue(obj);
    }
}
