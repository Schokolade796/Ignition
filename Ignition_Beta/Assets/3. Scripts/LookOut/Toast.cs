using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Toast : MonoBehaviour
{
    [SerializeField]
    Vector3 offset;

    [SerializeField]
    TextMeshProUGUI toastMsg;
    [SerializeField]
    float fadeTime = 0.3f;

    bool interrupt;

    public Camera mainCamera;

    private static Toast instance;
    public static Toast Instance
    {
        get
        {
            if (instance == null)
                return null;
            return instance;
        }
    }

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
    }

    private void Start()
    {
        // Show("�ȳ��ϼ���!", 10.0f, new Color(0.56f, 1, 0.43f));
    }

    private void Update()
    {
        FollowCamera();
    }

    /// <summary>
    /// UI�� ī�޶� ���󰡱� ���� �Լ�
    /// </summary>
    /// <returns>Null</returns>
    void FollowCamera()
    {
        transform.position = Vector3.Lerp(transform.position, mainCamera.transform.position 
            + mainCamera.transform.forward * offset.z 
            + mainCamera.transform.up * offset.y 
            + mainCamera.transform.right * offset.x, 
            3 * Time.deltaTime);

        Vector3 l_vector = mainCamera.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(-l_vector).normalized;
    }

    struct TOAST
    {
        public string msg;
        public float durationTime;
        public Color color;
    }

    Queue<TOAST> queue = new();
    bool isPopUp;

    /// <summary>
    /// Toast �޽����� �����ֱ� ���� �Լ� (�̰ɷ� �޽��� ����)
    /// </summary>
    /// <param name="msg">�޽��� ����</param>
    /// <param name="durationTime">�����ִ� �ð�</param>
    /// <param name="color">�޽��� ����</param>
    public void Show(string msg, float durationTime, Color? color = null)
    {
        TOAST toast;

        toast.msg = msg;
        toast.durationTime = durationTime;
        toast.color = color ?? new Color(1, 1, 1, 0);

        queue.Enqueue(toast);
        if (!isPopUp) StartCoroutine(ShowToastQueue());
    }

    /// <summary>
    /// Toast �޽����� �����ֱ� ���� ť �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns>Coroutine</returns>
    IEnumerator ShowToastQueue()
    {
        isPopUp = true;

        while (queue.Count != 0)
        {
            TOAST toast = queue.Dequeue();
            yield return StartCoroutine(ShowMessageCoroutine(toast.msg, toast.durationTime, toast.color));
        }
    }

    /// <summary>
    /// Toast �޽����� �����ֱ� ���� �ڷ�ƾ �Լ�
    /// </summary>
    /// <param name="msg">�޽��� ����</param>
    /// <param name="durationTime">�����ִ� �ð�</param>
    /// <param name="color">�޽��� ����</param>
    /// <returns></returns>
    IEnumerator ShowMessageCoroutine(string msg, float durationTime, Color color)
    {
        toastMsg.text = msg;
        toastMsg.color = color;
        toastMsg.enabled = true;

        yield return FadeInOut(toastMsg, fadeTime, color, true);

        float elapsedTime = 0.0f;
        while (elapsedTime < durationTime && !interrupt)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return FadeInOut(toastMsg, fadeTime, color, false);

        interrupt = false;
        toastMsg.enabled = false;
    }

    /// <summary>
    /// �۾� õõ�� ������� ����� �ϱ�
    /// </summary>
    /// <param name="target">�۾�</param>
    /// <param name="durationTime">������� ����� �ð�(��)</param>
    /// <param name="color">�۾� ����</param>
    /// <param name="inOut">true = in -> out</param>
    /// <returns>Null</returns>
    IEnumerator FadeInOut(TextMeshProUGUI target, float durationTime, Color color, bool inOut) 
    {
        float start, end;
        if (inOut)
        {
            start = 0.0f;
            end = 1.0f;
        }
        else
        {
            start = 1.0f;
            end = 0.0f;
        }

        float elapsedTime = 0.0f;

        while (elapsedTime < durationTime)
        {
            float alpha = Mathf.Lerp(start, end, elapsedTime / durationTime);

            target.color = new Color(color.r, color.g, color.b, alpha);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }

    /// <summary>
    /// Toast �޽��� �ٷ� ���߰� ���� �� ���� �Լ�
    /// </summary>
    public void InterruptMessage()
    {
        interrupt = true;
    }
}
