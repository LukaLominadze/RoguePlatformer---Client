using UnityEngine;

public class SetParentNull : MonoBehaviour
{
    private void Start()
    {
        transform.SetParent(null);
    }
}
