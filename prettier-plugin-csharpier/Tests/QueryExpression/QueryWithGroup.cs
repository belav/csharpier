class ClassName
{
    void MethodName()
    {
        var query = from c in customers group c by c.Country;
    }
}
