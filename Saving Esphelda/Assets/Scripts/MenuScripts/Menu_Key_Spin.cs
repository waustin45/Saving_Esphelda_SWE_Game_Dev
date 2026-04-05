using UnityEngine;
using System.Collections;

public class Menu_Key_Spin : MonoBehaviour
{
    [Header("Spin Settings")]
    public float spinSpeed = 90f;

    [Header("Bounce Settings")]
    public float bounceHeight = 20f;
    public float bounceSpeed = 2f;

    private RectTransform rectTransform;
    private Vector3 startLocalPosition;
    private bool ready = false;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        StartCoroutine(CaptureStartPosition());
    }

    IEnumerator CaptureStartPosition()
    {
        // Wait two frames for layout to fully settle
        yield return null;
        yield return null;

        startLocalPosition = rectTransform.localPosition;
        ready = true;
    }

    void Update()
    {
        if (!ready) return;

        // Spin on Z axis for 2D UI
        rectTransform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);

        // Bounce around the true start position
        float newY = startLocalPosition.y + Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        rectTransform.localPosition = new Vector3(startLocalPosition.x, newY, startLocalPosition.z);
    }
}