using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerCollisionDetection : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;
    //[SerializeField] Tilemap tilemap;

    [SerializeField] private float linecastLenght;

    private bool onGround;

    //private void Awake()
    //{
    //    TileBase tile = tilemap.GetTile(tilemap.WorldToCell(new Vector3(0, -5, 0)));
    //    print(tile);
    //}

    public bool OnGround()
    {
        onGround = Physics2D.Linecast(transform.position, (Vector2)transform.position - Vector2.up * linecastLenght, groundLayer);
        return onGround;
    }
}
