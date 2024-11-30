using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Volume_Settings : MonoBehaviour
{
    private Volume volume;
    private VolumeProfile profile;
    public float maxPanini, maxVignette, maxChromatic;


    // Start is called before the first frame update
    void Start()
    {
        // Get the VolumeProfile
        volume = GetComponent<Volume>();
        profile = volume.profile;
    }

    public void SetSettings(float shadowDistance) {
        
        if(profile.TryGet<PaniniProjection>(out var paniniProjection)) {
            float value = Remap(shadowDistance, 10.0f, 0.0f, 0.0f, maxPanini);
            paniniProjection.distance.value = value;
        }

        if(profile.TryGet<Vignette>(out var vignette)) {
            float value1 = Remap(shadowDistance, 10.0f, 0.0f, 0.0f, maxVignette);
            vignette.intensity.value = value1;
        }

        if(profile.TryGet<ChromaticAberration>(out var chromaticAberration)) {
            float value2 = Remap(shadowDistance, 10.0f, 0.0f, 0.0f, maxChromatic);
            chromaticAberration.intensity.value = value2;
        }
    }

    private float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }


}
