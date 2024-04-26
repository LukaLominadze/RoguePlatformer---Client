using UnityEngine;

public class WeaponSlotController : MonoBehaviour
{
    [SerializeField] Player player;

    private void Awake()
    {
        UIManager.Singleton.SetWeaponSlots(player.meleeScript.gameObject.GetComponent<SpriteRenderer>().sprite,
                              player.gunScript.gameObject.GetComponent<SpriteRenderer>().sprite);
    }
}
