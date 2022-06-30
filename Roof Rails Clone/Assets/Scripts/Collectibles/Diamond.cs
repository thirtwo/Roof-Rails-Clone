using UnityEngine;
using DG.Tweening;
public class Diamond : MonoBehaviour
{
    [SerializeField] private Vector3 rotationVector;
    [SerializeField] private float rotationTime = 3f;
    [SerializeField] private float bounceTime = 1.5f;
    void Start()
    {
        transform.DOLocalRotate(rotationVector, rotationTime, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
        transform.DOMoveY(transform.position.y + 0.5f, bounceTime).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InBounce);
    }
}
