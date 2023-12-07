using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXTest : MonoBehaviour
{
    public ParticleSystem pSys;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) boom();
    }

    void boom()
    {
        Instantiate(pSys, transform.position, transform.rotation);
    }
}
