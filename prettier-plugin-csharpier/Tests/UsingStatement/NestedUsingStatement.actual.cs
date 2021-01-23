class ClassName
{
    void MethodName()
    {
        using (var x = BeginScope())
        using (var y = BeginScope())
        {
            return;
        }
    }
}
