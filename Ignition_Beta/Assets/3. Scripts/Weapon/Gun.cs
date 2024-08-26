using UnityEngine;
using UnityEngine.VFX;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private VisualEffect muzzelFlash;
    [SerializeField] private GameObject muzzleLight;
    [SerializeField] private AudioClip shotSound;
    [SerializeField] private AudioClip emptyShotSound;
    [SerializeField] private float fireTime;

    public SteamVR_Action_Boolean fireAction;
    public SteamVR_Action_Boolean ejectMagazine;
    public SteamVR_Action_Boolean changeFireMode;
    public GameObject bulletPref;
    private Rigidbody rb;
    //public Transform gunTransform; // �ѱ��� Transform
    //public float recoilAmount = 2f; // �ݵ��� ����
    //public float recoilSpeed = 5f; // �ݵ��� ����ġ�� ���ƿ��� �ӵ�

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 recoilOffset;
    private Quaternion recoilRotation;

    public float shootingSpeed = 1f;
    public float recoil = 5;
    private int fireMode = 1;
    private float currentTime;

    private Interactable interactable;
    private GameObject socket;

    public static float Damage { get; set; }

    private void Start()
    {
        interactable = GetComponent<Interactable>();
        socket = transform.Find("Body").Find("Socket").gameObject;
        currentTime = fireTime;
        //originalPosition = gunTransform.localPosition;
        //originalRotation = gunTransform.localRotation;
    }
    private void FixedUpdate()
    {
        if (this.gameObject.tag == "Pistol")
        {
            Damage = 20;
        }
        else if (this.gameObject.tag == "Rifle")
        {
            Damage = 30;
            //if (changeFireMode[source].stateDown)
            //{
            //    fireMode = 4 - fireMode;
            //}
        } // ���� �ʿ�

        if (currentTime <= fireTime)
        {
            currentTime += Time.deltaTime;
        }
        // ���� ��� ���� �� ����
        if (interactable.attachedToHand != null)
        {
            SteamVR_Input_Sources source = interactable.attachedToHand.handType;

            if (this.gameObject.tag == "Pistol")
            {
                Damage = 20;
            }
            else if (this.gameObject.tag == "Rifle")
            {
                Damage = 30;
                if (changeFireMode[source].stateDown)
                {
                    fireMode = 4 - fireMode;
                }
            }

            // źâ ���տ��ο� �Ѿ� ���� Ȯ��
            if (GetComponentInChildren<MagazineSystem>() != null && GetComponentInChildren<MagazineSystem>().BulletCount > 0)
            {
                // �߻��庰 �߻� ���
                if (fireMode == 1) // �ܹ�
                {
                    if (fireAction[source].stateDown) // Ʈ���Ÿ� �������� Ȯ��
                    {
                        Fire();
                        //ApplyRecoil();
                    }
                }
                else // ����
                {
                    if (fireAction[source].lastState != fireAction[source].stateDown) // Ʈ���Ű� �����ִ��� Ȯ��
                    {
                        if (currentTime >= fireTime) // �߻� �����ð�
                        {
                            Fire();
                            currentTime = 0;
                            //ApplyRecoil();
                        }
                    }
                    else // Ʈ���Ű� �������� ���� ��� �߻� �����ð� �ʱ�ȭ
                        currentTime = fireTime;
                }
                if (ejectMagazine[source].stateDown) // źâ �и�
                {
                    GetComponentInChildren<MagazineSystem>().ChangeMagazine();
                }
            }
            else if (fireAction[source].stateDown) // źâ�� ���ų� �Ѿ��� ��� �������� ��� ���� ���
            {
                audioSource.PlayOneShot(emptyShotSound);
            }
        }
        //gunTransform.localPosition = 
        //    Vector3.Lerp(gunTransform.localPosition, originalPosition + recoilOffset, Time.deltaTime * recoilSpeed);
        //gunTransform.localRotation = 
        //    Quaternion.Slerp(gunTransform.localRotation, originalRotation * recoilRotation, Time.deltaTime * recoilSpeed);
    }
    void Fire()
    {   
        // �Ѿ� ����
        Rigidbody bulletrb = Instantiate(bulletPref, firePoint.position, firePoint.rotation).GetComponent<Rigidbody>();
        bulletrb.velocity = firePoint.forward * shootingSpeed; // �Ѿ��� �߻� ���� �� �ӵ�
        muzzelFlash.Play(); // �ѱ� ȭ�� ����Ʈ ���
        audioSource.PlayOneShot(shotSound); // �߻� ���� ���
        GetComponentInChildren<MagazineSystem>().BulletCount -= 1; // �� �߻�� źâ�� �� �Ѿ� ���� -1
        muzzleLight.SetActive(true); // �ѱ� ȭ�� ����Ʈ �ѱ�
        Invoke("HideLight", 0.1f); // 0.1�� �� �ѱ� ȭ�� ����Ʈ ����
    }

    void HideLight()
    {
        muzzleLight.SetActive(false);
    }

    //void ApplyRecoil()
    //{
    //    // ������ �ݵ� ���� (���ʰ� �¿��)
    //    recoilOffset = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(0.1f, 0.2f), 0) * recoilAmount;
    //    recoilRotation = Quaternion.Euler(new Vector3(-Random.Range(2f, 5f), Random.Range(-1f, 1f), 0) * recoilAmount);
    //}
}