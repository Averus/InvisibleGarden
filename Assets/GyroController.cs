// ***********************************************************
// Written by Heyworks Unity Studio http://unity.heyworks.com/
// ***********************************************************
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Gyroscope controller that works with any device orientation.
/// </summary>
public class GyroController : MonoBehaviour 
{
	

	private bool gyroEnabled = true;
	private const float lowPassFilterFactor = 0.2f;

	private readonly Quaternion baseIdentity =  Quaternion.Euler(90, 0, 0); 
	private readonly Quaternion landscapeRight =  Quaternion.Euler(0, 0, 90);
	private readonly Quaternion landscapeLeft =  Quaternion.Euler(0, 0, -90);
	private readonly Quaternion upsideDown =  Quaternion.Euler(0, 0, 180);
	
	private Quaternion cameraBase =  Quaternion.identity;
	private Quaternion calibration =  Quaternion.identity;
	private Quaternion baseOrientation =  Quaternion.Euler(90, 0, 0);
    private Quaternion baseOrientationRotationFix =  Quaternion.identity;

	private Quaternion referanceRotation = Quaternion.identity;
	private bool debug = true;

    InputField usersCompassValue;
    float compassOffset;
    Vector3 correctedCompassDirection;


    protected void Start () 
	{
        usersCompassValue = GameObject.Find("CompassOffset").GetComponent<InputField>();

        Input.gyro.enabled = true;
		AttachGyro();
	}

	protected void Update() 
	{
		if (!gyroEnabled)
			return;



        Quaternion q = cameraBase * (ConvertRotation(referanceRotation * Input.gyro.attitude) * GetRotFix());

        q.eulerAngles -= new Vector3(0, compassOffset, 0);

        transform.rotation = Quaternion.Slerp(transform.rotation, q, lowPassFilterFactor);


        /*
		transform.rotation = Quaternion.Slerp(transform.rotation,
			cameraBase * ( ConvertRotation(referanceRotation * Input.gyro.attitude) * GetRotFix()), lowPassFilterFactor);

        */


    }

	protected void OnGUI()
	{
		if (!debug)
			return;
        /*
		GUILayout.Label("Orientation: " + Screen.orientation);
		GUILayout.Label("Calibration: " + calibration);
		GUILayout.Label("Camera base: " + cameraBase);
		GUILayout.Label("input.gyro.attitude: " + Input.gyro.attitude);
		GUILayout.Label("transform.rotation: " + transform.rotation);


        

		if (GUILayout.Button("On/off gyro: " + Input.gyro.enabled, GUILayout.Height(50)))
		{
			Input.gyro.enabled = !Input.gyro.enabled;
		}

		if (GUILayout.Button("On/off gyro controller: " + gyroEnabled, GUILayout.Height(50)))
		{
			if (gyroEnabled)
			{
				DetachGyro();
			}
			else
			{
				AttachGyro();
			}
		}

		if (GUILayout.Button("Update gyro calibration (Horizontal only)", GUILayout.Height(40)))
		{
			UpdateCalibration(true);
		}

		if (GUILayout.Button("Update camera base rotation (Horizontal only)", GUILayout.Height(40)))
		{
			UpdateCameraBaseRotation(true);
		}

		if (GUILayout.Button("Reset base orientation", GUILayout.Height(40)))
		{
			ResetBaseOrientation();
		}

		if (GUILayout.Button("Reset camera rotation", GUILayout.Height(40)))
		{
			transform.rotation = Quaternion.identity;
		}

    */
    }


    private void AttachGyro()
	{
		gyroEnabled = true;
		ResetBaseOrientation();
		UpdateCalibration(true);
		UpdateCameraBaseRotation(true);
		RecalculateReferenceRotation();
	}


	private void DetachGyro()
	{
		gyroEnabled = false;
	}

    public void SetCorrectionAngle()
    {
        float.TryParse(usersCompassValue.text, out compassOffset);
        Debug.Log("value is " + compassOffset);


        

       // transform.Rotate(correctedCompassDirection);

    }


    private void UpdateCalibration(bool onlyHorizontal)
	{
		if (onlyHorizontal)
		{
			var fw = (Input.gyro.attitude) * (-Vector3.forward);
			fw.z = 0;
			if (fw == Vector3.zero)
			{
				calibration = Quaternion.identity;
			}
			else
			{
				calibration = (Quaternion.FromToRotation(baseOrientationRotationFix * Vector3.up, fw));
			}
		}
		else
		{
			calibration = Input.gyro.attitude;
		}
	}
	

	private void UpdateCameraBaseRotation(bool onlyHorizontal)
	{
		if (onlyHorizontal)
		{
			var fw = transform.forward;
			fw.y = 0;
			if (fw == Vector3.zero)
			{
				cameraBase = Quaternion.identity;
			}
			else
			{
				cameraBase = Quaternion.FromToRotation(Vector3.forward, fw);
			}
		}
		else
		{
			cameraBase = transform.rotation;
		}
	}
	

	private static Quaternion ConvertRotation(Quaternion q)
	{
		return new Quaternion(q.x, q.y, -q.z, -q.w);	
	}
	

	private Quaternion GetRotFix()
	{
#if UNITY_3_5
		if (Screen.orientation == ScreenOrientation.Portrait)
			return Quaternion.identity;
		
		if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.Landscape)
			return landscapeLeft;
				
		if (Screen.orientation == ScreenOrientation.LandscapeRight)
			return landscapeRight;
				
		if (Screen.orientation == ScreenOrientation.PortraitUpsideDown)
			return upsideDown;
		return Quaternion.identity;
#else
		return Quaternion.identity;
#endif
	}
	

	private void ResetBaseOrientation()
	{
		baseOrientationRotationFix = GetRotFix();
		baseOrientation = baseOrientationRotationFix * baseIdentity;
	}

	private void RecalculateReferenceRotation()
	{
		referanceRotation = Quaternion.Inverse(baseOrientation)*Quaternion.Inverse(calibration);
	}


}
