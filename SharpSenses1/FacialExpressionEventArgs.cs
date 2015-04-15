using System;

namespace SharpSenses {
    public class FacialExpressionEventArgs : EventArgs {
        public FacialExpression OldFacialExpression { get; set; }
        public FacialExpression NewFacialExpression { get; set; }

        public FacialExpressionEventArgs(FacialExpression oldFacialExpression, FacialExpression newFacialExpression) {
            OldFacialExpression = oldFacialExpression;
            NewFacialExpression = newFacialExpression;
        }
    }
}