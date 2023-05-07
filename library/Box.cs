using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArgumentParser
{
    public static class Box
    {
        public static Box<T> Some<T>(T value)
        {
            return new Box<T>(value);
        }

        public static Box<bool> Some()
        {
            return new Box<bool>(true);
        }

        public static Box<dynamic> None()
        {
            return new Box<object>(default, false);
        }
    }

    public class Box<T>
    {
        public Box(T? value)
        {
            m_value = value;
            IsPresent = value != null;
        }

        public Box(T? value, bool isPresent)
        {
            m_value = value;
            IsPresent = isPresent;
        }
        
        public T? Unwrap()
        {
            return m_value;
        }

        public Box<T> Some(Action<T> func)
        {
            if (IsPresent)
            {
                func(m_value!);
            }

            return this;
        }

        public Box<T> None(Action func)
        {
            if (!IsPresent)
            {
                func();
            }

            return this;
        }

        public Box<T> Require(Func<T, bool> predicate)
        {
            IsPresent = IsPresent && predicate(m_value!);
            return this;
        }

        public static implicit operator Box<T>(T? value)
        {
            return new Box<T>(value, value != null);
        }

        public static implicit operator Box<T>(Box<dynamic> value)
        {
            if (value.IsPresent)
            {
                return new Box<T>((T)value.m_value!, true);
            }

            return new Box<T>(default, false);
        }

        public static implicit operator T(Box<T> value)
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