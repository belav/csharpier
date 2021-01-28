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
            var thirdItem = list[2];
            var lastItem = list[^1];
            var multiDimensional = list[3, ^2];

            var slice1 = list[2..^3];
            var slice2 = list[..^3];
            var slice3 = list[2..];
            var slice4 = list[..];
            var multiDimensional = list[1..2, ..];
        }

        void UsingDeclarators()
        {
            using var item = new FileStream("./.f");
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
