using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDecalPool : MonoBehaviour
{
    // Maxmimum number of decals in pool
    public int maxDecals = 100;
    // Minimum size of each decal
    public float decalSizeMin = .5f;
    // Maximum size of each decal
    public float decalSizeMax = 1.5f;
    // Index of the current (available) decal in pool
    private int particleDecalDataIndex;
    // Array of decal objects data
    private ParticleDecalData[] particleData;
    // Array of decal particles
    private ParticleSystem.Particle[] particles;
    // The particle system that contains all decal pool particles
    private ParticleSystem decalParticleSystem;

    // Use this for initialization
    void Start()
    {
        decalParticleSystem = GetComponent<ParticleSystem>();
        // Initialise the array of decal objects 
        particleData = new ParticleDecalData[maxDecals];
        // Initialise the array of decal particles
        particles = new ParticleSystem.Particle[maxDecals];
        for (int i = 0; i < maxDecals; i++)
        {
            // Populate the array of decal objects
            particleData[i] = new ParticleDecalData();
        }
    }
    /// <summary>
    /// Displays decal particles with position, rotation, size and color using collision data and gradient color
    /// </summary>
    /// <param name="particleCollisionEvent"></param>
    /// <param name="colorGradient"></param>
    public void ParticleHit(ParticleCollisionEvent particleCollisionEvent, Gradient colorGradient)
    {
        SetParticleData(particleCollisionEvent, colorGradient);
        DisplayParticles();
    }
    /// <summary>
    /// Set position, rotation, size and color attributes of each decal object using collision data and gradient color
    /// </summary>
    /// <param name="particleCollisionEvent"></param>
    /// <param name="colorGradient"></param>
    void SetParticleData(ParticleCollisionEvent particleCollisionEvent, Gradient colorGradient)
    {
        // To loop back in decal particles pool by resetting current decal particle index
        if(particleDecalDataIndex >= maxDecals)
        {
            particleDecalDataIndex = 0;
        }

        // store position, rotation, size and color from each collision event
        particleData[particleDecalDataIndex].position = particleCollisionEvent.intersection;
        Vector3 particleRotationEuler = Quaternion.LookRotation(particleCollisionEvent.normal).eulerAngles;
        particleRotationEuler.z = Random.Range(0, 360);
        particleData[particleDecalDataIndex].rotation = particleRotationEuler;
        particleData[particleDecalDataIndex].size = Random.Range(decalSizeMin, decalSizeMax);
        particleData[particleDecalDataIndex].color = colorGradient.Evaluate(Random.Range(0f, 1f));

        // Increment decal particle index for next collision event
        particleDecalDataIndex++;
    }
    /// <summary>
    /// Set the properties of each decal particle using decal object data 
    /// </summary>
    void DisplayParticles()
    {
        // Set position, rotation, size and color  of each particle using decal data stored in particleData
        for (int i = 0; i < particleData.Length; i++)
        {
            particles[i].position = particleData[i].position;
            particles[i].rotation3D = particleData[i].rotation;
            particles[i].startSize = particleData[i].size;
            particles[i].startColor = particleData[i].color;
        }
        // Set the particles in the decal particle system
        decalParticleSystem.SetParticles(particles, particles.Length);
    }
}
