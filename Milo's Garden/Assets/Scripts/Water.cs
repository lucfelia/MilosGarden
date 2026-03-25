using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Water : MonoBehaviour
{
	public AudioClip[] sounds; 
	private AudioSource audioSource;
    private ParticleSystem water;
	private List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();

	private void Start()
	{
		water = GetComponent<ParticleSystem>();
		audioSource = GetComponent<AudioSource>();

    }

	private void OnParticleTrigger()
	{
		int numberOfParticlesEntered = water.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, particles);
		if (numberOfParticlesEntered <= 0)
		{
			GameManager.Manager.isInteractionSucceed = false;
			return;
		}
		GameManager.Manager.isInteractionSucceed = true;
        PlayRandomSound();
    }
    private float lastPlayTime;
    public float cooldown = 0.1f;

    private void PlayRandomSound()
    {
        if (Time.time - lastPlayTime < cooldown)
            return;

        if (sounds == null || sounds.Length == 0 || audioSource == null)
            return;

        lastPlayTime = Time.time;

        int randomIndex = Random.Range(0, sounds.Length);
        audioSource.PlayOneShot(sounds[randomIndex]);
    }
}
