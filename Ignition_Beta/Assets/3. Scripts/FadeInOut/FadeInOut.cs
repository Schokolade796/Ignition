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
    private bool fadeIn;
    private bool fadeOut;
    private void Start()
    {
        mainCamera = GetComponentInParent<Camera>();
        if (onStartEvent != null)
        {
            fadeIn = false;
            fadeOut = false; 
            onStartEvent.Invoke();
        }
    }
    public void StartFadeIn()
    {
        StartCoroutine(Fade(1, 0));
        fadeIn = true;
    }
        
    public void StartFadeOut()
    {
        StartCoroutine(Fade(0, 1));
        fadeOut = true;
    }

    private IEnumerator Fade(float start, float end)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / _fadeTime;
            if (fadeIn)
            {
                fadeIn = false;
                if (_fadeCurve.Evaluate(percent) >= 0.01)
                {
                    mainCamera.cullingMask = 1;
                }
            }
            else if (fadeOut)
            {
                fadeOut = false;
                if (_fadeCurve.Evaluate(percent) >= 1)
                {
                    mainCamera.cullingMask = 0;
                    mainCamera.cullingMask = 1 << LayerMask.NameToLayer("Fade");
                }
            }
            Color color = _image.color;
            color.a = Mathf.Lerp(start, end, _fadeCurve.Evaluate(percent));
            _image.color = color;
            yield return null;

        }
    }
}
