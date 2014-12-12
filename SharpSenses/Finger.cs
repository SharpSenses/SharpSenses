namespace SharpSenses {
    public class Finger : FlexiblePart {

        public FingerKind Kind { get; private set; }

        public Finger(FingerKind kind) {
            Kind = kind;
        }
    }
}