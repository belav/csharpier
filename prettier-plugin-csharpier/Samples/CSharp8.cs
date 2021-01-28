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
            if (o is string { Length: 5 } s)
                Do();

            return lang.CountOfTokens switch
            {
                1 => 100,
                2 => 200,
                _ => throw new global::System.Exception()
            };

            var newState = (GetState(), action, hasKey) switch
            {
                (DoorState.Closed, Action.Open, _) => DoorState.Opened,
                (DoorState.Opened, Action.Close, _) => DoorState.Closed,
                (DoorState.Closed, Action.Lock, true) => DoorState.Locked,
                (DoorState.Locked, Action.Unlock, true) => DoorState.Closed,
                (var state, _, _) => state
            };
        }

        async Task AsyncStreams()
        {
            await foreach (var item in asyncEnumerables) { }
        }

        void Ranges()
        {
            var thirdItem = list[2];                // list[2]
            var lastItem = list[^1];                // list[Index.CreateFromEnd(1)]
            var multiDimensional = list[3, ^2];     // list[3, Index.CreateFromEnd(2)]

            var slice1 = list[2..^3];               // list[Range.Create(2, Index.CreateFromEnd(3))]
            var slice2 = list[..^3];                // list[Range.ToEnd(Index.CreateFromEnd(3))]
            var slice3 = list[2..];                 // list[Range.FromStart(2)]
            var slice4 = list[..];                  // list[Range.All]
            var multiDimensional = list[1..2, ..];  // list[Range.Create(1, 2), Range.All]
        }

        void UsingDeclarators()
        {
            using var item = new FileStream("./.f");
            fixed char* ch = "hell";
            item.Dispose(); // no!
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
        override void IA.M() // explicitly named
        {
            WriteLine("IB.M");
        }
    }

    interface IC : IA
    {
        override void M() // implicitly named
        {
            WriteLine("IC.M");
        }
    }
}
