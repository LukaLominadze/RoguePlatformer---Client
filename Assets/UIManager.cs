using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _singleton;
    public static UIManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
            {
                _singleton = value;
                if (_singleton != value)
                {
                    Destroy(value);
                }
            }
        }
    }

    private void Awake()
    {
        Singleton = this;
    }

    [SerializeField] Image healthBar;
    [SerializeField] Image expBar;
    [SerializeField] Image weaponMelee;
    [SerializeField] Image weaponRanged;
    [SerializeField] Animator meleeAnimator;
    [SerializeField] Animator rangedAnimator;

    const string MELEEACTIVE = "WeaponSlotMelee_Melee";
    const string MELEEINACTIVE = "WeaponSlotMelee_Ranged";
    const string RANGEDACTIVE = "WeaponSlotRanged_Ranged";
    const string RANGEDINACTIVE = "WeaponSlotRanged_Melee";

    public void SetHP(float currentHpDividedByMaxHp)
    {
        healthBar.fillAmount = currentHpDividedByMaxHp;
    }

    public void SetWeaponSlot(Sprite sprite, Weapon weaponId)
    {
        switch (weaponId)
        {
            case Weapon.melee:
                weaponMelee.sprite = sprite;
                break;
            case Weapon.ranged:
                weaponRanged.sprite = sprite;
                break;
        }
    }

    public void SetWeaponSlots(Sprite meleeSprite, Sprite rangedSprite)
    {
        SetWeaponSlot(meleeSprite, Weapon.melee);
        SetWeaponSlot(rangedSprite, Weapon.ranged);
    }

    public void PlaySlotAnimation(Weapon weaponId)
    {
        switch (weaponId)
        {
            case Weapon.melee:
                meleeAnimator.Play(MELEEACTIVE);
                rangedAnimator.Play(RANGEDINACTIVE);
                break;
            case Weapon.ranged:
                meleeAnimator.Play(MELEEINACTIVE);
                rangedAnimator.Play(RANGEDACTIVE);
                break;
        }
    }
}
