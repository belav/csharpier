using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Math;
using System.Diagnostics;
using ConsoleApplication2.Test;
using int1;
using ABC.X<int>;
using System.Math;
using System.DayOfWeek;
using System.Linq.Enumerable;

class TopLevelType : IDisposable
{
    void Dispose() { }
}

namespace My
{
    using A.B;
    interface CoContra { }
    delegate void CoContra2<T, K>() where T : struct;

    public unsafe partial class A : C,
    I
    {
        static extern bool CreateDirectory(string name, SecurityAttribute sa);

        private const int global = int.MinValue - 1;

        static A() { }
        public A(int foo)
        {
            L:

            {
                int i = sizeof(int);
                ++i;
                var s1 = $"x {1}";
                var s2 = $@"x {1}";
            }
            int? local = int.MaxValue;
            Guid? local0 = new Guid(r.ToString());

            var ?????? = local;
            var ??? = local;
            var local3 = 0, local4 = 1;
            local3 = local4 = 1;
            var local5 = null as Action ?? null;
            var local6 = local5 is Action;

            var u = 1u;
            var U = 1U;
            long hex = 0xBADC0DE, Hex = 0XDEADBEEF, l = -1L, L = 1L, l2 = 2l;
            ulong ul = 1ul, Ul = 1Ul, uL = 1uL, UL = 1UL, lu = 1lu, Lu = 1Lu, lU = 1lU, LU = 1LU;
            int minInt32Value = -2147483648;
            int minInt64Value = -9223372036854775808L;

            bool @bool;
            byte @byte;
            char @char = 'c', \u0066 = '\u0066', hexchar = '\x0130', hexchar2 = (char)0xBAD;
            string \U00000065 = "\U00000065";
            decimal @decimal = 1.44M;
            @decimal = 1.2m;
            dynamic @dynamic;
            double @double = M.PI;
            @double = 1d;
            @double = 1D;
            @double = -1.2e3;
            float @float = 1.2f;
            @float = 1.44F;
            int @int = local ?? -1;
            long @long;
            object @object;
            sbyte @sbyte;
            short @short;
            string @string = @"""/*";
            uint @uint;
            ulong @ulong;
            ushort @ushort;

            dynamic dynamic = local5;
            var add = 0;
            var alias = 0;
            var arglist = 0;
            var ascending = 0;
            var async = 0;
            var await = 0;
            var by = 0;
            var descending = 0;
            var dynamic = 0;
            var equals = 0;
            var from = 0;
            var get = 0;
            var group = 0;
            var into = 0;
            var join = 0;
            var let = 0;
            var nameof = 0;
            var on = 0;
            var orderby = 0;
            var partial = 0;
            var remove = 0;
            var select = 0;
            var set = 0;
            var when = 0;
            var where = 0;
            var yield = 0;
            var __ = 0;
            where = yield = 0;

            if (i > 0)
            {
                return;
            }
            else if (i == 0)
            {
                throw new Exception();
            }
            var o1 = new MyObject();
            var o2 = new MyObject(var);
            var o3 = new MyObject { A = i };
            var o4 = new MyObject(@dynamic) { A = 0, B = 0, C = 0 };
            var o5 = new { A = 0 };
            var dictionaryInitializer = new Dictionary<int, string>
            {
                { 1, "" },
                { 2, "a" }
            };
            float[] a = TODO Node ArrayCreationExpression;
            int[, , ] cube = TODO Node ArrayInitializerExpression;
            int[][] jagged = TODO Node ArrayInitializerExpression;
            int[][, ] arr = TODO Node ArrayCreationExpression;
            arr[0] = TODO Node ArrayCreationExpression;
            arr[0][0, 0] = 47;
            int[] arrayTypeInference = TODO Node ImplicitArrayCreationExpression;
            switch (3) { }
            switch (i)
            {
                case 0:
                case 1:
                {
                    goto case 2;
                }
                case 2 + 3:
                {
                    goto default;
                    break;
                }
                default:
                {
                    return;
                }
            }
            while (i < 10)
            {
                ++i;
                if (true)continue;
                break;
            }
            do
            {
                ++i;
                if (true)continue;
                break;
            }
            while (i < 10);
            for (int j = 0; j < 100; ++j)
            {
                for (; ; )
                {
                    for (int i = 0, j = 0; i < length; i++, j++) { }
                    if (true)continue;
                    break;
                }
            }
            label:
            goto label;
            label2:
            ;
            foreach (var i in Items())
            {
                if (i == 7)return;
                else continue;
            }
            checked
            {
                checked(++i);
            }
            unchecked
            {
                unchecked(++i);
            }
            lock (sync)process();
            using (var v = BeginScope())
            using (A a = new A())
            using (A a = new A(), b = new A())
            using ()
            return;
            yield return [object Object];
            yield break;
            fixed (int* p = stackalloc int[100], q = &y)
            {
                *intref = 1;
            }
            fixed (int* p = stackalloc int[100])
            {
                *intref = 1;
            }
            unsafe
            {
                int* p = null;
            }
            try
            {
                throw null;
            }
            catch (System.AccessViolationExceptionav)
            {
                throw av;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                try { }
                catch  { }
            }
            var anonymous = TODO Node ArrayInitializerExpression;
            var query = TODO Node QueryExpression;
            query = TODO Node QueryExpression;
        }
        ~A() { }
        private readonly int f1;
        private volatile int f2;
        public void Handler(object value) { }
        public int m<T>(T t)
        {
            base.m(t);
            return 1;
        }
        public string P
        {
            get { return "A"; }
            set;
        }
        public abstract string P { get; }
        TODO Node IndexerDeclaration
        TODO Node EventFieldDeclaration
        TODO Node EventDeclaration
        TODO Node OperatorDeclaration
        TODO Node OperatorDeclaration
        TODO Node OperatorDeclaration
        class C { }
    }
    public struct S : I
    {
        public S() { }
        private int f1;
        private volatile int f2;
        public abstract int m<T>(T t)
        {
            return 1;
        }
        public string P
        {
            get { int value = 0; return "A"; }
            set;
        }
        public abstract string P { get; }
        TODO Node IndexerDeclaration
        TODO Node EventFieldDeclaration
        TODO Node OperatorDeclaration
        fixed int field;
        class C { }
    }
    public interface I
    {
        void A(int value);

        string Value { get; set; }

        unsafe void UpdateSignatureByHashingContent(byte* buffer, int size);
    }
    public enum E
    {
        A,
        B,
        C,
        E
    }
    delegate void Delegate(object P);
    namespace Test
    {
        using System;
        using System.Collections;
        public class ??????
        {
            public static IEnumerable Power(int number, int exponent)
            {
                ?????? ?????? = new ??????();
                ??????.Main();
                int counter = (0 + 0);
                int ??? = 0;
                while (++counter++ < --exponent--)
                {
                    result = result * number + TODO Node UnaryPlusExpression + number;
                    yield return result;
                }
            }
            static void Main()
            {
                foreach (int i in Power(2, 8))
                {
                    Console.Write("{0} ", i);
                }
            }
            async void Wait()
            {
                await System.Threading.Tasks.Task.Delay(0);
            }
            void AsyncAnonymous()
            {
                var task = Task.Factory.StartNew(() => );
            }
        }
    }
}

namespace ConsoleApplication1
{
    namespace RecursiveGenericBaseType
    {
        class A : B<A<T>, A<T>>
        {
            protected virtual A<T> M() { }
            protected abstract B<A<T>, A<T>> N() { }
            static B<A<T>, A<T>> O() { }
        }

        sealed class B : A<B<T1, T2>>
        {
            protected override A<T> M() { }
            protected sealed override B<A<T>, A<T>> N() { }
            new static A<T> O() { }
        }
    }

    namespace Boo
    {
        public class Bar
        {
            public T f;
            public class Foo : IEnumerable<T>
            {
                public void Method<K, V>(K k, T t, U u)
                {
                    A<int> a;
                    M(A<B, C>(5));
                }
            }
        }
    }

    class Test
    {
        void Bar3()
        {
            var x = new Boo.Bar<int>.Foo<object>();
            x.Method<string, string>(" ", 5, new object());

            var q = TODO Node QueryExpression;
        }
        TODO Node ConversionOperatorDeclaration
        TODO Node ConversionOperatorDeclaration

        public int foo = 5;
        void Bar2()
        {
            foo = 6;
            this.Foo = 5.GetType();
            Test t = "sss";
        }
        TODO Node EventFieldDeclaration

        void Blah()
        {
            int i = 5;
            int? j = 6;

            Expression<Func<int>> e = () => i;
            Expression<Func<bool, Action>> e2 = b => () => ;
            Func<bool, bool> f = TODO Node AnonymousMethodExpression;
            Func<int, int, int> f2 = () => 0;
            f2 = () => 1;
            Action a = Blah;
            f2 = () => ;
            f2 = () => ;
        }

        delegate Recursive Recursive(Recursive r);
        delegate Recursive Recursive<A, R>(Recursive<A, R> r);

        public Type Foo
        {
            get
            {
                var result = typeof(IEnumerable<int>);
                var t = typeof(int?) == typeof(Nullable<int>);
                t = typeof(IEnumerable<int?[][][]>);
                return typeof(IEnumerable<>);
            }
            set { var t = typeof(System.Int32); t.ToString(); t = value; }
        }

        public void Constants()
        {
            int i = 1 + 2 + 3 + 5;
            global::System.String s = "a" + (System.String)"a" + "a" + "a" + "a" + "A";
        }

        public void ConstructedType()
        {
            List<int> i = null;
            int c = i.Count;
        }
    }
}

namespace Comments.XmlComments.UndocumentedKeywords
{
    class C
    {
        void M<U>(T t, U u)
        {

            int intValue = 0;
            intValue = intValue + 1;
            string strValue = "hello";
            MyClass c = new MyClass();
            string verbatimStr = @"\\\\";
        }
    }
    //General Test F. Type a very long class name, verify colorization happens correctly only upto the correct size (118324)

    class TestClassXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX { }

    class TestClassXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX22 { }

    class yield
    {
        void Foo<U>(__arglist)
        {
            C<U> c = null;
            c.M<int>(5, default(U));
            TypedReference tr = __makeref(c);
            Type t = __reftype(tr);
            int j = __refvalue(tr);
            Params(t, t);
            Params(ref c, out c);
        }
        void Params(dynamic a, dynamic b, dynamic[] c) { }
        void Params(dynamic a, dynamic c, dynamic[][] c) { }

        public override string ToString()
        {
            return base.ToString();
        }

        public partial void OnError();

        public partial void method()
        {
            int?[] a = TODO Node ArrayCreationExpression;
            int[] var = TODO Node ArrayInitializerExpression;
            int i = a[i];
            Foo<T> f = new Foo<int>();
            f.method();
            i = i + i - i * i / i % i & i | i ^ i;
            bool b = true & false | true ^ false;
            b = !b;
            i = ~i;
            b = i < i && i > i;
            int? ii = 5;
            int f = true ? 1 : 0;
            i++;
            i--;
            b = true && false || true;
            i << 5;
            i >> 5;
            b = i == i && i != i && i <= i && i >= i;
            i += 5.0;
            i -= i;
            i *= i;

            i /= i;
            i %= i;
            i &= i;
            i |= i;
            i ^= i;
            i <<= i;
            i >>= i;
            object s = x => x + 1;
            double d = .3;
            Point point;
            unsafe
            {
                Point* p = &point;

                p->x = 10;
            }
            IO::BinaryReader br = null;
            x[1] = 3;
            x[1, 5] = "str";
        }

        struct Point
        {
            public int X;
            public int Y;
            public void ThisAccess()
            {
                this = this;
            }
        }
    }
    // From here:https://github.com/dotnet/roslyn/wiki/New-Language-Features-in-C%23-6

    class CSharp6Features
    {

        public string First
        {
            get;
            set;
        }
        public string Last { get; set; }

        public string Third
        {
            get;
        }
        public string Fourth { get; }

        public Point Move(int dx, int dy);
        TODO Node OperatorDeclaration
        TODO Node ConversionOperatorDeclaration
        public void Print();

        public string Name
        => First + " " + Last;
        TODO Node IndexerDeclaration

        async void Test()
        {

            WriteLine(Sqrt(3 * 3 + 4 * 4));
            WriteLine(Friday - Monday);
            var range = Range(5, 17);
            var even = range.Where(i => i % 2 == 0);

            int? length = customers?.Length;
            Customer first = customers?[0];
            int length = customers?.Length ?? 0;
            int? first = customers?[0]?.Orders?.Count();
            PropertyChanged?.Invoke(this, args);

            string s = $"{p.Name} is {p.Age} year{{s}} old #";
            s = $"{p.Name} is \"{p.Age} year{(p.Age == 1 ? "" : "s")} old";
            s = $"{(p.Age == 2 ? $"{new Person { }}" : "")}";
            s = $@"\{p.Name}
                                   ""\";
            s = $"Color [ R={func(3)}, G={G}, B={B}, A={A} ]";

            if (x == null)throw new ArgumentNullException(nameof(x));
            WriteLine(nameof(person.Address.ZipCode));

            var numbers = new Dictionary<int, string>
            {
                TODO Node ImplicitElementAccess = "seven",
                TODO Node ImplicitElementAccess = "nine",
                TODO Node ImplicitElementAccess = "thirteen"
            };

            try { }
            catch (MyExceptione) { }

            Resource res = null;
            try
            {
                res = await Resource.OpenAsync();
            }
            catch (ResourceExceptione)
            {
                await Resource.LogAsync(res, e);
            }
            finally
            {
                if (res != null)await res.CloseAsync();
            }
        }
    }
}

class CSharp70
{
    void PatternMatching(string arg, int b)
    {
        switch (arg)
        {
            TODO Node CasePatternSwitchLabel
            TODO Node CasePatternSwitchLabel
            default:break;
        }
        TODO Node TupleExpression = e;

        if (TODO Node IsPatternExpression) { }

        if (TODO Node IsPatternExpression)
        {
            Hello();
        }
    }

    public static async Task LocalFunctions(string[] args)
    {
        string Hello2(int i)
        {
            return args[i];
        }

        async Task<string> Hello<T>(T i);
        await Hello(1);
    }

    public static void OutVar(string[] args)
    {
        int.TryParse(Hello(1), out var item);
        int.TryParse(Hello(1), out int item);
    }

    public void ThrowExpression()
    {
        var result = nullableResult ?? TODO Node ThrowExpression;
    }

    public void BinaryLiterals()
    {
        int nineteen = 0b10011;
    }

    public void DigitSeparators()
    {
        int bin = 0b1001_1010_0001_0100;
        int hex = 0x1b_a0_44_fe;
        int dec = 33_554_432;
        int weird = 1_2__3___4____5_____6______7_______8________9;
        double real = 1_000.111_1e-1_000;
    }
}

class CSharp71
{
    void DefaultWithoutTypeName(string content)
    {
        DefaultWithoutTypeName(default);
    }

    void TupleRecognize(int a, TODO Node TupleType b, TODO Node TupleType? c)
    {
        var result = list.Select(c => TODO Node TupleExpression).Where(
            t => t.f2 == 1);
    }
}

class CSharp72
{
    readonly struct ReadonlyRef1
    {
        Func<int, int> s = () => x;
        TODO Node IndexerDeclaration
        TODO Node OperatorDeclaration

        static readonly TODO Node RefType M1_Trace()
        {
            TODO Node RefType r1 = TODO Node RefExpression;
            TODO Node RefType r2 = TODO Node RefExpression;

            Mutate(ref r1);

            Print(in r1);

            return TODO Node RefExpression;
        }
    }

    ref struct ReadonlyRef2
    {
        TODO Node RefType Test(Vector3 v1, Vector3 v2)
        {

            v1 = default(Vector3);

            v1.X = 0;

            foo(ref v1.X);

            return TODO Node RefExpression;

            Span<int> span = stackalloc int[1];

            return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }
        TODO Node RefType Choice(bool condition, T consequence, T alternative)
        {
            if (condition)
            {
                return TODO Node RefExpression;
            }
            else
            {
                return TODO Node RefExpression;
            }
        }
    }

    public void DoSomething(bool isEmployed, string personName, int personAge)
    { }

    public void NonTrailingNamedArguments()
    {
        DoSomething(true, name, age);
        DoSomething(true, name, age);
        DoSomething(name, true, age);
        DoSomething(name, age, true);
        DoSomething(true, age, name);
    }

    public void ConditionalRef()
    {
        TODO Node RefType r = TODO Node RefExpression;
    }

    public void LeadingSeparator()
    {
        var res = 0 + 123 + 1_2_3 + 0x1_2_3 + 0b101 + 0b1_0_1
        + 0x_1_2 + 0b_1_0_1;
    }
}

class CSharp73
{
    void Blittable<T>(T value)
    {
        var unmanaged = 666;
    }

    unsafe struct IndexingMovableFixed
    {
        public fixed int myFixedField;
    }

    static IndexingMovableFixed s;

    public unsafe void IndexingMovableFixedFields()
    {
        int* ptr = s.myFixedField;
        int t = s.myFixedField[5];
    }

    public void PatternBasedFixed()
    {
        fixed (byte* ptr = byteArray) { }
    }

    public void StackallocArrayInitializer()
    {
        Span<int> a = stackalloc int[3];
        Span<int> a = stackalloc int[3];
        Span<int> a = stackalloc int[];
        Span<int> a = TODO Node ImplicitStackAllocArrayCreationExpression;
    }

    public void TupleEquality()
    {
        TODO Node TupleType t1, t2;
        var res = t1 == TODO Node TupleExpression;
    }
}
