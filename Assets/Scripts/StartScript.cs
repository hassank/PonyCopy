﻿using UnityEngine;
using System.Collections;

public class StartScript : MonoBehaviour {
	public GameObject dataPrefab;

	[SerializeField] private GameObject cameraHolder;
	[SerializeField] private ExperienceData data;

	[SerializeField] private AudioSource ambientAudio;
	[SerializeField] private AudioSource selectAudio;

	[SerializeField] private VRAssets.VRInput input;
	[SerializeField] private VRAssets.ReticleRadial radial;
	[SerializeField] private Animator cameraAnim;
	[SerializeField] private Animator canvasAnim;


	[SerializeField] private EarthInteraction earth;
	[SerializeField] private WorldSystem worldSys;

	[SerializeField] private GameObject explorePrompts;

	private void OnEnable() {
		radial.OnSelectionComplete += HandleStart;
		input.Back += ExitApplication;
	}

	private void OnDisable() {
		radial.OnSelectionComplete -= HandleStart;
		input.Back -= ExitApplication;
	}

	void Start() {
		GameObject dataObject = GameObject.FindGameObjectWithTag ("Data");

		if (dataObject == null) {
			dataObject = Instantiate (dataPrefab);
			data = dataObject.GetComponent<ExperienceData> ();
		} else {
			data = dataObject.GetComponent<ExperienceData> ();
			print ("Skipping start scene");

			canvasAnim.Play ("FadeCanvas");

			cameraAnim.Play ("JumpToStart");

			earth.enabled = true;
			worldSys.enabled = true;
			explorePrompts.SetActive (false);
		}
	}

	// Update is called once per frame
	void Update () {
		if (!data.started)
			radial.Show ();
	}

	void HandleStart() {
		if (!data.started) {
			data.started = true;
			selectAudio.Play ();

			radial.Hide ();

			earth.enabled = true;
			worldSys.enabled = true;

			cameraAnim.Play ("CameraTranslation");
			canvasAnim.Play ("FadeCanvas");

			StartCoroutine (PromptExploration ());
		}
	}

	void ExitApplication() {
		Application.Quit ();
	}

	IEnumerator PromptExploration() {
		yield return new WaitForSeconds (5f);

		explorePrompts.SetActive (true);
	}
}
