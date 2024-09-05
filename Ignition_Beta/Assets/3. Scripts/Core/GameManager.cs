using System.Collections;
using UnityEditor.Experimental.GraphView;
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
    private LookOut lookOut;

    [SerializeField]
    private float shakeAmount = 0.2f;


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
    /// <summary>
    /// �⺻���� ���۳�Ʈ ����
    /// </summary>
    public void ReGetCom()
    {
        player = FindObjectOfType<Player>();
        barrier = FindObjectOfType<Barrier>();
        lookOut = FindObjectOfType<LookOut>();
        enemyGenerate = FindObjectOfType<EnemyGenerate>();
    }

    public void ClearEnemy()
    {

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
    IEnumerator Shake()
    {
        Vector3 originPosition = player.transform.localPosition;
        float elapsedTime = 0.0f;

        while (true)
        {
            Vector3 randomPoint = originPosition + Random.insideUnitSphere * shakeAmount;
            //player.transform.localPosition = Vector3.Lerp(player.transform.localPosition, randomPoint, Time.deltaTime * shakeSpeed);
            yield return null;

            elapsedTime += Time.deltaTime;
        }
        //player.transform.localPosition = originPosition;
    }
    //private void Pulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources source)
    //{
    //    hapticAction.Execute(0, duration, frequency, amplitude, source);
    //    Debug.Log("Pulse " + source.ToString());

    //}
}
