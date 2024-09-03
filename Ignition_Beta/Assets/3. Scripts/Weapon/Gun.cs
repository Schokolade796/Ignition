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
    private float currentTime;
    private int fireMode = 1;

    public Interactable interactable;
    public Socket socket;
    public Bolt bolt;

    public static float Damage { get; set; }

    private void Start()
    {
        StartCoroutine("GunWork");
        currentTime = fireTime;
        //originalPosition = gunTransform.localPosition;
        //originalRotation = gunTransform.localRotation;
    }
    void Fire()
    {
        // �߻� �����ð�
        if (currentTime >= fireTime)
        {
            // �Ѿ� ����
            Rigidbody bulletrb = Instantiate
                (bulletPref, muzzelFlash.transform.position, muzzelFlash.transform.rotation).GetComponent<Rigidbody>();
            bulletrb.velocity = muzzelFlash.transform.forward * shootingSpeed; // �Ѿ��� �߻� ���� �� �ӵ�
            muzzelFlash.Play(); // �ѱ� ȭ�� ����Ʈ ���
            audioSource.PlayOneShot(shotSound); // �߻� ���� ���
            bolt.Shot();
            muzzleLight.SetActive(true); // �ѱ� ȭ�� ����Ʈ �ѱ�
            Invoke("HideLight", 0.1f); // 0.1�� �� �ѱ� ȭ�� ����Ʈ ����
            currentTime = 0; // �߻� �����ð� �ʱ�ȭ
        }
    }

    void HideLight()
    {
        muzzleLight.SetActive(false);
    }

    IEnumerator GunWork()
    {
        while (true)
        {
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
                    bolt.magazineSystem.ChangeMagazine();

                // źâ ���տ��ο� �Ѿ� ���� Ȯ��, ��� Ȯ��
                if (socket.IsMagazine && bolt.redyToShot)
                {
                    if (fireAction[source].lastState != fireAction[source].stateDown) // Ʈ���Ÿ� ������ �� �۵�
                    {
                        Fire();
                    }
                    else // Ʈ���Ű� �������� ���� ��� �߻� �����ð� �ʱ�ȭ
                        currentTime = fireTime;
                }
                else if (fireAction[source].stateDown) // źâ�� ���ų� �Ѿ��� ��� �������� ��� ���� ���
                    audioSource.PlayOneShot(emptyShotSound);
            }
            //gunTransform.localPosition = 
            //    Vector3.Lerp(gunTransform.localPosition, originalPosition + recoilOffset, Time.deltaTime * recoilSpeed);
            //gunTransform.localRotation = 
            //    Quaternion.Slerp(gunTransform.localRotation, originalRotation * recoilRotation, Time.deltaTime * recoilSpeed);
            yield return null;
        }
    }

    //void ApplyRecoil()
    //{
    //    // ������ �ݵ� ���� (���ʰ� �¿��)
    //    recoilOffset = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(0.1f, 0.2f), 0) * recoilAmount;
    //    recoilRotation = Quaternion.Euler(new Vector3(-Random.Range(2f, 5f), Random.Range(-1f, 1f), 0) * recoilAmount);
    //}
}