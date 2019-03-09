using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFB_MonsterCreatureDemo : MonoBehaviour
{

    public float turnValue = 0.0f;      // value from slider of turning
    public float turnRate = 30.0f;      // Speed of turning

    public Material[] materials;
    public bool turningOn = false;
    public float onSpeedMultiplier = 4.0f;  // Speed of turning on texture blend
    public float offSpeedMultiplier = 0.25f; // Speed of turning off texture blend
    public float blendValue = 0.0f;        // Value of blend

    public ParticleSystem[] particlesWithBlend;

    public Material[] blendMaterials;
    public Material blendMaterial;
    public float blendValue1 = 0.0f;
    public float blendValue2 = 0.0f;
    public float blendValue3 = 0.0f;

    public Slider slider1;
    public Slider slider2;
    public Slider slider3;
    public Slider hueSlider;
    public Slider saturationSlider;

    public GameObject[] RandomizableParts;

    public ParticleSystem[] stinkBreathParticles;

    public void StartStinkBreath()
    {
        Debug.Log("Start");
        for (int i = 0; i < stinkBreathParticles.Length; i++)
        {
            Debug.Log("Start " + stinkBreathParticles[i].name);
            stinkBreathParticles[i].Play();
        }
    }

    public void StopStinkBreath()
    {
        for (int i = 0; i < stinkBreathParticles.Length; i++)
        {
            stinkBreathParticles[i].Stop();
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    public void SuperRandomize()
    {
        for (int i = 0; i < RandomizableParts.Length; i++)
        {
            int random = Random.Range(0, 2);
            if (random == 0)
            {
                RandomizableParts[i].SetActive(false);
            }
            else
            {
                RandomizableParts[i].SetActive(true);
            }
        }
        hueSlider.value = Random.Range(-1.0f, 1.0f);
        saturationSlider.value = Random.Range(0.6f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetFloat("_TextureBlend", blendValue);
        }
       

        Vector3 newAngle = transform.eulerAngles;
        newAngle = new Vector3(newAngle.x, newAngle.y + (turnValue * Time.deltaTime * turnRate), newAngle.z);
        transform.eulerAngles = newAngle;


        // Use this if you want to automate the blend based on something like an animation trigger or when they get hit etc.
        if (turningOn)
        {
            blendValue += Time.deltaTime * onSpeedMultiplier;
            if (blendValue > 1)
            {
                blendValue = 1.0f;
                turningOn = false;
            }
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].SetFloat("_TextureBlend", blendValue);
            }
        }
        else if (blendValue > 0)
        {
            blendValue -= Time.deltaTime * offSpeedMultiplier;
            if (blendValue < 0)
            {
                blendValue = 0;
                for (int i = 0; i < particlesWithBlend.Length; i++)
                {
                    var em = particlesWithBlend[i].emission;
                    em.enabled = false;
                }
            }
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].SetFloat("_TextureBlend", blendValue);
            }
        }
    }

    public void SetHue(float newValue)
    {
        for (int i = 0; i < blendMaterials.Length; i++)
        {
            blendMaterials[i].SetFloat("_HueShift", newValue);
        }
        blendMaterial.SetFloat("_HueShift", newValue);
    }

    public void SetSaturation(float newValue)
    {
        for (int i = 0; i < blendMaterials.Length; i++)
        {
            blendMaterials[i].SetFloat("_Saturation", newValue);
        }
        blendMaterial.SetFloat("_Saturation", newValue);
    }

    public void SetSlider1(float newValue)
    {
        blendMaterial.SetFloat("_TextureBlend", newValue);
    }

    public void SetSlider2(float newValue)
    {
        blendMaterial.SetFloat("_TextureBlend2", newValue);
    }

    public void SetSlider3(float newValue)
    {
        blendMaterial.SetFloat("_TextureBlend3", newValue);
    }

    public void RandomizeSlider()
    {
        slider1.value = Random.Range(0.0f, 1.0f);
        slider2.value = Random.Range(0.0f, 1.0f);
        slider3.value = Random.Range(0.0f, 1.0f);
    }

    public void SetLocomotion(float value)
    {
        GetComponent<Animator>().SetFloat("Locomotion", value);
    }

    public void SetTurning(float value)
    {
        turnValue = value;
    }

    public void TurnOnBlend(float startValue)
    {
        turningOn = true;
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetFloat("_TextureBlend", startValue);
        }
        for (int i = 0; i < particlesWithBlend.Length; i++)
        {
            var em = particlesWithBlend[i].emission;
            em.enabled = true;
        }
    }

    public void SetBlendValue(float value)
    {
        blendValue = value;
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetFloat("_TextureBlend", value);
        }
    }
}
