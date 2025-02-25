namespace CSharpier.DocTypes;

internal abstract partial class Concat
{
    internal sealed class WithTwoChildren(Doc child0, Doc child1) : Concat
    {
        private Doc _child0 = child0;
        private Doc _child1 = child1;
        public override int Count => 2;

        public override Doc this[int index]
        {
            get =>
                index switch
                {
                    0 => _child0,
                    1 => _child1,
                    _ => throw new IndexOutOfRangeException(nameof(index)),
                };
            set
            {
                switch (index)
                {
                    case 0:
                        _child0 = value;
                        break;
                    case 1:
                        _child1 = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException(nameof(index));
                }
            }
        }

        public override void RemoveAt(int index)
        {
            switch (index)
            {
                case 0:
                    _child0 = _child1;
                    _child1 = Doc.Null;
                    break;
                case 1:
                    _child1 = Doc.Null;
                    break;
                default:
                    throw new IndexOutOfRangeException(nameof(index));
            }
        }
    }
}
