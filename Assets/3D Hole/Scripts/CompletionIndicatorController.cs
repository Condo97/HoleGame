using Platinio.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompletionIndicatorController : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private Image fillImage;
    [SerializeField] private float completionPercentage = 0.80f;
    [SerializeField] private AnimationCurve bounceCurve;
    [SerializeField] private float shakeAmount = 1.4f;
    private bool isShaking = false;


    // Start is called before the first frame update
    void Start()
    {
        // Start with the fill image showing empty
        fillImage.fillAmount = 0;

        // Subscribe to collectibleCollected
        CollectedManager.collected += CollectedCallback;
    }

    private void OnDestroy()
    {
        CollectedManager.collected -= CollectedCallback;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ScaleDown()
    {
        LeanTween.scale(gameObject, Vector3.one, 0.1f * Time.deltaTime * 60);
    }

    void ResetShake()
    {
        LeanTween.rotateAroundLocal(gameObject, Vector3.forward, 0f, 0.1f * Time.deltaTime * 60)
            .setEase(LeanTweenType.easeShake);
        isShaking = false;
    }

    void CollectedCallback(Collectible collectible)
    {
        float totalEaten = CollectedManager.instance.GetTotalCollectedSize();
        float totalToEat = LevelManager.instance.GetTotalValuesToEat();
        float targetFillAmount = totalEaten / (totalToEat * completionPercentage);

        // Fill image
        LeanTween.value(fillImage.fillAmount, targetFillAmount, 0.2f * Time.deltaTime * 60)
            .setOnUpdate((value) => fillImage.fillAmount = value);

        // Do animation to note filled
        //LeanTween.easeInOutBounce(gameObject.transform.localScale.magnitude, gameObject.transform.localScale.magnitude, 0.2f * Time.deltaTime * 60);
        LeanTween.scale(gameObject, 1.05f * Vector3.one, 0.2f * Time.deltaTime * 60)
            .setOnComplete((value) => ScaleDown());

        if (!isShaking)
        {
            LeanTween.rotateAroundLocal(gameObject, Vector3.forward, shakeAmount, 0.2f * Time.deltaTime * 60)
                .setEase(bounceCurve)
                .setOnComplete((value) => ResetShake());
            isShaking = true;
        }


        //transform.ScaleTween(Vector3.one, 0.2f * Time.deltaTime * 60);
        

        // Do animation if complete

    }

}
