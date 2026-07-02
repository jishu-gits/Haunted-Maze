using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Gem : MonoBehaviour
{
    public Mesh[] allGems;
    public Color[] allColors;
    public AudioClip pickUpSfx;

    private bool isCollected;

    private void Start()
    {
        if (allGems?.Length > 0 && TryGetComponent<MeshFilter>(out var meshFilter))
        {
            meshFilter.mesh = allGems[Random.Range(0, allGems.Length)];
        }

        if (allColors?.Length > 0 && TryGetComponent<MeshRenderer>(out var meshRenderer))
        {
            meshRenderer.material.color = allColors[Random.Range(0, allColors.Length)];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCollected || !other.CompareTag("Player")) return;

        isCollected = true;

#if UNITY_EDITOR
        if (Selection.activeGameObject == gameObject) Selection.activeObject = null;
#endif

        if (pickUpSfx) AudioManager.instance.PlaySFX(pickUpSfx);

        MapManager.instance.GemPickedUp();
        gameObject.SetActive(false);
    }
}
