namespace SharpSenses {
    public class Face : Item {
        public Month Month { get; private set; }

        public Face() {
            Month = new Month();
        }
    }
}