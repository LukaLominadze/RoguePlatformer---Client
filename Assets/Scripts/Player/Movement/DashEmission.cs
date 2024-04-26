using UnityEngine;

public class DashEmission : MonoBehaviour
{
    [SerializeField] TrailRenderer trail;
    [SerializeField] GameObject dashLight;

    public void EmittingTrail(bool isDashing)
    {
        if (isDashing)
        {
            trail.emitting = true;
            dashLight.SetActive(true);
        }
        else
        {
            trail.emitting = false;
            dashLight.SetActive(false);
        }
    }
}
