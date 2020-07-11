using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SGDb.Common.Infrastructure
{
    public class Result<TData>
    {
        private readonly IEnumerable<string> _errors;
        private readonly TData _data;

        internal Result(bool succeeded, IEnumerable<string> errors, TData data) : this(succeeded, errors)
        {
            this._data = data;
        }

        internal Result(bool succeeded, IEnumerable<string> errors)
        {
            this.Succeeded = succeeded;
            this._errors = errors;
        }

        public bool Succeeded { get; }

        public virtual TData Data => this.Succeeded ? this._data : throw new InvalidOperationException("No data is loaded.");

        public IEnumerable<string> Errors => this.Succeeded ? new List<string>() : this._errors;

        public static Result<TData> SuccessWith(TData data)
            => new Result<TData>(true, new List<string>(), data);
    }

    public class Result : Result<string>
    {
        internal Result(bool succeeded, IEnumerable<string> errors, string data) : base(succeeded, errors, data)
        {
        }

        internal Result(bool succeeded, IEnumerable<string> errors) : base(succeeded, errors)
        {
        }

        [JsonIgnore] public override string Data { get; }

        public static Result Success() => new Result(true, new List<string>());

        public static Result Failure() => new Result(false, new List<string>());
        
        public static Result Failure(string error) => new Result(false, new List<string> {error});

        public static implicit operator Result(ModelStateDictionary modelState)
        {
            var error = modelState
                .FirstOrDefault(m => m.Value.Errors.Count > 0);
            
            return new Result(false, new string[]
            {
                $"{error.Key} - {error.Value.Errors.FirstOrDefault().ErrorMessage}"
            });
        }
    }
}