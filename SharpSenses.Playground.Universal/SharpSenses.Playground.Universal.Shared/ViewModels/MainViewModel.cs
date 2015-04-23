namespace SharpSenses.Playground.Universal.ViewModels {
    public class MainViewModel : ObservableObject {

        public Hand LeftHand { get; set; }
        public Hand RightHand { get; set; }

        public MainViewModel(ICamera camera) {
            LeftHand = camera.LeftHand;
            RightHand = camera.RightHand;
        }
    }
}
