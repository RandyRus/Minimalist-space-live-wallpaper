using UnityEngine;

namespace LostPolygon.uLiveWallpaper.Demos {
    /// <summary>
    /// An example of applying a parallax effect that utilizes accelerometer/gyroscope.
    /// </summary>
    public class AccelerometerParallax : MonoBehaviour {
#pragma warning disable 0414
        [SerializeField]
        private Transform _controlledTransform;

        [SerializeField]
        [Range(0f, 25f)]
        private float _rotationSmoothing = 1f / 2.5f;

        [SerializeField]
        private Vector2 _sensitivity = Vector2.one * 0.3f;

        [SerializeField]
        [Range(0f, 180f)]
        private float _maxRotationAngle = 3f;

        private Vector3 _lastAcceleration;
        private Vector2 _lastRotation;
        private Vector2 _deltaRotation;
        private Vector3 _controlledTransformLocalRotationEulerAnglesRelative;
        private Vector3 _controlledTransformLocalRotationEulerAnglesInitial;
#pragma warning restore 0414

#if UNITY_ANDROID
        public float RotationSmoothing {
            get {
                return _rotationSmoothing;
            }
            set {
                _rotationSmoothing = value;
            }
        }

        public Transform ControlledTransform {
            get {
                return _controlledTransform;
            }
            set {
                _controlledTransform = value;
            }
        }

        public float MaxRotationAngle {
            get {
                return _maxRotationAngle;
            }
        }

        public Vector2 Sensitivity {
            get {
                return _sensitivity;
            }
            set {
                _sensitivity = value;
            }
        }

        private void OnEnable() {
            if (_controlledTransform == null) {
                Debug.LogWarning("Controlled transform is not set, AccelerometerParallax won't work", this);
            }

            ResetState();
        }

        private void OnDisable() {
            if (_controlledTransform != null) {
                _controlledTransform.localRotation = Quaternion.Euler(_controlledTransformLocalRotationEulerAnglesInitial);
            }
        }

        private void OnApplicationPause(bool pauseStatus) {
            if (pauseStatus) {
                return;
            }

            ResetState(false);
        }

        private void Update() {
            if (Application.platform != RuntimePlatform.Android
#if UNITY_EDITOR
                && !UnityEditor.EditorApplication.isRemoteConnected
#endif
            ) {
                return;
            }

            if (_controlledTransform == null)
                return;

            if (SystemInfo.supportsGyroscope && Input.gyro.enabled) {
                Input.gyro.enabled = true;
                Vector3 gyroRotationRate = Input.gyro.rotationRateUnbiased;
                Vector2 adaptedGyroRotationRate = gyroRotationRate * Mathf.Rad2Deg * Time.deltaTime;

                _deltaRotation = Vector2.Lerp(_deltaRotation, adaptedGyroRotationRate, 1f / _rotationSmoothing * Time.deltaTime);
            } else {
                // Adapted from
                // http://vitiy.info/how-to-create-parallax-effect-using-accelerometer/
                Vector3 acceleration = Vector3.Lerp(_lastAcceleration, Input.acceleration, 1f / _rotationSmoothing * Time.deltaTime);
                float accelerationMagnitude = acceleration.magnitude;

                if (Mathf.Abs(accelerationMagnitude) > Vector3.kEpsilon) {
                    acceleration *= 1f / accelerationMagnitude;
                }

                float roll = 0;
                if (Mathf.Abs(acceleration.z) > Vector3.kEpsilon) {
                    roll = Mathf.Atan2(acceleration.x, acceleration.z) * Mathf.Rad2Deg;
                }

                float pitch = new Vector2(acceleration.x, acceleration.z).magnitude;
                if (Mathf.Abs(pitch) > Vector3.kEpsilon) {
                    pitch = Mathf.Atan2(acceleration.y, pitch) * Mathf.Rad2Deg;
                }

                _deltaRotation.y = roll - _lastRotation.y;
                _deltaRotation.x = pitch - _lastRotation.x;

                // If rotation was too intensive – more than 180 degrees – skip it
                if (_deltaRotation.x > 180f || _deltaRotation.x < -180f) _deltaRotation.x = 0;
                if (_deltaRotation.y > 180f || _deltaRotation.y < -180f) _deltaRotation.y = 0;

                // If device orientation is close to vertical – rotation around x is almost undefined – skip!
                if (acceleration.y > 1f - Vector3.kEpsilon) _deltaRotation.y = 0;

                _lastAcceleration = acceleration;

                _lastRotation.y = roll;
                _lastRotation.x = pitch;
            }

            // Apply delta rotation
            _controlledTransformLocalRotationEulerAnglesRelative -=
                new Vector3(_deltaRotation.x * _sensitivity.x, _deltaRotation.y * _sensitivity.y, 0f);

            // Limit rotation
            _controlledTransformLocalRotationEulerAnglesRelative.x =
                Mathf.Clamp(_controlledTransformLocalRotationEulerAnglesRelative.x, -_maxRotationAngle, _maxRotationAngle);
            _controlledTransformLocalRotationEulerAnglesRelative.y =
                Mathf.Clamp(_controlledTransformLocalRotationEulerAnglesRelative.y, -_maxRotationAngle, _maxRotationAngle);

            // Set rotation to the transform
            _controlledTransform.localRotation = Quaternion.Euler(_controlledTransformLocalRotationEulerAnglesInitial + _controlledTransformLocalRotationEulerAnglesRelative);
        }

        private void ResetState(bool resetInitialRotation = true) {
            _controlledTransformLocalRotationEulerAnglesRelative = Vector3.zero;
            if (resetInitialRotation && _controlledTransform != null) {
                _controlledTransformLocalRotationEulerAnglesInitial = _controlledTransform.localRotation.eulerAngles;
            }

            _lastAcceleration = Input.acceleration;
        }
#endif
    }
}
