using UnityEngine;

public class RangedFollowPlayer : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform gunTransform;

    [SerializeField] private float offset;

    public float angle;

    private void Awake()
    {
        transform.parent = null;
    }

    private void LateUpdate()
    {
        transform.position = playerTransform.position + gunTransform.right * offset;
    }

    public void SetAngle(float angle)
    {
        this.angle = angle;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
        if (angle < -90 && angle > -270)
        {
            transform.localScale = new Vector3(transform.localScale.x, -1, 1);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, 1, 1);
        }
    }
}