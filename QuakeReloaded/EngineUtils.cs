namespace QuakeReloaded;

internal static class EngineUtils
{
    public static int QCGetReturnStatementIndex(QuakeEngine engine)
    {
        unsafe
        {
            int runaway = 100000;
            int index = 0;
            while (runaway-- > 0)
            {
                var statement = engine.QCGetStatement(index++);
                if (statement->op == 43) // OP_RETURN
                    return index;
            }
        }

        return 0;
    }
}
