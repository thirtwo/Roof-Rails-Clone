using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region Enums
    public enum FollowType
    {
        horizontalOnly,
        fullFollow
    }
    public enum OffsetType
    {
        add_On,
        self,
    }
    #endregion
    [Header("Camera Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private FollowType followType;
    [Header("Offset Settings")]
    [SerializeField] private OffsetType offsetType;
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;
    [SerializeField] private float offsetZ;
    private Vector3 offset;
    private Vector3 desiredCameraPos;
    [Header("Follow Settings")]
    [SerializeField] private float smoothness = 1;
    [SerializeField] private float snapOffset = 0.25f;
    private void Start()
    {
        switch (offsetType)
        {
            case OffsetType.self:
                offset = transform.position = new Vector3(target.position.x + offsetX,
                    target.position.y + offsetY, target.position.z);
                break;
            case OffsetType.add_On:
                transform.position += new Vector3(offsetX, offsetY, offsetZ);
                offset = transform.position - target.position;
                break;
            default:
                break;
        }
    }

    private void LateUpdate()
    {
        GetDesiredCameraPosition();
        if (Vector3.Distance(transform.position, desiredCameraPos) >= snapOffset)
        {
            transform.position = Vector3.Lerp(transform.position, desiredCameraPos, smoothness * Time.deltaTime);
        }
        else
        {
            transform.position = desiredCameraPos;
        }
    }

    private void GetDesiredCameraPosition()
    {
        desiredCameraPos = offset + target.position;
    }
}
