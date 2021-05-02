using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundService : MonoBehaviour
{
    public AudioClip Explode;
    public AudioClip ReceiveDamage;
    public AudioClip Jump;
    public AudioClip CassouletPresentation;
    public AudioClip CassouletDie;

    public GameObject SoundHolder;
    public GameObject PrefabSound;

    private void _playSound(AudioClip clip)
    {
        var go = GameObject.Instantiate(PrefabSound, SoundHolder.transform);
        var audioSource = go.GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
        StartCoroutine(_handleDestroy(go, clip.length));
    }

    IEnumerator _handleDestroy(GameObject go, float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(go);
    }
    public void PlayExplode() // kill an enemy
    {
        _playSound(Explode);
    }
    public void PlayErg() // receive Damage
    {
        _playSound(Explode);
    }

    public void PlayJump()
    {
        _playSound(Jump);
    }

    public void PlayCassoulet()
    {
        _playSound(CassouletPresentation);
    }
    public void PlayCassouletDie()
    {
        _playSound(CassouletDie);
    }
}
