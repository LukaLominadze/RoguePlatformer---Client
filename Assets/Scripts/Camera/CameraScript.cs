using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] Transform player;

    private void Start()
    {
        transform.SetParent(null);
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(player.position.x, player.position.y + 3.44f, -10);
    }
}
