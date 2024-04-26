using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public SpriteRenderer meleeSprite;
    public SpriteRenderer rangedSprite;

    const string WEAPONSLOT = "WeaponSlot";

    public void SetCurrentId(ushort newId)
    {
        if (newId == (ushort)Weapon.melee)
        {
            meleeSprite.enabled = true;
            rangedSprite.enabled = false;
            UIManager.Singleton.PlaySlotAnimation(Weapon.melee);
        }
        else
        {
            meleeSprite.enabled = false;
            rangedSprite.enabled = true;
            UIManager.Singleton.PlaySlotAnimation(Weapon.ranged);
        }
    }
}
