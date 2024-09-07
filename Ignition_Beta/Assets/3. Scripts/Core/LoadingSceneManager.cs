using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;

    public string defultScene = "Base";

    public bool test; // �׽�Ʈ �� true�� ��� ���� �ε�

    public GameObject LoadingText;
    public TextMeshProUGUI text;
    public Image progressBar;
    [SerializeField, Tooltip("�ε��Ǵ� �ð�(���� �ε����� ������ �ð��� �ּ� 10��)"), Min(10f)]
    private float loadingDuration;

    private void Start()
    {
        if (nextScene == null)
        {
            nextScene = defultScene;
        }
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Loading");
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        float timer = 0.0f;
        float timer2 = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;

            switch (timer)
            {
                case float n when (n >= 1.5f && n <= 9f):
                    timer2 += Time.deltaTime;
                    progressBar.fillAmount = Mathf.Lerp(0 , 1, timer2 / 7.5f);
                    text.text = $"��ǥ���� �����Ÿ�";
                    break;
                case float n when (n >= 9.5f):
                    progressBar.gameObject.SetActive(false);
                    if (op.progress < 0.9f || timer < loadingDuration) // ���� �ε� üũ
                    {
                        text.gameObject.SetActive(false);
                        LoadingText.gameObject.SetActive(true);
                        if (op.progress > 0.9)
                            Debug.Log("���� �ε�");
                    }
                    else if (timer > loadingDuration + 3f && !test) // �ε� �Ϸ�� �� ��ȯ
                    {
                        text.text = $"<color=red>����!</color>";
                        op.allowSceneActivation = true;
                        yield break;
                    }
                    else if (timer > loadingDuration + 2f) // �ڿ������� �ε� �ؽ�Ʈ 
                    {
                        text.text = $"���� ��...";
                    }
                    else if (timer > loadingDuration + 1.5f)
                    {
                        text.text = $"���� ��..";
                    }
                    else if (timer > loadingDuration + 1f)
                    {
                        text.text = $"���� ��.";
                    }
                    else if (timer > loadingDuration) 
                    {
                        text.text = $"���� ���� Ȯ��!";
                        text.gameObject.SetActive(true);
                        LoadingText.gameObject.SetActive(false);
                    }
                    break;
            }
            //if (op.progress < 0.9f)
            //{
                
            //}
            //else
            //{
            //    if (timer > 13f && !test)
            //    {
            //        op.allowSceneActivation = true;
            //        yield break;
            //    }
            //}
        }
    }
}
