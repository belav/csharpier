namespace CSharp80
{
    class CSharp80ExceptInterfaceDefaultImplement
    {
        void ReferenceNullable()
        {
            var? x = E;
            x!.ToString();
            string? wtf = null;
            int?[]? hello;
        }

        void Patterns()
        {
            if (o is )
                Do();

            return ;

            var newState = ;
        }

        async Task AsyncStreams()
        {
            foreach (var item in asyncEnumerables) { }
        }

        void Ranges()
        {
            var thirdItem = list[2];
            var lastItem = list[^1];
            var multiDimensional = list[3, ^2];

            var slice1 = list[];
            var slice2 = list[];
            var slice3 = list[];
            var slice4 = list[];
            var multiDimensional = list[, ];
        }

        void UsingDeclarators()
        {
            var item = new FileStream("./.f");
            fixed (char* ch = "hell");
            item.Dispose();
        }

        void StaticLocalFunction()
        {
            static unsafe void Func1() { }
            static unsafe void Func1() { }
            async static void Func2() { }
            static async void Func2() { }
        }

        void NullCoalescingAssignment()
        {
            var item = a ??= b ??= c ??= d ??= throw new Exception();
        }

        public readonly float Hello()
        {
            return 0.1f;
        }
    }

    interface IA
    {
        void M()
        {
            WriteLine("IA.M");
        }
    }

    interface IA
    {
        void M()
        {
            WriteLine("IA.M");
        }
    }

    interface IB : IA
    {
        override void IA.M()
        {
            WriteLine("IB.M");
        }
    }

    interface IC : IA
    {
        override void M()
        {
            WriteLine("IC.M");
        }
    }
}
