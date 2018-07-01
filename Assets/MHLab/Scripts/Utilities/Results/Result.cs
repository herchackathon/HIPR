namespace MHLab.Utilities
{
    public struct Result<TValue, TError>
    {
        public TValue Value { get; }

        public TError Error { get; }

        internal Result(TValue value, TError error)
        {
            Value = value;
            Error = error;
        }

        public static Result<TValue, TError> Create(TValue value, TError error)
        {
            var r = new Result<TValue, TError>(value, error);
            return r;
        }
    }

    public struct Result<TValue1, TValue2, TError>
    {
        public TValue1 Value1 { get; }
        public TValue2 Value2 { get; }

        public TError Error { get; }

        internal Result(TValue1 value1, TValue2 value2, TError error)
        {
            Value1 = value1;
            Value2 = value2;
            Error = error;
        }

        public static Result<TValue1, TValue2, TError> Create(TValue1 value1, TValue2 value2, TError error)
        {
            var r = new Result<TValue1, TValue2, TError>(value1, value2, error);
            return r;
        }
    }
}
