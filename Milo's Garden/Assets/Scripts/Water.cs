using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
	private ParticleSystem water;
	private List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();

	private void Start()
	{
		water = GetComponent<ParticleSystem>();
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
	}
}
