using System.Collections;
using UnityEngine;

public class FinishBlock : MonoBehaviour
{
    [SerializeField] private int scoreMultiplier;
    private Color objColor;
    MeshRenderer objRenderer;
    MaterialPropertyBlock _mpb;
    private void Start()
    {
        objRenderer = GetComponent<MeshRenderer>();
        _mpb = new MaterialPropertyBlock();
        objColor = objRenderer.material.color;
    }
    public void AnimateBlock()
    {
        StartCoroutine(Co_AnimateBlock());
    }
    private IEnumerator Co_AnimateBlock()
    {
        var wait = new WaitForSeconds(0.2f);
        for (int i = 0; i < 4; i++)
        {
            _mpb.SetColor("_Color", Color.white);
            objRenderer.SetPropertyBlock(_mpb);
            yield return wait;
            _mpb.SetColor("_Color", objColor);
            objRenderer.SetPropertyBlock(_mpb);
            yield return wait;
        }
        GameManager.FinishGame(true);
    }
}
