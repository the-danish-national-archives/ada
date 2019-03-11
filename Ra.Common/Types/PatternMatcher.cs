#region Header

// Author 
// Created 20

#endregion

namespace Ra.Common.Types
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;

    #endregion

    public class PatternMatcher<TOutput>
    {
        #region  Fields

        private readonly List<Tuple<Predicate<object>, Func<object, TOutput>>> cases = new List<Tuple<Predicate<object>, Func<object, TOutput>>>();

        #endregion

        #region  Constructors

        #endregion

        #region

        public PatternMatcher<TOutput> Case(Predicate<object> condition, Func<object, TOutput> function)
        {
            cases.Add(new Tuple<Predicate<object>, Func<object, TOutput>>(condition, function));
            return this;
        }

        public PatternMatcher<TOutput> Case<T>(Predicate<T> condition, Func<T, TOutput> function)
        {
            return Case(
                o => o is T && condition((T) o),
                o => function((T) o));
        }

        public PatternMatcher<TOutput> Case<T>(Func<T, TOutput> function)
        {
            return Case(
                o => o is T,
                o => function((T) o));
        }

        public PatternMatcher<TOutput> Case<T>(Predicate<T> condition, TOutput o)
        {
            return Case(condition, x => o);
        }

        public PatternMatcher<TOutput> Case<T>(TOutput o)
        {
            return Case<T>(x => o);
        }

        public PatternMatcher<TOutput> Default(Func<object, TOutput> function)
        {
            return Case(o => true, function);
        }

        public PatternMatcher<TOutput> Default(TOutput o)
        {
            return Default(x => o);
        }

        public TOutput Match(object o)
        {
            foreach (var tuple in cases)
                if (tuple.Item1(o))
                    return tuple.Item2(o);
            throw new Exception("Failed to match");
        }

        #endregion

        // Example
//        public enum EngineType
//        {
//            Diesel,
//            Gasoline
//        }
//
//        public class Bicycle
//        {
//            public int Cylinders;
//        }
//
//        public class Car
//        {
//            public EngineType EngineType;
//            public int Doors;
//        }
//
//        public class MotorCycle
//        {
//            public int Cylinders;
//        }
//
//        public void Run()
//        {
//            var getRentPrice = new PatternMatcher<int>()
//                .Case<MotorCycle>(bike => 100 + bike.Cylinders * 10)
//                .Case<Bicycle>(30)
//                .Case<Car>(car => car.EngineType == EngineType.Diesel, car => 220 + car.Doors * 20)
//                .Case<Car>(car => car.EngineType == EngineType.Gasoline, car => 200 + car.Doors * 20)
//                .Default(0);
//
//            var vehicles = new object[] {
//            new Car { EngineType = EngineType.Diesel, Doors = 2 },
//            new Car { EngineType = EngineType.Diesel, Doors = 4 },
//            new Car { EngineType = EngineType.Gasoline, Doors = 3 },
//            new Car { EngineType = EngineType.Gasoline, Doors = 5 },
//            new Bicycle(),
//            new MotorCycle { Cylinders = 2 },
//            new MotorCycle { Cylinders = 3 },
//        };
//
//            foreach (var v in vehicles)
//            {
//                Console.WriteLine("Vehicle of type {0} costs {1} to rent", v.GetType(), getRentPrice.Match(v));
//            }
//        }
    }
}