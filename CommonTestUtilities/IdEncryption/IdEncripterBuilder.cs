using Sqids;

namespace CommonTestUtilities.IdEncryption;

public class IdEncripterBuilder
{
    public static SqidsEncoder<int> Build()
    {
        return new SqidsEncoder<int>(new ()
        {
         MinLength = 3,
         Alphabet = "z3ZdxGJh7OjPRE1AIm58yHFwrgSv4ncULMWQNDa9XtCVo2eYfTKpliku6bqsB"
        });

    }
}