using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using MEC;

public class CustomTrackableEventHandler : MonoBehaviour, ITrackableEventHandler {

	#region PRIVATE_MEMBER_VARIABLES

	protected TrackableBehaviour mTrackableBehaviour;

	#endregion // PRIVATE_MEMBER_VARIABLES

	#region UNTIY_MONOBEHAVIOUR_METHODS
	public Vector3 initPosition;
	public Transform parentTransform;
	public GameObject prefabObjct;
	public GameObject instancedobj;

	protected virtual void Start()
	{
		mTrackableBehaviour = GetComponent<TrackableBehaviour>();
		if (mTrackableBehaviour)
			mTrackableBehaviour.RegisterTrackableEventHandler(this);
	}

	#endregion // UNTIY_MONOBEHAVIOUR_METHODS

	#region PUBLIC_METHODS

	/// <summary>
	///     Implementation of the ITrackableEventHandler function called when the
	///     tracking state changes.
	/// </summary>
	public void OnTrackableStateChanged(
		TrackableBehaviour.Status previousStatus,
		TrackableBehaviour.Status newStatus)
	{
		if (newStatus == TrackableBehaviour.Status.DETECTED ||
			newStatus == TrackableBehaviour.Status.TRACKED ||
			newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
		{
			Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
			OnTrackingFound();
		}
		else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
			newStatus == TrackableBehaviour.Status.NO_POSE)
		{
			Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
			OnTrackingLost();
		}
		else
		{
			// For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
			// Vuforia is starting, but tracking has not been lost or found yet
			// Call OnTrackingLost() to hide the augmentations
			OnTrackingLost();
		}
	}

	public void StartTracking(){
		VuforiaRuntime.Instance.InitVuforia();
		mTrackableBehaviour = GetComponent<TrackableBehaviour>();
		if (mTrackableBehaviour)
			mTrackableBehaviour.RegisterTrackableEventHandler(this);
	}

	public void StopTracking(){
		//VuforiaRuntime.Instance.Deinit ();
	}

	#endregion // PUBLIC_METHODS

	#region PRIVATE_METHODS

	protected virtual void OnTrackingFound(){
		if (instancedobj != null) {
			Destroy (instancedobj);
		}
		instancedobj = Instantiate (prefabObjct, parentTransform) as GameObject;
	}


	protected virtual void OnTrackingLost(){
		if (instancedobj == null) return;
		Timing.KillCoroutines ();
		Destroy (instancedobj);
	}

	#endregion // PRIVATE_METHODS

}
