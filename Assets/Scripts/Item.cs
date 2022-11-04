using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
  private CircleCollider2D itemCollider;
  private Rigidbody2D itemRB;
  private Renderer itemRenderer;
  private GameObject background;
  public GameManager gameManager;
  private Renderer itemBackgroundRenderer;
  private Vector3 defaultPosition;
  public bool dragged;
  public bool hoverOnBucket;
  public string color;

  // Start is called before the first frame update
  void Start()
    {
      gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
      itemCollider = GetComponent<CircleCollider2D>();
      defaultPosition = transform.position;
      itemRenderer = GetComponent<Renderer>();
      background = transform.GetChild(0).gameObject;
      itemBackgroundRenderer = transform.GetChild(0).gameObject.GetComponent<Renderer>();
  }


  // Update is called once per frame
  void Update()
  {
    if (Input.GetMouseButtonUp(0))
    {
      dragged = false;
      itemRenderer.sortingLayerName = "default";
      itemBackgroundRenderer.sortingLayerName = "default";
    }
  }

  private void LateUpdate()
  {
    if (!dragged && !hoverOnBucket) { transform.position = defaultPosition; }
  }

  private void OnMouseDrag()
  {
    dragged = true;
    transform.position = GetMousePosition();
    itemRenderer.sortingLayerName = "dragged";
    itemBackgroundRenderer.sortingLayerName = "dragged";
  }

  private Vector3 GetMousePosition()
  {
    var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    mousePos.z = 0;
    return mousePos;
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.CompareTag("item") && dragged)
    {
      itemCollider.enabled = false;
    }

    if (collision.gameObject.CompareTag("bucket") && dragged)
    {
      hoverOnBucket = true;
    }
  }

  private void OnTriggerStay2D(Collider2D collision)
  {
    if (collision.gameObject.CompareTag("bucket") && !dragged)
    {
      var bucket = collision.GetComponent<Bucket>();
      if (bucket.color.Contains(color)) { 
        Destroy(gameObject);
        gameManager.destroyedItems++;
        if (gameManager.destroyedItems == 3) { gameManager.GameOver(); }
      }
      else { transform.position = defaultPosition; }    
    }

  }

  private void OnTriggerExit2D(Collider2D collision)
  {
    if (collision.gameObject.CompareTag("item") && dragged)
    {
      itemCollider.enabled = true;
    }

    if (collision.gameObject.CompareTag("bucket") && dragged)
    {
      hoverOnBucket = false;
    }
  }

  public virtual void SetColor(string generatedColor) {
    color = generatedColor;
  }

}
