using UnityEngine;

public class AlertIcon : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Hide();
    }

    public void Show()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }
    }

    public void Hide()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
    }
}
