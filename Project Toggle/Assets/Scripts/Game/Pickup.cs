using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
  
    [SerializeField]
    GameObject explosionPrefab;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            AudioCtrl.playSFX(gameObject.GetComponent<AudioSource>(), gameObject.GetComponent<AudioSource>().clip, 0.9f);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;

            if (collision.gameObject.transform.position.y < 0)
            {
                GameObject topExplosion = Instantiate(explosionPrefab, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                topExplosion.GetComponent<ParticleSystem>().Play();
            }
            else
            {
                GameObject explosion = Instantiate(explosionPrefab, new Vector2(transform.position.x, transform.position.y - 1f), Quaternion.identity);
                explosion.GetComponent<ParticleSystem>().Play();
            }
            Destroy(gameObject, gameObject.GetComponent<AudioSource>().clip.length);
        }
        if (collision.tag == "Ghost")
        {
            AudioCtrl.playSFX(gameObject.GetComponent<AudioSource>(), gameObject.GetComponent<AudioSource>().clip, 1f);

            if (collision.gameObject.transform.position.y < 0)
            {
                GameObject topExplosion = Instantiate(explosionPrefab, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                topExplosion.GetComponent<ParticleSystem>().Play();
            }
            else
            {
                GameObject explosion = Instantiate(explosionPrefab, new Vector2(transform.position.x, transform.position.y - 1f), Quaternion.identity);
                explosion.GetComponent<ParticleSystem>().Play();
            }
        }
    }

    private bool isOutsideMap()
    {
        return transform.position.x <= -20;
    }
}
