using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Totomaro.Core;

public class BulletBehaviour : MonoBehaviour
{
    float destroyDelta = 0f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        destroyDelta += Time.deltaTime;

        if (destroyDelta > 5f)
        {
            Destroy(gameObject);
        }
    }
}
