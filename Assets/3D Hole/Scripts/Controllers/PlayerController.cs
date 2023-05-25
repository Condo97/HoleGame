using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header(" Boss ")]
    [SerializeField] private float bossHoleZ = 20f;

    [Header(" Elements ")]
    [SerializeField] private GameObject ground;
    [SerializeField] private GameObject layerSwitch;

    [Header(" Settings ")]
    [SerializeField] private float screenPositionFollowThreshold;
    [SerializeField] private float initialMoveSpeed;
    [SerializeField] private float moveSpeedMultiplier;
    private Vector3 clickedScreenPosition;
    private float moveSpeed;
    private bool canMove;

    private void Awake()
    {
        // Subscribe HoleParentSizeIncreasedCallback to HoleParentSize onIncrease
        //LayerSwitchSize.onDiameterIncrease += PlayerSizeIncreasedCallback;
        HoleParentSize.onIncrease += PlayerSizeIncreasedCallback;

        // Set moveSpeed
        moveSpeed = initialMoveSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe GameStateChangedCallback to GameManager onStateChanged
        GameManager.onStateChanged += GameStateChangedCallback;

        // Subscribe DisableMovement to PlayerTimer onTimerOver
        TimerManager.onTimerOver += DisableMovement;
    }

    private void OnDestroy()
    {
        // Unsubscribe HoleParentSizeIncreasedCallback from HoleParentSize onIncrease
        //LayerSwitchSize.onDiameterIncrease -= PlayerSizeIncreasedCallback;
        HoleParentSize.onIncrease -= PlayerSizeIncreasedCallback;

        // Unsubscribe GameStateChangedCallback from GameManager onStateChanged
        GameManager.onStateChanged -= GameStateChangedCallback;

        // Unsubscribe DisableMovement from PlayerTimer onTimerOver
        TimerManager.onTimerOver -= DisableMovement;

    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
            ManageControl();
    }

    private void ManageControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // If clicked, get click and store it in a variable
            clickedScreenPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            // If holding, move based on difference of clickedScreenPosition and current holding location
            // Get position difference of holding location vs click location
            Vector3 difference = Input.mousePosition - clickedScreenPosition;

            // Get direction as normalized difference (this returns difference as a vector with a magnatude of 1)
            Vector3 direction = difference.normalized;

            float maxScreenDistance = screenPositionFollowThreshold * Screen.height;

            if (difference.magnitude > maxScreenDistance)
            {
                clickedScreenPosition = Input.mousePosition - direction * maxScreenDistance;
                difference = Input.mousePosition - clickedScreenPosition;
            }

            difference /= Screen.width;

            difference.z = difference.y;
            difference.y = 0;

            Vector3 targetPosition = transform.position + difference * moveSpeed * Time.deltaTime;


            // Ensure targetPosition is within the bounds of the ground (with a little inset)
            float inset = layerSwitch.GetComponent<Collider>().bounds.size.x / 2;
            Bounds insetBounds = ground.GetComponent<Collider>().bounds;
            insetBounds.SetMinMax(
                new Vector3(
                    insetBounds.min.x + inset,
                    insetBounds.min.y,
                    insetBounds.min.z + inset
                ),
                new Vector3(
                    insetBounds.max.x - inset,
                    insetBounds.max.y,
                    insetBounds.max.z - inset
                )
            );

            if (insetBounds.Contains(targetPosition))
                transform.position = targetPosition;
        }
    }

    private void MoveToBoss()
    {
        LeanTween.value(gameObject, transform.position, new Vector3(0, transform.position.y, bossHoleZ), 1f * Time.deltaTime * 60)
            .setOnUpdate((Vector3 vector) => transform.position = vector);
    }

    private void MoveToHome()
    {
        LeanTween.value(gameObject, transform.position, new Vector3(0, transform.position.y, 0), 1f * Time.deltaTime * 60)
            .setOnUpdate((Vector3 vector) => transform.position = vector);
    }

    private void GameStateChangedCallback(GameState gameState)
    {
        switch(gameState)
        {
            case GameState.COLLECTION:
                EnableMovement();
                break;
            case GameState.BOSS:
                DisableMovement();
                MoveToBoss();
                break;
            case GameState.LEVELCOMPLETE:
                MoveToHome();
                break;
        }
    }

    private void PlayerSizeIncreasedCallback(float playerSize)
    {
        moveSpeed = playerSize * moveSpeedMultiplier + initialMoveSpeed;
    }

    private void EnableMovement()
    {
        canMove = true;
    }

    private void DisableMovement()
    {
        canMove = false;
    }

}
