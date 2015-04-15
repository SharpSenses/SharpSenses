namespace SharpSenses {
    public struct Position {
        public Point3D Image { get; set; }
        public Point3D World { get; set; }

        public bool Equals(Position other) {
            return Image.Equals(other.Image) && World.Equals(other.World);
        }

        public override int GetHashCode() {
            unchecked {
                return (Image.GetHashCode()*397) ^ World.GetHashCode();
            }
        }

        public override string ToString() {
            return Image.ToString();
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Position && Equals((Position) obj);
        }
    }
}