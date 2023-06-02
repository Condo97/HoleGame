using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInFromSidesAnimationController : MonoBehaviour
{

    [System.Serializable]
    public class MoveInFromSidesObject
    {
        public enum MoveInDirection { LEFT, TOP, RIGHT, BOTTOM };

        public GameObject gameObject;
        public float secondsDelay;
        public MoveInDirection moveInDirection;
        [System.NonSerialized] public Vector3? initialPosition;
    }

    [Header(" Elements ")]
    [SerializeField] private List<MoveInFromSidesObject> objectsToMoveInFromSides;

    [Header(" Settings ")]
    [SerializeField] private float moveOffset;
    [SerializeField] private float animationDuration;


    public void MoveObjectsOut()
    {
        foreach (MoveInFromSidesObject objectToMoveInFromSides in objectsToMoveInFromSides)
        {
            SetInitialPosition(objectToMoveInFromSides);

            Vector3 targetPosition = objectToMoveInFromSides.gameObject.transform.position;
            float heightRatio = (float)Screen.height / (float)Screen.width;

            switch (objectToMoveInFromSides.moveInDirection)
            {
                case MoveInFromSidesObject.MoveInDirection.LEFT:
                    targetPosition.x -= moveOffset / heightRatio;
                    break;
                case MoveInFromSidesObject.MoveInDirection.BOTTOM:
                    targetPosition.y -= moveOffset * heightRatio;
                    break;
                case MoveInFromSidesObject.MoveInDirection.RIGHT:
                    targetPosition.x += moveOffset / heightRatio;
                    break;
                case MoveInFromSidesObject.MoveInDirection.TOP:
                    targetPosition.y += moveOffset * heightRatio;
                    break;
            }

            objectToMoveInFromSides.gameObject.transform.position = targetPosition;
        }
    }

    public void MoveObjectsIn(bool animated)
    {
        foreach (MoveInFromSidesObject objectToMoveInFromSides in objectsToMoveInFromSides)
        {
            if (objectToMoveInFromSides.initialPosition != null)
            {
                Vector3 targetPosition = (Vector3)objectToMoveInFromSides.initialPosition;
                float animationDuration = animated ? this.animationDuration : 0f;

                //LeanTween.delayedCall(objectToMoveInFromSides.secondsDelay, () =>
                //{
                    LeanTween.value(objectToMoveInFromSides.gameObject, objectToMoveInFromSides.gameObject.transform.position, targetPosition, animationDuration * Time.deltaTime * 60)
                        .setOnUpdate((Vector3 value) => objectToMoveInFromSides.gameObject.transform.position = value)
                        .setEase(LeanTweenType.easeInOutExpo);
                //});
            }
        }
    }

    private void SetInitialPosition(MoveInFromSidesObject moveInFromSidesObject)
    {
        if (moveInFromSidesObject.initialPosition == null)
            moveInFromSidesObject.initialPosition = moveInFromSidesObject.gameObject.transform.position;
    }


}
