using System.Collections;
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

    public SteamVR_Action_Boolean fireAction;
    public SteamVR_Action_Boolean ejectMagazine;
    public SteamVR_Action_Boolean changeFireMode;
    public GameObject bulletPref;

    public float shootingSpeed = 1f;
    public float recoil = 5;
    private float currentTime;
    private int fireMode = 1;

    public Interactable interactable;
    public MagazineSystem magazineSystem;
    public Socket socket;
    public Bolt bolt;

    public bool isGrab;

    private void Start()
    {
        StartCoroutine("GunWork");
        currentTime = fireTime; // �߻� �����ð� �ʱ�ȭ
    }
    void Fire()
    {
        // �߻� �����ð�
        if (currentTime <= fireTime) return;
        if (bolt.redyToShot) // �븮�谡 �غ� �Ǿ��� ��
        {
            // �Ѿ� ����
            Rigidbody bulletrb = Instantiate
                (bulletPref, muzzelFlash.transform.position, muzzelFlash.transform.rotation).GetComponent<Rigidbody>();
            bulletrb.velocity = muzzelFlash.transform.forward * shootingSpeed; // �Ѿ��� �߻� ���� �� �ӵ�
            muzzelFlash.Play(); // �ѱ� ȭ�� ����Ʈ ���
            audioSource.PlayOneShot(shotSound); // �߻� ���� ���
            magazineSystem.BulletCount -= 1; // �� �߻�� źâ�� �� �Ѿ� ���� -1
            bolt.Shot();
            muzzleLight.SetActive(true); // �ѱ� ȭ�� ����Ʈ �ѱ�
            Invoke("HideLight", 0.1f); // 0.1�� �� �ѱ� ȭ�� ����Ʈ ����
            currentTime = 0; // �߻� �����ð� �ʱ�ȭ
        }
        else // �غ���� �ʾҴٸ� �Ҹ� ���
            audioSource.PlayOneShot(emptyShotSound);
    }

    void HideLight()
    {
        muzzleLight.SetActive(false);
    }

    IEnumerator GunWork()
    {
        while (true)
        {
            if (socket.IsMagazine) // źâ�� ���� ��� ��ũ��Ʈ ��������
                magazineSystem = GetComponentInChildren<MagazineSystem>();
            else // �ƴѰ�� ��ũ��Ʈ NULL
                magazineSystem = null;
            isGrab = true;
            if (currentTime <= fireTime && fireMode == 3) // ���� ����϶� �߻� �����ð� �۵�
                currentTime += Time.deltaTime;
            // ���� ��� ���� �� ����
            if (interactable.attachedToHand != null)
            {
                SteamVR_Input_Sources source = interactable.attachedToHand.handType;

                // �߻� ��� ����
                if (changeFireMode[source].stateDown && ableAutomaticFire) // ����,�ܹ� ����
                    fireMode = 4 - fireMode;
                if (ejectMagazine[source].stateDown) // źâ �и�
                    magazineSystem.ChangeMagazine();

                if (fireAction[source].lastState != fireAction[source].stateDown) // Ʈ���Ÿ� ������ �� �۵�
                {
                    Fire();
                }
                else // Ʈ���Ű� �������� ���� ��� �߻� �����ð� �ʱ�ȭ
                    currentTime = fireTime;                    
            }
            else
            {
                isGrab = false;
            }
            yield return null;
        }
    }
}