using UnityEngine;
using EzySlice;
using System.Collections.Generic;

public class CylinderController : MonoBehaviour
{
    [SerializeField] private Material cylinderMaterial;
    [SerializeField] private GameObject myCylinder;
    [SerializeField] private float extensionRate = 0.5f;
    private int barCount = 0;
    private bool isSlipping = false;
    private float slipTimer = 0;
    [SerializeField] private GameObject collectParticle;
    [SerializeField] private GameObject sparkleParticle;
    private Dictionary<string, GameObject> particleDictionary = new Dictionary<string, GameObject>();
    private void Update()
    {
        if (!isSlipping) return;
        if (myCylinder == null) return;
        if (Time.time > slipTimer + 0.2f && barCount == 1)
        {
            myCylinder.AddComponent<Rigidbody>().useGravity = true;
            isSlipping = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6) //saw layer
        {
            var direction = collision.transform.position.x < transform.position.x ? Vector3.right : Vector3.left;
            var slicedPart = myCylinder.SliceInstantiate(collision.collider.ClosestPoint(myCylinder.transform.position), direction, cylinderMaterial);
            if (slicedPart != null && slicedPart.Length > 0)
            {
                slicedPart[0].AddComponent<CapsuleCollider>();
                slicedPart[0].transform.SetParent(transform);
                slicedPart[0].transform.position += transform.position;
                slicedPart[1].AddComponent<CapsuleCollider>();
                slicedPart[1].AddComponent<Rigidbody>();
                slicedPart[1].transform.position += transform.position;
                myCylinder.SetActive(false);
                myCylinder = slicedPart[0];
            }
        }
        if (collision.gameObject.layer == 7) //collectible cylinder layer
        {
            if (!collision.collider.TryGetComponent<CollectibleCylinder>(out var collectible)) return;
            myCylinder.transform.localScale += new Vector3(0, collectible.collectValue * extensionRate, 0);
            collision.gameObject.SetActive(false);
        }
        if (collision.gameObject.layer == 8) //diamond layer
        {
            UIManager.Instance.SpawnDiamond(collision.transform.position);
            collision.gameObject.SetActive(false);
            GameManager.Money++;
            Instantiate(collectParticle, transform.position, Quaternion.identity);
        }
        if (collision.gameObject.layer == 9) // bar layer
        {
            var particle = Instantiate(sparkleParticle,
                collision.collider.ClosestPoint(myCylinder.transform.position),
                Quaternion.Euler(new Vector3(0, 180, 0)));
            if (!particleDictionary.ContainsKey(collision.gameObject.name))
                particleDictionary.Add(collision.gameObject.name, particle);
            barCount++;
            isSlipping = true;
            slipTimer = Time.time;
        }
        if (collision.gameObject.layer == 10) //deadzone layer
        {
            GameManager.FinishGame(true);
        }
        if (collision.gameObject.layer == 11) // finish layer
        {
            var finish = collision.collider.GetComponent<FinishBlock>();
            if (finish != null)
            {
                var playerController = GetComponent<PlayerController>();
                playerController.IsControllerActive = false;
                finish.AnimateBlock();
            }
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer != 9) return;
        if (particleDictionary.TryGetValue(collision.gameObject.name, out var particle))
        {
            particle.transform.position = collision.collider.ClosestPoint(myCylinder.transform.position);
        }

    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 9) // bar layer
        {
            barCount--;
            slipTimer = Time.time;
            if (barCount == 0)
                isSlipping = false;
            if (particleDictionary.TryGetValue(collision.gameObject.name, out var particle))
            {
                particle.SetActive(false);
            }
            particleDictionary.Remove(collision.gameObject.name);
        }
    }
}
