using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] RangedFollowPlayer gunParent;
    [SerializeField] GameObject bulletPrefab;

    [SerializeField] private string weaponName;
    [SerializeField] private string idleAnimation;
    [SerializeField] private string attackAnimation;
    [SerializeField] private float attackTime;

    public ushort nameId;

    private void Awake()
    {
        nameId = CharValues.GetValueOfString(weaponName);
    }

    public void Attack()
    {
        animator.Play(attackAnimation);
        Invoke("AttackEnd", attackTime);

        if (gameObject.tag == "Ranged")
        {
            Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, gunParent.angle));
        }
    }

    private void AttackEnd()
    {
        animator.Play(idleAnimation);
    }
}