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
    [SerializeField] private bool ableAutomaticFire;

    public SteamVR_Action_Boolean fireAction;
    public SteamVR_Action_Boolean ejectMagazine;
    public SteamVR_Action_Boolean changeFireMode;
    public GameObject bulletPref;
    private Rigidbody rb;
    //public Transform gunTransform; // 총기의 Transform
    //public float recoilAmount = 2f; // 반동의 세기
    //public float recoilSpeed = 5f; // 반동이 원위치로 돌아오는 속도

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 recoilOffset;
    private Quaternion recoilRotation;

    public float shootingSpeed = 1f;
    public float recoil = 5;
    private float currentTime;
    private int fireMode = 1;

    public MagazineSystem magazineSystem;
    private Interactable interactable;
    private Socket socket;
    private Bolt bolt;

    public static float Damage { get; set; }

    private void Start()
    {
        interactable = GetComponent<Interactable>();
        //socket = transform.Find("Body").Find("Socket").gameObject;
        socket = GetComponentInChildren<Socket>();
        bolt = GetComponentInChildren<Bolt>();
        currentTime = fireTime;
        //originalPosition = gunTransform.localPosition;
        //originalRotation = gunTransform.localRotation;
    }
    private void FixedUpdate()
    {
        if (socket.IsMagazine)
            magazineSystem = GetComponentInChildren<MagazineSystem>();
        if (currentTime <= fireTime && fireMode == 3)
            currentTime += Time.deltaTime;
        // 총을 잡고 있을 때 실행
        if (interactable.attachedToHand != null)
        {
            SteamVR_Input_Sources source = interactable.attachedToHand.handType;
            Damage = 30;

            // 발사 모드 변경
            if (changeFireMode[source].stateDown && ableAutomaticFire)
                fireMode = 4 - fireMode;

            // 탄창 결합여부와 총알 개수 확인
            if (magazineSystem != null && magazineSystem.BulletCount > 0 && bolt.redyToShot)
            {
                if (fireAction[source].lastState != fireAction[source].stateDown) // 트리거를 눌렀을 때 작동
                {
                    Fire();
                }
                else // 트리거가 눌려있지 않을 경우 발사 지연시간 초기화
                    currentTime = fireTime;

                if (ejectMagazine[source].stateDown) // 탄창 분리
                    magazineSystem.ChangeMagazine();
            }
            else if (fireAction[source].stateDown) // 탄창이 없거나 총알을 모두 소진했을 경우 사운드 재생
                audioSource.PlayOneShot(emptyShotSound);
        }
        //gunTransform.localPosition = 
        //    Vector3.Lerp(gunTransform.localPosition, originalPosition + recoilOffset, Time.deltaTime * recoilSpeed);
        //gunTransform.localRotation = 
        //    Quaternion.Slerp(gunTransform.localRotation, originalRotation * recoilRotation, Time.deltaTime * recoilSpeed);
    }
    void Fire()
    {
        // 발사 지연시간
        if (currentTime >= fireTime)
        {
            // 총알 생성
            Rigidbody bulletrb = Instantiate(bulletPref, firePoint.position, firePoint.rotation).GetComponent<Rigidbody>();
            bulletrb.velocity = firePoint.forward * shootingSpeed; // 총알의 발사 방향 및 속도
            muzzelFlash.Play(); // 총구 화염 이펙트 재생
            audioSource.PlayOneShot(shotSound); // 발사 사운드 재생
            GetComponentInChildren<MagazineSystem>().BulletCount -= 1; // 총 발사시 탄창의 총 총알 개수 -1
            muzzleLight.SetActive(true); // 총구 화염 라이트 켜기
            Invoke("HideLight", 0.1f); // 0.1초 후 총구 화염 라이트 끄기
            currentTime = 0; // 발사 지연시간 초기화
        }
    }

    void HideLight()
    {
        muzzleLight.SetActive(false);
    }

    //void ApplyRecoil()
    //{
    //    // 랜덤한 반동 적용 (위쪽과 좌우로)
    //    recoilOffset = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(0.1f, 0.2f), 0) * recoilAmount;
    //    recoilRotation = Quaternion.Euler(new Vector3(-Random.Range(2f, 5f), Random.Range(-1f, 1f), 0) * recoilAmount);
    //}
}