using UnityEngine;
using System.Collections.Generic;

public class BossEffectManager : BehaviourSingleton<BossEffectManager>
{
    public List<ParticleSystem> Boss1ParticleSystmList;

    public void PlayBoss1Particle(int index)
    {
        Boss1ParticleSystmList[index].Play();
    }

    public void StopBoss1Particle(int index)
    {
        Boss1ParticleSystmList[index].Stop();
    }
}
