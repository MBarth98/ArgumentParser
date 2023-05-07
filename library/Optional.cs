using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArgumentParser
{
    public static class Optional
    {
        public static Optional<T> Some<T>(T value)
        {
            return new Optional<T>(value);
        }

        public static Optional<bool> Some()
        {
            return new Optional<bool>(true);
        }

        public static Optional<dynamic> None()
        {
            return new Optional<object>(default, false);
        }
    }

    public class Optional<T>
    {
        public Optional(T? value)
        {
            m_value = value;
            IsPresent = value != null;
        }

        public Optional(T? value, bool isPresent)
        {
            m_value = value;
            IsPresent = isPresent;
        }

        public Optional<T> Some(Action<T> func)
        {
            if (IsPresent)
            {
                func(m_value!);
            }

            return this;
        }

        public Optional<T> None(Action func)
        {
            if (!IsPresent)
            {
                func();
            }

            return this;
        }

        public static implicit operator Optional<T>(T? value)
        {
            return new Optional<T>(value, value != null);
        }

        public static implicit operator Optional<T>(Optional<dynamic> value)
        {
            if (value.IsPresent)
            {
                return new Optional<T>((T)value.m_value!, true);
            }

            return new Optional<T>(default, false);
        }

        public static implicit operator T(Optional<T> value)
        {
            if (value.IsPresent)
            {
                return value.m_value!;
            }

            throw new InvalidOperationException("Optional is not present");
        }

        public bool IsSome()
        {
            return IsPresent;
        }

        public bool IsSome(Func<T, bool> value)
        {
            return IsPresent && value(m_value!);
        }

        public bool IsNone()
        {
            return !IsPresent;
        }

        private readonly T? m_value;

        private bool IsPresent { get; set; }
    }
}