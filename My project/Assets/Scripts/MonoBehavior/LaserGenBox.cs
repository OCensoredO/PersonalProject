using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGenBox : MonoBehaviour
{
    LaserGenerator LGen;

    private void Start()
    {
        LGen = GetComponentInParent<LaserGenerator>();
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Bullet"))
        {
            Destroy(this);
            LGen.UpdateGeneratorCondition();
        }
    }
    */

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Bullet"))
        {
            Debug.Log("asdf");
            Destroy(this.gameObject);
            LGen.UpdateGeneratorCondition();
        }
    }
}
