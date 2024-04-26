using UnityEngine;

public class Username : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] RectTransform rectTransform;

    private void LateUpdate()
    {
        rectTransform.localScale = new Vector3(player.localScale.x, 1, 1);
    }
}
