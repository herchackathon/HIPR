using System;

namespace MHLab.Utilities
{
    public struct Result<T>
    {
        public T Value;

        public Exception Error;

        public bool HasError
        {
            get { return Error != null; }
        }

        private Result(T value, Exception error)
        {
            Value = value;
            Error = error;
        }

        public static Result<T> Create(T value, Exception error)
        {
            var r = new Result<T>(value, error);
            return r;
        }
    }
}
