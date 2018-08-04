using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

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

        Destroy(GetComponent<CircleCollider2D>());

        transform.DOScale(2.5f, 0.3f);
        SpriteRenderer spriteRenderer = transform.GetComponent<SpriteRenderer>();
        DOTween.ToAlpha(
            () => spriteRenderer.color,
            a => spriteRenderer.color = a,
            0.0f, 0.3f);

        Destroy(this.gameObject, 0.5f);
    }
}
