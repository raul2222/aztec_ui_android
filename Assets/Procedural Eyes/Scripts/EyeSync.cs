using UnityEngine;

[ExecuteInEditMode]
public class EyeSync : MonoBehaviour {

	//Objects to be updated
	public ProceduralEye leftEye;
	public ProceduralEye rightEye;
	public PupilLook leftPupil;
	public PupilLook rightPupil;


	//Eye properties
	public EyeProps top;
	public EyeProps bottom;

	//Synchronize blinks
	private BlinkController blink;

	//Stare properties
	public StareProps stare;

	void Start(){
		blink = GetComponent<BlinkController> ();
		blink.eye = leftEye;
	}

	//Update properties
	void LateUpdate () {
		//"Flipped" angles
		leftEye.topAngle = -top.angle;
		leftEye.bottomAngle = -bottom.angle;
		leftEye.topOffset = top.offset;
		leftEye.bottomOffset = bottom.offset;

		if (Object.ReferenceEquals (leftEye, blink.eye)) {
			rightEye.percentClosed = leftEye.percentClosed;
			rightEye.closedPoint = leftEye.closedPoint;
			rightEye.blinkPointMode = leftEye.blinkPointMode;
		} else {
			leftEye.percentClosed = rightEye.percentClosed;
			leftEye.closedPoint = rightEye.closedPoint;
			leftEye.blinkPointMode = rightEye.blinkPointMode;
		}
			

		rightEye.topAngle = top.angle;
		rightEye.bottomAngle = bottom.angle;
		rightEye.topOffset = top.offset;
		rightEye.bottomOffset = bottom.offset;


		//"Shake"
		if (top.randomSpeed > 0) {
			leftEye.topOffset += perlin (Time.time * top.randomSpeed, 0f)*top.offsetRandom;
			rightEye.topOffset += perlin (Time.time * top.randomSpeed, 1f)*top.offsetRandom;
			leftEye.topAngle += perlin (Time.time * top.randomSpeed, 0f) * top.angleRandom;
			rightEye.topAngle += perlin (Time.time * top.randomSpeed, 1f) * top.angleRandom;
		}

		if (bottom.offsetRandom > 0) {
			leftEye.bottomOffset += perlin (Time.time * bottom.randomSpeed, 0f) * bottom.offsetRandom;
			rightEye.bottomOffset += perlin (Time.time * bottom.randomSpeed, 1f) * bottom.offsetRandom;
			leftEye.bottomAngle += perlin (Time.time * bottom.randomSpeed, 0f) * bottom.angleRandom;
			rightEye.bottomAngle += perlin (Time.time * bottom.randomSpeed, 1f) * bottom.angleRandom;
		}
			

		leftPupil.maxDist = stare.maxDist;
		leftPupil.targTrans = stare.targTrans;
		leftPupil.useLids = stare.useLids;
		leftPupil.flat = stare.flat;
		leftPupil.targPoint = stare.targPoint;
		leftPupil.randomOffset = stare.randomOffset;
		leftPupil.randomSpeed = stare.randomSpeed;
		leftPupil.mode = stare.mode;

		rightPupil.maxDist = stare.maxDist;
		rightPupil.targTrans = stare.targTrans;
		rightPupil.useLids = stare.useLids;
		rightPupil.flat = stare.flat;
		rightPupil.targPoint = stare.targPoint;
		rightPupil.randomOffset = stare.randomOffset;
		rightPupil.randomSpeed = stare.randomSpeed;
		leftPupil.mode = stare.mode;
	}

	//Used for top and bottom
	[System.Serializable]
	public struct EyeProps{
		[Range(-1, 1)]
		public float offset;
		[Range(-45, 45)]
		public float angle;

		//Deviation
		public float offsetRandom;
		public float angleRandom;

		//Speed of perlin noise
		public float randomSpeed;
	}

	[System.Serializable]
	public struct StareProps{
		public float maxDist;
		public Transform targTrans;
		public bool useLids;
		public bool flat;
		public Vector3 targPoint;
		public float randomOffset;
		public float randomSpeed;
		public PupilLook.LookMode mode;
	}
		
	public float perlin(float x, float y){
		return (Mathf.PerlinNoise (x, y) * 2f) - 1f;
	}
}

