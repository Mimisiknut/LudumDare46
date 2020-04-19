﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyCatcher : MonoBehaviour
{
    [SerializeField] Transform positionCarry;
    [SerializeField] float angleCarry;
    [SerializeField] bool autoReleaseTest;

    public bool isCarrying { get; set; }

    private GameObject baby;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"BABY CATCHER ENTER {collision.gameObject} {collision.tag}");
        if(collision.tag == "baby")
        {
            var tg = collision.gameObject.GetComponent<TouchGround>();
            if (tg.IsGameOver || tg.IsCarried) return;

            baby = collision.gameObject;
            // Debug.Log($"{transform.parent.name} Carry {baby}");
            collision.gameObject.transform.SetParent(positionCarry);
            collision.gameObject.transform.localPosition = Vector3.zero;
            collision.gameObject.transform.localRotation = Quaternion.Euler(0, 0, angleCarry);
            collision.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

            tg.Carry();
            isCarrying = true;

            // For testing
            if(autoReleaseTest) StartCoroutine(AutoRelease());
        }
    }

    public void Release(Vector2 velocity)
    {
        Debug.Log($"{transform.parent.name} Release");
        if (baby == null) { Debug.LogWarning("Already Released"); return;  }
        baby.transform.SetParent(null);
        baby.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        baby.GetComponent<Rigidbody2D>().velocity = velocity;
        isCarrying = false;
        baby.GetComponent<TouchGround>().Fire();
        baby = null;
    }

    void Update()
    {
        if (baby != null) baby.transform.localPosition = Vector3.zero;
    }

    IEnumerator AutoRelease()
    {
        yield return new WaitForSeconds(2);

        Release(new Vector2(0, 5));
    }
}
