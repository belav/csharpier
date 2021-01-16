namespace CSharp80
{
    class CSharp80ExceptInterfaceDefaultImplement
    {
        void ReferenceNullable()
        {
            var? x = E;
            TODO Node SuppressNullableWarningExpression.ToString();
            string? wtf = null;
            int?[]? hello;
        }

        void Patterns()
        {
            if (TODO Node IsPatternExpression)Do();

            return TODO Node SwitchExpression;

            var newState = TODO Node SwitchExpression;
        }

        async Task AsyncStreams()
        {
            foreach (var item in asyncEnumerables) { }
        }

        void Ranges()
        {
            var thirdItem = list[2];
            var lastItem = list[TODO Node IndexExpression];
            var multiDimensional = list[3, TODO Node IndexExpression];

            var slice1 = list[TODO Node RangeExpression];
            var slice2 = list[TODO Node RangeExpression];
            var slice3 = list[TODO Node RangeExpression];
            var slice4 = list[TODO Node RangeExpression];
            var multiDimensional = list[
                TODO Node RangeExpression,
                TODO Node RangeExpression
            ];
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
            var item = TODO Node CoalesceAssignmentExpression;
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
        override void M()
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
