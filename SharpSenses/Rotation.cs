namespace SharpSenses {
    public class Rotation {
        public double Roll { get; set; }
        public double Yaw { get; set;}
        public double Pitch { get; set; }
        
        public Rotation(double roll, double yaw, double pitch) {
            Roll = roll;
            Yaw = yaw;
            Pitch = pitch;
        }
        
        protected bool Equals(Rotation other) {
            return Roll.Equals(other.Roll) && Yaw.Equals(other.Yaw) && Pitch.Equals(other.Pitch);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = Roll.GetHashCode();
                hashCode = (hashCode*397) ^ Yaw.GetHashCode();
                hashCode = (hashCode*397) ^ Pitch.GetHashCode();
                return hashCode;
            }
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }
            if (ReferenceEquals(this, obj)) {
                return true;
            }
            if (obj.GetType() != GetType()) {
                return false;
            }
            return Equals((Rotation) obj);
        }
    }
}