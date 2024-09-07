using Michsky.UI.Shift;
using System.Collections;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEditorInternal;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null) 
                return null;
            return instance;
        }
    }

    public Barrier barrier;
    public Player player;
    public EnemyGenerate enemyGenerate;
    public LookOut lookOut;
    public Drone drone;
    public ModalWindowManager window;

    public SteamVR_Action_Vibration hapticAction;
    [SerializeField]
    private float shakeAmount = 0.2f;
    public float ShakeAmount
    {
        get { return shakeAmount; }
    }
    [SerializeField]
    private float shakeSpeed = 1.0f;
    [SerializeField]
    private float vibrate = 0.1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        ReGetCom();
        // gameClearObject = GameObject.Find("Turret").GetComponent<GameObject>();
    }

    public void ReGetCom()
    {
        player = FindObjectOfType<Player>();
        barrier = FindObjectOfType<Barrier>();
        lookOut = FindObjectOfType<LookOut>();
        enemyGenerate = FindObjectOfType<EnemyGenerate>();
        drone = FindObjectOfType<Drone>();
        window = FindObjectOfType<ModalWindowManager>(true);
    }

    public void ClearEnemy()
    {

    }

    /// <summary>
    /// ��� ���� �̺�Ʈ �Լ�
    /// </summary>
    public void DefFailureEvent()
    {
        drone.Animator.Play("DefenceFailure");
        window.gameObject.SetActive(true);
    }
    
    /// <summary>
    /// ��� ���� �� ���ư��� �̺�Ʈ �Լ�
    /// </summary>
    public void DefEscapeEvent()
    {
        window.ModalWindowOut();
        drone.Animator.Play("DefenceEscape");
        StartCoroutine(PlayerShake(10, 0));
    }

    /// <summary>
    /// ���潺 ���� �̺�Ʈ
    /// </summary>
    public void DefSuccessEvent()
    {
        lookOut.DefSuccessAnimation();
        enemyGenerate.canSpawn = false;
    }

    /// <summary>
    /// �÷��̾� �����ֱ� (��Ʈ�ѷ� ����)
    /// </summary>
    /// <returns>Null</returns>
    public IEnumerator PlayerShake(float time, float amount)
    {
        Vector3 originPosition = player.transform.localPosition;
        float elapsedTime = 0.0f;

        while (elapsedTime <= time)
        {
            Vector3 randomPoint = originPosition + Random.insideUnitSphere * amount;
            player.transform.localPosition = Vector3.Lerp(player.transform.localPosition, randomPoint, Time.deltaTime * shakeSpeed);
            yield return null;

            elapsedTime += Time.deltaTime;
            int ran = Random.Range(0, 2);
            Pulse(.01f, 150, ran, SteamVR_Input_Sources.LeftHand);
            Pulse(.01f, 150, ran, SteamVR_Input_Sources.RightHand);
        }
        // player.transform.localPosition = originPosition;
    }

    /// <summary>
    /// ��Ʈ�ѷ� ����
    /// </summary>
    /// <param name="duration">���ӽð�</param>
    /// <param name="frequency">Hz (�� �ٲ㵵 ū ��ȭ ����)</param>
    /// <param name="amplitude">���� ����</param>
    /// <param name="source">��Ʈ�ѷ� ����</param>
    private void Pulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources source)
    {
        hapticAction.Execute(0, duration, frequency, amplitude, source);
    }
}
