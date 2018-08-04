using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbManager : MonoBehaviour
{
    private const int ORB_POINT = 100;

    private GameObject gameManager;

    // Use this for initialization
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetOrb()
    {
        gameManager.GetComponent<GameManager>().AddScore(ORB_POINT);
        Destroy(this.gameObject);
    }
}
