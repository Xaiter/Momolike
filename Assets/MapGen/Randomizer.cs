using System;
using System.Collections.Generic;

using System.Text;

namespace MapGen
{
    public static class Randomizer
    {
        private static Random _random = new Random();
        private static Directions[] _directions = (Directions[])Enum.GetValues(typeof(Directions));
        private static int _seed;

        public static int Seed
        {
            get { return _seed; }
            private set { _seed = value; _random = new Random(value); }
        }

        public static int GetRandomNumber()
        {
            return _random.Next();
        }
        public static int GetRandomNumber(int max)
        {
            return _random.Next(max);
        }
        public static int GetRandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }
        public static int GetHighRandomNumber(int max)
        {
            return GetHighRandomNumber(0, max);
        }
        public static int GetHighRandomNumber(int min, int max)
        {
            int num = _random.Next(min, max);
            num = (int)(num * 2);
            while(num >= max)
            {
                int sub = (max / 10);
                if (sub == 0)
                    sub = 2;
                num -= (sub);
            }
            return num;
        }

        public static double GetRandomDouble()
        {
            return _random.NextDouble();
        }

        public static bool GetRandomBool(double odds)
        {
            return GetRandomDouble() <= odds;
        }

        public static Directions GetRandomDirection()
        {
            return Randomizer.PickRandomItem(_directions);
        }

        public static void SetSeed(int seed)
        {
            Seed = seed;
        }

        public static void RandomizeSeed()
        {
            _random = new Random();
        }

        public static T PickRandomItem<T>(T[] items)
        {
            return items[_random.Next(0, items.Length)];
        }

        /// <summary>
        /// Shuffles this array in place.
        /// </summary>
        public static void Shuffle<T>(T[] items)
        {
            int maxLength = items.Length;

            for (int i = 0; i < items.Length; i++)
                Randomizer.Swap(items, Randomizer.GetRandomNumber(items.Length), Randomizer.GetRandomNumber(items.Length));
        }

        private static void Swap<T>(T[] items, int a, int b)
        {
            T temp = items[a];
            items[a] = items[b];
            items[b] = temp;
        }
    }    
}
