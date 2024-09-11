using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Gun : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private VisualEffect muzzelFlash;
    [SerializeField] private GameObject muzzleLight;
    [SerializeField] private AudioClip shotSound;
    [SerializeField] private AudioClip emptyShotSound;
    [SerializeField] private float fireTime;
    [SerializeField] private bool ableAutomaticFire;
    public bool isShotgun;
    public int spawnPrefabAmount;
    Queue<GameObject> poolingObjectQueue = new Queue<GameObject>();
    private bool canFire;

    public float shootingSpeed = 1f;
    private int fireMode = 1;
    public GameObject bulletPref;

    public Interactable interactable;
    public Socket socket;
    public Bolt bolt;
    private MagazineSystem magazineSystem;
    private Hand currentHand = null;

    [Header("Recoil")]
    public float maxRecoil;
    public float minRecoil;
    public Interactable secondInteractable;
    private Rigidbody gunRb;
    private float recoilPower;

    [Header("Action Button")]
    public SteamVR_Action_Boolean fireAction;
    public SteamVR_Action_Boolean ejectMagazine;
    public SteamVR_Action_Boolean changeFireMode;

    [HideInInspector]
    public bool isGrab;

    private void Awake()
    {
        gunRb = GetComponent<Rigidbody>();
        Initialize(spawnPrefabAmount);
    }

    private void Start()
    {
        StartCoroutine("GunWork");
    }

    IEnumerator GunWork()
    {
        while (true)
        {
            if (socket.isMagazine) // źâ�� ���� ��� ��ũ��Ʈ ��������
                magazineSystem = GetComponentInChildren<MagazineSystem>();
            else // �ƴѰ�� ��ũ��Ʈ NULL
                magazineSystem = null;
            bolt.magazineSystem = magazineSystem;

            if (secondInteractable.attachedToHand != null) // �ٸ� �յ� ���� ��� �ִٸ� �ݵ� ���̱�
                recoilPower = minRecoil;
            else // �ƴ϶�� �ִ� �ݵ�
                recoilPower = maxRecoil;

            if (fireMode == 3) // ���� ����� �� �׻� �߻� ����
                canFire = true;
            
            if (interactable.attachedToHand != null) // ���� ��� ���� �� ����
                CanFire();

            else // ��� ���� ���� ���
                isGrab = false;

            yield return null;
        }
    }

    void Fire()
    {
        // �߻� �����ð�
        if (canFire)
        {
            if (!isShotgun)
            {
                // �Ѿ� ����
                Rigidbody bulletrb = Instantiate
                    (bulletPref, muzzelFlash.transform.position, muzzelFlash.transform.rotation).GetComponent<Rigidbody>();
                bulletrb.velocity = muzzelFlash.transform.forward * shootingSpeed; // �Ѿ��� �߻� ���� �� �ӵ�
            }
            else
            {
                for (int i = 0; i < 25; i++)
                {
                    Rigidbody bulletrb = Instantiate
                    (bulletPref, muzzelFlash.transform.position,
                    Quaternion.Euler(new Vector3(
                            muzzelFlash.transform.rotation.x + Random.Range(-20, 20),
                            muzzelFlash.transform.rotation.y + Random.Range(-20, 20),
                            muzzelFlash.transform.rotation.z))).GetComponent<Rigidbody>();
                    bulletrb.velocity = muzzelFlash.transform.forward * shootingSpeed;
                }
            }
            muzzelFlash.Play(); // �ѱ� ȭ�� ����Ʈ ���
            audioSource.PlayOneShot(shotSound); // �߻� ���� ���
            gunRb.AddRelativeForce(Vector3.back * recoilPower, ForceMode.Force);
            gunRb.AddRelativeTorque(Vector3.left * recoilPower, ForceMode.Force);
            magazineSystem.bulletCount -= 1; // �� �߻�� źâ�� �� �Ѿ� ���� -1
            bolt.Shot();
            muzzleLight.SetActive(true); // �ѱ� ȭ�� ����Ʈ �ѱ�
            Invoke("HideLight", 0.1f); // 0.1�� �� �ѱ� ȭ�� ����Ʈ ����
            canFire = false; // �߻� �Ұ���
        }
    }

    private void CanFire()
    {
        SteamVR_Input_Sources source = interactable.attachedToHand.handType;

        isGrab = true;
        // �߻� ��� ����
        if (changeFireMode[source].stateDown && ableAutomaticFire) // ����,�ܹ� ����
            fireMode = 4 - fireMode;
        if (ejectMagazine[source].stateDown && magazineSystem != null) // źâ �и�
            magazineSystem.ChangeMagazine();

        if (bolt.redyToShot) // �븮�谡 �غ� �Ǿ��� ��
        {
            if (fireAction[source].lastState != fireAction[source].stateDown) // Ʈ���Ÿ� ������ �� �۵�
                Fire();
            else // Ʈ���Ű� �������� ���� ��� �߻� ����
                canFire = true;
        }
        else if (fireAction[source].stateDown) // �غ���� �ʾҴٸ� �Ҹ� ���
            audioSource.PlayOneShot(emptyShotSound);
    }

    void HideLight()
    {
        muzzleLight.SetActive(false);
    }

    private void HandHoverUpdate(Hand hand)
    {
        // ���� ���� ��ü�� ��� �ִ��� Ȯ��
        if (interactable.attachedToHand != null)
        {
            // �ٸ� ���� ������ �ϸ�, ���� ���ϰ� ����
            if (currentHand != null && currentHand != hand)
            {
                hand.DetachObject(gameObject); // �ٸ� ������ ���� ���ϰ� ��ü �и�
            }
        }
    }

    private void OnAttachedToHand(Hand hand)
    {
        // ��ü�� �տ� ������, �ش� ���� ���
        currentHand = hand;
    }

    private void OnDetachedFromHand(Hand hand)
    {
        // ��ü�� �տ��� ��������, ���� �� ����� �ʱ�ȭ
        if (currentHand == hand)
        {
            currentHand = null;
        }
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
        var newObj = Instantiate(bulletPref);
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public void GetObject()
    {
        if (poolingObjectQueue.Count > 0)
        {
            var obj = poolingObjectQueue.Dequeue();
            obj.transform.position = muzzelFlash.transform.position;
            obj.SetActive(true);
        }
        else
        {
            var newObj = CreateNewObject();
            newObj.transform.position = muzzelFlash.transform.position;
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