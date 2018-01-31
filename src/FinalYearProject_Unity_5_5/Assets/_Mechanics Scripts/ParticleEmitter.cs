using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Following documentation in the Unity website was used to implement this script:
/// - To detect particle collisions with other gameObjects in the scene:
/// https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnParticleCollision.html
/// - To get the layer of gameObjects struck by particles in decimal numbers (base 10) - 0,1,2..10
/// https://docs.unity3d.com/ScriptReference/GameObject-layer.html
/// - To check layermask of gameObject struck by particle against the custom collision layermask (_collisionMask)
/// https://docs.unity3d.com/Manual/Layers.html
/// </summary>
public class ParticleEmitter : MonoBehaviour
{
    // LayerMask used for checking target collision
    public LayerMask _collisionMask;

    // Particle Systems in scene
    public ParticleSystem _particleEmitter;
    public ParticleSystem _flyingSplatterParticles;
    // Gradient used to set start of particles
    public Gradient _particleColorGradient;
    // Pool of big splat particles 
    public ParticleDecalPool _BigSplatDecalPool;
    // List that store all particle collision data
    List<ParticleCollisionEvent> _collisionEvents;    
    
    // Use this for initialization
    void Start()
    {
        _collisionEvents = new List<ParticleCollisionEvent>();
    }
    /// <summary>
    /// To detect particle collisions with other GameObjects 
    /// </summary>
    /// <param name="other"></param>
    void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(_particleEmitter, other, _collisionEvents);

        // Iterate through array of collision events to search for target collisions
        for (int i = 0; i < _collisionEvents.Count; i++)
        {
            // Only display big splatter particles if gun particles collide with layers specified by _collisionMask
            if ((_collisionMask.value & 1 << _collisionEvents[i].colliderComponent.gameObject.layer) == 1<<_collisionEvents[i].colliderComponent.gameObject.layer)
            {
                _BigSplatDecalPool.ParticleHit(_collisionEvents[i], _particleColorGradient);
            }
            EmitAtLocation(_collisionEvents[i]);

            // Play explosion particle system when target is hit by particles
            if (_collisionEvents[i].colliderComponent.gameObject.tag == "Target")
            {
                other.GetComponent<Explosion>().AnimateExplosion();
            }
        }
    }
        
   
    /// <summary>
    /// Emit splatter particles affected by gravity at collision position of particle bullet.
    /// </summary>
    /// <param name="particleCollisionEvent"></param>
    void EmitAtLocation(ParticleCollisionEvent particleCollisionEvent)
    {
        // Position particle position at collision intersection
        _flyingSplatterParticles.transform.position = particleCollisionEvent.intersection;
        // Rotate particle system away from collision location
        _flyingSplatterParticles.transform.rotation = Quaternion.LookRotation(particleCollisionEvent.normal); 
        // Access the main module of the particle system 
        ParticleSystem.MainModule psMain = _flyingSplatterParticles.main;
        // Change its start color property
        psMain.startColor = _particleColorGradient.Evaluate(Random.Range(0f, 1f));
        // Emit 1 particle for each collision
        _flyingSplatterParticles.Emit(1); 
    }
    /// <summary>
    /// Fire particle gun
    /// </summary>
    public void FireParticles()
    {
        if (Input.GetMouseButton(0))
        {
            // Access main module of particle emitter 
            ParticleSystem.MainModule psMain = _particleEmitter.main;
            // Set start color of particle
            psMain.startColor = _particleColorGradient.Evaluate(Random.Range(0f, 1f));
            // Emit 1 particle every frame
            _particleEmitter.Emit(1); 
        }
    }
}
