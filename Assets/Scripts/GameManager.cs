using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
  public List<GameObject> buckets;
  public List<GameObject> items;
  public List<Material> colors;
  public List<Material> usedColors;
  private GameObject item1;
  private GameObject item2;
  private GameObject item3;

  public GameObject itemBackgroundPlain;
  public GameObject bucketBackgroundPlain;

  private float xSpawnPoint = 6;
  private float ySpawnPoint = 2.5f;

  public bool gameOver;
  public int destroyedItems = 0;

  public ParticleSystem explosionParticle;
  public Button restartButton;

  public GameObject SpawnItem(float xValue)
  {
    int itemIndex = Random.Range(0, items.Count);
    GameObject item = Instantiate(items[itemIndex]);
    items.RemoveAt(itemIndex);
    item.transform.position = new Vector2(xValue, ySpawnPoint);

    return item;
  }

  private Material GenarateBackgroundMaterial(GameObject item) {
    GameObject itemBackground = Instantiate(itemBackgroundPlain);
    itemBackground.transform.SetParent(item.transform);
    itemBackground.transform.position = item.transform.position;
    SpriteRenderer itemBackgroundRenderer = itemBackground.GetComponent<SpriteRenderer>();
    int colorIndex = Random.Range(0, colors.Count);
    itemBackgroundRenderer.material = colors[colorIndex];

    var itemScript = item.GetComponent<Item>();
    itemScript.color = colors[colorIndex].name;

    colors.RemoveAt(colorIndex);

    return itemBackgroundRenderer.material;
  }

  void GenarateBucketBackgroundMaterial(GameObject bucket)
  {
    GameObject bucketBackground = Instantiate(bucketBackgroundPlain);
    bucketBackground.transform.SetParent(bucket.transform);
    bucketBackground.transform.position = bucket.transform.position - new Vector3(0, 1.5f, 0);
    SpriteRenderer itemBucketBackgroundRenderer = bucketBackground.GetComponent<SpriteRenderer>();

    int colorIndex = Random.Range(0, usedColors.Count);
    itemBucketBackgroundRenderer.material = usedColors[colorIndex];
    var bucketScript = bucket.GetComponent<Bucket>();
    bucketScript.color = usedColors[colorIndex].name;

    usedColors.RemoveAt(colorIndex);
  }

  public void StartGame()
  {
    // this is an example of abstraction
    item1 = SpawnItem(-xSpawnPoint);
    usedColors.Add(GenarateBackgroundMaterial(item1));
    item2 = SpawnItem(0);
    usedColors.Add(GenarateBackgroundMaterial(item2));
    item3 = SpawnItem(xSpawnPoint);
    usedColors.Add(GenarateBackgroundMaterial(item3));

    foreach (GameObject bucket in buckets)
    {
      GenarateBucketBackgroundMaterial(bucket);
    }

    //this is handling of main menu - it is enabled at start and hidden after starting a game
    GameObject.Find("StartScreen").SetActive(false);

  }

  public void GameOver()
  {
    gameOver = true;
    restartButton.gameObject.SetActive(true);
    Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
  }

  public void RestartGame()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }
}
