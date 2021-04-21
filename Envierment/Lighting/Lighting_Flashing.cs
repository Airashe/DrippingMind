using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighting_Flashing : MonoBehaviour {

	private Light _light;
    public Light secondLight;
	private float start_intensity;
	public float min_intensity = 3.0f;
	public float max_intensity = 1.5f;
	public float noise_speed = 0.15f;
	public bool flicker_ON;
	public bool random_timer;
	public float random_timer_value_MIN = 5f;
	public float random_timer_value_MAX = 20f;
	public float random_timer_value;
	public float start_timer_value = 0.1f;
	public AudioSource flame_sound;
	


	// Use this for initialization
	IEnumerator Start () {
		_light = GetComponent<Light>();
		start_intensity = _light.intensity;
		yield return new WaitForSeconds(start_timer_value);
		if(flicker_ON)
            if(flame_sound != null)
            {
                flame_sound.Play();
            }

		while(random_timer){
			random_timer_value = Random.Range(random_timer_value_MIN, random_timer_value_MAX);
			yield return new WaitForSeconds(random_timer_value);
			if(flicker_ON){
				_light.intensity = start_intensity;
                if (flame_sound != null)
                {
                    flame_sound.Pause();
                }
				flicker_ON = false;
			}
			else{
                if (flame_sound != null)
                {
                    flame_sound.Play();
                }
				flicker_ON = true;
			}
		}
		
	}
	
	// Update is called once per frame
	void Update () {
        if (flicker_ON)
        {
            _light.intensity = Mathf.Lerp(min_intensity, max_intensity, Mathf.PerlinNoise(10, Time.time / noise_speed));
            if(secondLight != null)
            {
                secondLight.intensity = Mathf.Lerp(min_intensity, max_intensity, Mathf.PerlinNoise(10, Time.time / noise_speed));
            }
        }
		
	}
}
