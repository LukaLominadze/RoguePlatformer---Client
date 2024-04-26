using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] Player player;

    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;

    private void Awake()
    {
        currentHp = maxHp;
    }

    private void OnValidate()
    {
        if (player == null) return;

        if (player.IsLocal)
        {
            if (UIManager.Singleton == null) return;

            UIManager.Singleton.SetHP(currentHp / maxHp);
        }
    }
}
