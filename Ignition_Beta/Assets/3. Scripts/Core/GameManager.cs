using Michsky.UI.Shift;
using System.Collections;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEditorInternal;
using UnityEngine;
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
    private Drone drone;
    private ModalWindowManager window;
    

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
        drone = FindObjectOfType<Drone>(true);
        window = FindObjectOfType<ModalWindowManager>();
    }

    public void ClearEnemy()
    {

    }

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    /// <summary>
    /// ��� ���� �̺�Ʈ �Լ�
    /// </summary>
    public void DefSuccessEvent()
    {
        lookOut.DefSuccessAnimation();  
        enemyGenerate.canSpawn = false;
    }

    /// <summary>
    /// ��� ���� �̺�Ʈ �Լ�
    /// </summary>
    public void DefFailureEvent()
    {
        window.gameObject.SetActive(true);
    }
    
    /// <summary>
    /// ��� ���� �� ���ư��� �̺�Ʈ �Լ�
    /// </summary>
    public void DefEscapeEvent()
    {
        drone.Animator.SetBool("TimeOut", true);
    }
}
