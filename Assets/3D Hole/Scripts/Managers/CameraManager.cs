using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{

    [Header(" Boss ")]
    private Vector3 bossCameraOffset = new Vector3(0f, 1f, -20f);
    private Vector3 bossTrackedObjectOffset = new Vector3(0f, 8f, 0f);    

    [Header(" Elements ")]
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private GameObject areaToPanOnZ;
    private float playerSize = 1;

    [Header(" Settings ")]
    [SerializeField] private float minDistance;
    [SerializeField] private float distanceMultiplier;
    [SerializeField] private float zPanRange;
    [SerializeField] private float defaultYFollowOffset;
    [SerializeField] private float defaultZFollowOffset;
    private bool tweenActive = false;
    private GameState gameState;


    private void Awake()
    {
        // Subscribe to onDiameterIncrease and onStateChanged
        //LayerSwitchSize.onDiameterIncrease += PlayerSizeIncreased;
        HoleParentSize.onIncrease += PlayerSizeIncreased;
        GameManager.onStateChanged += GameStateChangedCallback;
    }

    private void OnDestroy()
    {
        // Unsubscribe from onDiameterIncrease and onStateChanged
        //LayerSwitchSize.onDiameterIncrease -= PlayerSizeIncreased;
        HoleParentSize.onIncrease -= PlayerSizeIncreased;
        GameManager.onStateChanged -= GameStateChangedCallback;
    }

    // Update is called once per frame
    void Update()
    {
        // Move the camera for every state but boss
        if (gameState != GameState.BOSS)
        {
            // Set the playerCamera z to a place in the z pan range if not tweenActive to smooth out camera transitions
            if (!tweenActive) {
                float distance = minDistance + (playerSize - 1) * distanceMultiplier;
                Vector3 targetCameraOffset = new Vector3(0, distance/* * (defaultYFollowOffset / GetZMultiplierFromRange()) */* (defaultYFollowOffset / -defaultZFollowOffset), -distance);

                LeanTween.value(gameObject, GetFollowOffset(), targetCameraOffset, 0.2f * Time.deltaTime * 60)
                    .setOnUpdate((Vector3 offset) => playerCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = offset)
                    .setEase(LeanTweenType.easeInOutSine)
                    .setOnComplete(() => tweenActive = false);

                tweenActive = true;
            }
        }

        //playerCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = targetCameraOffset;
    }

    private float GetZMultiplierFromRange()
    {
        // Calculate Z Multiplier TODO: This can probably be simplified
        float cameraZ = playerCamera.transform.position.z;
        float floorZSize = areaToPanOnZ.GetComponent<Collider>().bounds.size.z;
        float floorMinZ = areaToPanOnZ.transform.position.z - floorZSize / 2;
        float cameraZMax = defaultZFollowOffset + zPanRange / 2;

        float normalizedFloorPosition = (((cameraZ - floorMinZ) / floorZSize));

        //Debug.Log(cameraZ + " " + floorZSize + " " + floorMinZ + " " + cameraZMax + " " + normalizedFloorPosition);

        return normalizedFloorPosition * zPanRange + cameraZMax;
    }

    private Vector3 GetFollowOffset()
    {
        return playerCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
    }

    private Vector3 GetTrackedObjectOffset()
    {
        return playerCamera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset;
    }

    private void PlayerSizeIncreased(float playerSize)
    {
        this.playerSize = playerSize;
    }

    private void GameStateChangedCallback(GameState gameState)
    {
        this.gameState = gameState;

        // Change camera offset and tracked object offset if if switched to boss state
        if (gameState == GameState.BOSS)
        {
            // Set camera offset
            LeanTween.value(gameObject, GetFollowOffset(), bossCameraOffset, 1f * Time.deltaTime * 60)
                .setOnUpdate((Vector3 vector) => playerCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = vector)
                .setEase(LeanTweenType.easeInOutSine);

            // Set tracked object offset
            LeanTween.value(gameObject, GetTrackedObjectOffset(), bossTrackedObjectOffset, 1f * Time.deltaTime * 60)
                .setOnUpdate((Vector3 vector) => playerCamera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset = vector)
                .setEase(LeanTweenType.easeInOutSine);
        }
    }

}
