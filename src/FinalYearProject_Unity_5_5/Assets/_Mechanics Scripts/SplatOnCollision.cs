using System.Collections.Generic;
using UnityEngine;

public class SplatOnCollision : MonoBehaviour
{
    public ParticleSystem _particleEmitter; // This is the gun particle system
    public Gradient _particleColorGradient; // Gradient of colors
    public ParticleDecalPool SmallSplatDecalPool; // Decal particle system

    List<ParticleCollisionEvent> _collisionEvents; // Particle Collision Events List

    void Start()
    {
        // Initialise list
        _collisionEvents = new List<ParticleCollisionEvent>();
    }
    /// <summary>
    /// Called when particle collides with other gameobjects
    /// </summary>
    /// <param name="other"></param>
    void OnParticleCollision(GameObject other)
    {
        // Get total number of collision events
        int numCollisionEvents = ParticlePhysicsExtensions.GetCollisionEvents(_particleEmitter, other, _collisionEvents);
        // Reset while loop by setting i to zero
        int i = 0;
        while (i < numCollisionEvents)
        {
            // Create small splatter particles for each collision event
            SmallSplatDecalPool.ParticleHit(_collisionEvents[i], _particleColorGradient);
            // Increment counter variable i for next collision event
            i++;
        }
    }
}
