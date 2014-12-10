namespace SharpSenses {
    public struct Position {
        public Point3D Image { get; set; }
        public Point3D World { get; set; }

        public override string ToString() {
            return Image.ToString();
        }
    }
}