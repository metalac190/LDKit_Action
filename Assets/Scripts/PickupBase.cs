using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public abstract class PickupBase : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] private ParticleSystem _collectParticle = null;
    [SerializeField] private AudioClip _collectSound = null;
    public UnityEvent OnCollect;    // this event gets called on player trigger collision

    public abstract void Collect();

    public virtual void PlayGFX()
    {
        if(_collectParticle != null)
        {
            //TODO convert to objectPool particle spawn/death system
            ParticleSystem particles = Instantiate(_collectParticle, 
                this.transform.position, Quaternion.identity);
            particles.Play();
        }
    }

    public virtual void PlaySFX()
    {
        if(_collectSound != null)
        {
            //TODO play sound
        }
    }

    // this is a separate function in case we want to override and delay destruction or something
    public virtual void DisableObject()
    {
        gameObject.SetActive(false);
    }

    #region Monobehaviour
    private void Awake()
    {
        Collider collider = GetComponent<Collider>();
        // error checking: ensure there's a collider
        if(collider == false)
        {
            Debug.LogError("Pickup: " + this.gameObject.name + " has no trigger collider. "
                + "Disabling pickup script.");
            this.enabled = false;
        }
        // error checking: ensure the collider is a trigger
        if(collider.isTrigger == false)
        {
            Debug.LogError("Pickup: " + this.gameObject.name + " collider MUST be set to trigger. "
                + "Setting collider to trigger.");
            this.enabled = false;
        }
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        // if the player has touched this collider, collect
        IPlayer player = otherCollider.GetComponent<IPlayer>();
        if (player != null)
        {
            Collect();
            // visual/audio
            PlayGFX();
            PlaySFX();
            // trigger level hookups too
            OnCollect?.Invoke();
            // hide the evidence
            DisableObject();
        }

    }
    #endregion
}
