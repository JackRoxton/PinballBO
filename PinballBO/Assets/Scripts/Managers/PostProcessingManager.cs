using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingManager : MonoBehaviour
{
    public static PostProcessingManager Instance;
    PostProcessVolume volume;
    Bloom bloom;

    void Awake()
    {
        Instance = this;

        bloom = ScriptableObject.CreateInstance<Bloom>();
        bloom.enabled.Override(true);
        bloom.intensity.Override(5);

        volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, bloom);

    }

    public IEnumerator BloomIntensity(float value, float amount)
    {
        if (value > bloom.intensity.value)
        {
            while (value > bloom.intensity.value)
            {
                bloom.intensity.Override(bloom.intensity.value + amount);
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (value < bloom.intensity.value)
            {
                bloom.intensity.Override(bloom.intensity.value - amount);
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public void BloomIntensity(float value)
    {
        bloom.intensity.Override(value);
    }


    public IEnumerator BloomDiffusion(float value, float amount)
    {
        if (value > bloom.diffusion.value)
        {
            while (value > bloom.diffusion.value)
            {
                bloom.diffusion.Override(bloom.diffusion.value + amount);
                yield return new WaitForEndOfFrame();
            }
            bloom.diffusion.Override(value);
        }
        else
        {
            while (value < bloom.diffusion.value)
            {
                bloom.intensity.Override(bloom.diffusion.value - amount);
                yield return new WaitForEndOfFrame();
            }
            bloom.diffusion.Override(value);
        }
    }
}
