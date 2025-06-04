using UnityEngine;
using System.Collections.Generic;

public class PlayerEffects : BehaviourSingleton<PlayerEffects>
{
    public List<ParticleSystem> PlayerParticleSystemList;

    public void PlayParticle(int index)
    {
        PlayerParticleSystemList[index].Play();
    }

    public void StopParticle(int index)
    {
        PlayerParticleSystemList[index].Stop();
    }
}
