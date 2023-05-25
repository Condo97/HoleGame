using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TransitionAnimationController : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private Image topCover;
    [SerializeField] private Image bottomCover;
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI text;

    [Header(" Timings ")]
    [SerializeField] private float buildInDuration;
    [SerializeField] private float stayDuration;
    [SerializeField] private float buildOutDuration;

    [Header(" Settings ")]
    [SerializeField] private float backgroundOpacity;
    //[SerializeField] private float textSize;

    [Header(" Events ")]
    public Action didFinishTransition;


    public static void DoTransitionCoroutine(TransitionAnimationController t)
    {
        // Static method to set active, do animation, and set inactive easily
        t.gameObject.SetActive(true);        

        t.DoTransitionCoroutine(() => t.gameObject.SetActive(false));
    }

    private void Awake()
    {
        // Move Out with no animation
        Move(0.0f, false);

        gameObject.SetActive(false);
    }

    private void Start()
    {
        
    }

    public void DoTransitionCoroutine(Action completion)
    {
        StartCoroutine(AnimateTransition(completion));
    }

    public IEnumerator AnimateTransition(Action completion)
    {
        gameObject.SetActive(true);

        Move(buildInDuration, true);

        yield return new WaitForSeconds(stayDuration);

        Move(buildOutDuration, false, () =>
        {
            didFinishTransition?.Invoke();
            completion?.Invoke();

            gameObject.SetActive(false);
        });
    }

    private void Move(float duration, bool moveIn)
    {
        Move(duration, moveIn, null);
    }

    private void Move(float duration, bool moveIn, Action completion)
    {
        // Move top cover up by its size
        //Vector3 topTargetPos = new Vector3(topCover.transform.position.x, topCover.transform.position.y - topCover.GetComponent<RectTransform>().rect.size.y / 2, topCover.transform.position.z);
        float topTargetYPos;
        if (moveIn)
            topTargetYPos = topCover.transform.position.y - topCover.GetComponent<RectTransform>().rect.size.y / 2;
        else
            topTargetYPos = topCover.transform.position.y + topCover.GetComponent<RectTransform>().rect.size.y / 2;
        LeanTween.moveY(topCover.gameObject, topTargetYPos, duration);
        //topCover.transform.position = topTargetPos;

        // Move bottom cover down by its size
        //Vector3 bottomTargetPos = new Vector3(bottomCover.transform.position.x, bottomCover.transform.position.y - bottomCover.GetComponent<RectTransform>().rect.size.y / 2, bottomCover.transform.position.z);
        float bottomTargetYPos;
        if (moveIn)
            bottomTargetYPos = bottomCover.transform.position.y + bottomCover.GetComponent<RectTransform>().rect.size.y / 2;
        else
            bottomTargetYPos = bottomCover.transform.position.y - bottomCover.GetComponent<RectTransform>().rect.size.y / 2;
        LeanTween.moveY(bottomCover.gameObject, bottomTargetYPos, duration);
        //bottomCover.transform.position = bottomTargetPos;

        // Transition background opacity
        float targetBackgroundOpacity = moveIn ? backgroundOpacity : 0.0f;
        LeanTween.alpha(background.gameObject, targetBackgroundOpacity, duration);

        // Transition text size and opacity
        float targetTextSize = moveIn ? 1.0f : 0.0f;
        float targetTextOpacity = moveIn ? 1.0f : 0.0f;
        LeanTween.scale(text.gameObject, targetTextSize * Vector3.one, duration)
            .setOnComplete(() => completion?.Invoke()); // Do the completion block here since it will call onComplete after duration
        LeanTween.alpha(text.gameObject, targetTextOpacity, duration / 4);
    }

}
