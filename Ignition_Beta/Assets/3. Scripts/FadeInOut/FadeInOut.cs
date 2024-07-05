using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class FadeInOut : MonoBehaviour
{
    public Image _image;
    public float _fadeTime = 1.5f;
    [Tooltip ("FadeIn ���� 0.1�� �ö󰡴� ������ Ű ���� �ϳ� �߰����ּ���. (Ű �� �� 3��)")]
    public AnimationCurve _fadeCurve;

    [SerializeField] private UnityEvent onStartEvent;

    private Camera mainCamera;
    private bool fadeIn = false;
    private bool fadeOut = false;
    private void Start()
    {
        mainCamera = GetComponentInParent<Camera>();
        if (onStartEvent != null)
        {
            onStartEvent.Invoke();
        }
    }
    public void StartFadeIn()
    {
        StartCoroutine(Fade(1, 0));
    }
        
    public void StartFadeOut()
    {
        StartCoroutine(Fade(0, 1));
    }

    private IEnumerator Fade(float start, float end)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / _fadeTime;
            Color color = _image.color;
            color.a = Mathf.Lerp(start, end, _fadeCurve.Evaluate(percent));
            _image.color = color;

            yield return null;

        }
    }
}
