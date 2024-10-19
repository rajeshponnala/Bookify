﻿using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Domain.Abstractions
{
  public class Result
    {
        protected internal Result(bool isSuccess,Error error)
        {
            if(isSuccess && error != Error.None)
            {
                throw new InvalidOperationException();
            }
            if (!isSuccess && error == Error.None)
            {
                throw new InvalidOperationException();
            }
            IsSuccess = isSuccess;
            Error = error;
        }
        public bool IsSuccess { get;}
        public bool IsFailure => !IsSuccess;
        public Error Error { get;}
        public static Result Success() => new(true, Error.None);
        public static Result Failure(Error error) => new(false,error);
        public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
        public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
        public static Result<TValue> Create<TValue>(TValue? value) =>
            value is not null ? Success(value) : Failure<TValue>(Error.NullValue);
    }

    public class Result<Tvalue> : Result
    {
        private readonly Tvalue? _value;
        protected internal Result(Tvalue? value, bool isSuccess, Error error): base(isSuccess, error)
        {
            _value = value;
        }
        [NotNull]
        public Tvalue Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("The value of a failure result cannot be accessed.");
        public static implicit operator Result<Tvalue>(Tvalue? value) => Create(value);
    }
}
