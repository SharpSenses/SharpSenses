namespace SharpSenses {
    public class Face : Item {
        public Mouth Mouth { get; private set; }

        public Face() {
            Mouth = new Mouth();
        }
    }
}