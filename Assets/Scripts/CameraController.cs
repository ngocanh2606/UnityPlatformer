using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player, firstBackground, secondBackground, thirdBackground, fourthBackground;
    private Vector2 lastPosition;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private SpriteRenderer backgroundRenderer;

    void Start()
    {
        lastPosition = transform.position;

        float widthToBeSeen = backgroundRenderer.bounds.size.x;
        mainCamera.orthographicSize = widthToBeSeen * Screen.height / Screen.width * 0.5f; //camera fit to background width
    }

    private void Update()
    {
        transform.position = new Vector3(player.position.x +2, player.position.y, transform.position.z);
        Vector2 amountToMove = new Vector2(transform.position.x - lastPosition.x, transform.position.y - lastPosition.y);
        firstBackground.position = firstBackground.position + new Vector3(amountToMove.x, amountToMove.y, 0);
        secondBackground.position = secondBackground.position + new Vector3(amountToMove.x, amountToMove.y, 0) * 0.7f;
        thirdBackground.position = thirdBackground.position + new Vector3(amountToMove.x, amountToMove.y, 0) * 0.5f;
        fourthBackground.position = fourthBackground.position + new Vector3(amountToMove.x, amountToMove.y, 0) * 0.2f;
        lastPosition = transform.position;
    }
}
