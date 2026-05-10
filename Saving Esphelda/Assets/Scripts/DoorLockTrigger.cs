using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class DoorLockTrigger : MonoBehaviour
{
    [Header("Esphelda Activation")]
    [Tooltip("Optional Esphelda GameObject to show when the player hits the door lock.")]
    public GameObject espheldaObject;

    [Tooltip("Tag of the Esphelda GameObject to find if no object is assigned.")]
    public string espheldaTag = "Esphelda";

    [Tooltip("The tilemap layer GameObject to disable when Esphelda appears.")]
    public GameObject tilemapLayerToDisable;
    public GameObject doorLockObject;

    [Header("Win Sequence")]
    [Tooltip("The number of keys the player must have before the door lock can trigger.")]
    public int requiredKeys = 3;

    [Tooltip("Seconds to wait after Esphelda appears before starting the jump.")]
    public float appearDelay = 1.7f;

    [Tooltip("Jump velocity applied to Esphelda when the sequence begins.")]
    public float jumpVelocity = 5f;

    [Tooltip("Seconds to wait after the jump before loading the win scene.")]
    public float sceneTransitionDelay = 1.2f;

    [Tooltip("Win scene name to load after the Esphelda sequence.")]
    public string winScene = "Win_Scene";

    private bool triggered;

    private void Awake()
    {
        if (!GetComponent<Collider2D>().isTrigger)
        {
            Debug.LogWarning("DoorLockTrigger requires a trigger collider. Mark the collider as trigger.", this);
        }

        if (espheldaObject == null && !string.IsNullOrEmpty(espheldaTag))
        {
            espheldaObject = FindInactiveObjectWithTag(espheldaTag);
        }

        if (espheldaObject != null)
        {
            espheldaObject.SetActive(false);
        }
    }

    private GameObject FindInactiveObjectWithTag(string tag)
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag(tag))
            {
                return obj;
            }
        }

        return null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered)
            return;

        if (!other.CompareTag("Player"))
            return;

        PlayerInventory inventory = other.GetComponent<PlayerInventory>();
        if (inventory == null)
        {
            Debug.LogWarning("DoorLockTrigger: Player does not have a PlayerInventory component.", other.gameObject);
            return;
        }

        if (inventory.KeyCount < requiredKeys)
        {
            Debug.Log($"DoorLockTrigger: Player needs {requiredKeys} keys but has {inventory.KeyCount}.");
            return;
        }

        triggered = true;
        StartCoroutine(WinSequenceCoroutine(other.gameObject));
    }

    private IEnumerator WinSequenceCoroutine(GameObject spike)
    {
        if (espheldaObject == null)
        {
            Debug.LogError("DoorLockTrigger: Esphelda object is not assigned and could not be found.");
            yield break;
        }

        espheldaObject.SetActive(true);

        if (tilemapLayerToDisable != null)
        {
            tilemapLayerToDisable.SetActive(false);
        }

        if (doorLockObject != null)
        {
            SetAlpha(doorLockObject, 0f);
        }

        yield return new WaitForSeconds(appearDelay);

        JumpGameObject(espheldaObject);
        JumpGameObject(spike);

        yield return new WaitForSeconds(sceneTransitionDelay);

        if (!string.IsNullOrEmpty(winScene))
        {
            SceneManager.LoadScene(winScene);
        }
    }

    private void SetAlpha(GameObject target, float alpha)
    {
        Renderer[] renderers = target.GetComponentsInChildren<Renderer>(true);

        foreach (Renderer renderer in renderers)
        {
            foreach (Material material in renderer.materials)
            {
                Color color = material.color;
                color.a = alpha;
                material.color = color;
            }
        }
    }

    private void JumpGameObject(GameObject targetObject)
    {
        if (targetObject == null)
            return;

        Rigidbody2D rb = targetObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity);
        }
        else
        {
            StartCoroutine(AnimateJump(targetObject.transform));
        }
    }


    private IEnumerator AnimateJump(Transform target)
    {
        float duration = 0.4f;
        float elapsed = 0f;
        Vector3 startPosition = target.position;
        Vector3 peakPosition = startPosition + Vector3.up * (jumpVelocity * 0.25f);

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float height = Mathf.Sin(t * Mathf.PI) * (jumpVelocity * 0.25f);
            target.position = startPosition + Vector3.up * height;
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.position = startPosition;
    }
}
