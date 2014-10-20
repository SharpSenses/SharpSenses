using System;
using System.Reflection;
using SharpSenses.Gestures;
using SharpSenses.Poses;

namespace SharpSenses {
    public abstract class Camera : ICamera {
        
        public static ICamera Create(CameraKind cameraKind) {
            return TryAssembly(cameraKind);
        }

        public static ICamera Create() {
            var cam = TryAssembly(CameraKind.RealSense);
            return cam ?? TryAssembly(CameraKind.Perceptual);
        }

        private static ICamera TryAssembly(CameraKind camraKind) {
            string name = camraKind.ToString();
            var realSense = LoadAssemblyOrNull(name);
            if (realSense == null) return null;
            try {
                return (ICamera) Activator.CreateInstance(
                    "SharpSenses." + name,
                    "SharpSenses." + name + "." + name + "Camera",
                    true, BindingFlags.Instance | BindingFlags.Public, null, null, null, null).Unwrap();
            }
            catch (Exception ex) {
                throw new CameraException(ex.Message);
            }
        }

        private static Assembly LoadAssemblyOrNull(string assemblyName) {
            try {
                return Assembly.Load("SharpSenses." + assemblyName);
            }
            catch {
                return null;
            }
        }

        protected GestureSensor _gestures;
        protected PoseSensor _poses;
        public abstract int ResolutionWidth { get; }
        public abstract int ResolutionHeight { get; }
        public Hand LeftHand { get; private set; }
        public Hand RightHand { get; private set; }
        public Face Face { get; private set; }

        public IGestureSensor Gestures {
            get { return _gestures; }
        }

        public IPoseSensor Poses {
            get { return _poses; }
        }

        public abstract void Start();
        public abstract void Dispose();

        protected Camera() {
            LeftHand = new Hand(Side.Left);
            RightHand = new Hand(Side.Right);
            Face = new Face();
            _gestures = new GestureSensor(this);
            _poses = new PoseSensor();
        }

        protected Position CreatePosition(Point3D imagePosition, Point3D worldPosition) {
            return new Position {
                Image = new Point3D(imagePosition.X, imagePosition.Y),
                World = new Point3D(ToRoundedCentimeters(worldPosition.X),
                                    ToRoundedCentimeters(worldPosition.Y),
                                    ToRoundedCentimeters(worldPosition.Z))
            };
        }
        protected double ToRoundedCentimeters(double value) {
            return Math.Round(value * 100, 2);
        }
    }
}