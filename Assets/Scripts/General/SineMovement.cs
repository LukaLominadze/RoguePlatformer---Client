using UnityEngine;

public class SineMovement : MonoBehaviour
{
    [SerializeField] private float _hoverSpeed; // Frequency of the sine wave
    [SerializeField] private float _maxHoverOffset; // Maximum offset in height

    private float _originalHeight; // Base height to which offset is added (typically the spawned or dropped height)
    private float _elapsedTime; // Accumulated time passed (multiplied by speed in order to get proper result)

    private void Start()
    {
        _originalHeight = transform.position.y;
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime * _hoverSpeed;
        float newHeight = _originalHeight + Mathf.Sin(_elapsedTime) * _maxHoverOffset;
        transform.position = new Vector2(transform.position.x, newHeight);
    }
}
