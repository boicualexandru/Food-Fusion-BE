using System;
using System.Collections.Generic;

namespace Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        public string ErrorMessage { get; }

        protected Result(bool isSucces, string error)
        {
            if (isSucces && !String.IsNullOrEmpty(error))
                throw new InvalidOperationException();

            if (!isSucces && String.IsNullOrEmpty(error))
                throw new InvalidOperationException();

            IsSuccess = isSucces;
            ErrorMessage = error;
        }

        public static Result Fail(string message)
        {
            return new Result(false, message);
        }

        public static Result<T> Fail<T>(string message)
        {
            return new Result<T>(default(T), false, message);
        }

        public static Result<T, ErrorType> Fail<T, ErrorType>(ErrorType error) where T : Enum
        {
            return new Result<T, ErrorType>(default, false, error);
        }

        public static Result Ok()
        {
            return new Result(true, string.Empty);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value, true, string.Empty);
        }

        public static Result<T, ErrorType> Ok<T, ErrorType>(T value) where T : Enum
        {
            return new Result<T, ErrorType>(value, true, default);
        }
    }

    public class Result<T> : Result
    {
        private readonly T _value;

        public T Value
        {
            get
            {
                if (!IsSuccess)
                    throw new InvalidOperationException();

                return _value;
            }
        }

        protected internal Result(T value, bool isSucces, string error)
            : base(isSucces, error)
        {
            _value = value;
        }
    }

    public class Result<T, ErrorType> : Result where T : Enum
    {
        private readonly T _value;
        private readonly ErrorType _errorType;

        public T Value
        {
            get
            {
                if (!IsSuccess)
                    throw new InvalidOperationException();

                return _value;
            }
        }

        public ErrorType Error
        {
            get
            {
                if (IsSuccess)
                    throw new InvalidOperationException();

                return _errorType;
            }
        }

        protected internal Result(T value, bool isSucces, ErrorType errorType)
            : base(isSucces, default(string))
        {
            _value = value;
            _errorType = errorType;
        }
    }
}
