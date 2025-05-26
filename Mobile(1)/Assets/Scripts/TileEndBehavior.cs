using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEndBehavior : MonoBehaviour
{

    public float destroyTime = 1.5f;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerBehavior>())
        {
            var gm = GameObject.FindObjectOfType<GameManager>();
            gm.SpawnNextTile();

            Destroy(transform.parent.gameObject, destroyTime);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
