using System.IO;
using Antlr4.Runtime;

public static class Program {
    public static void Main(string[] args) {
        if (args.Length < 1) {
            Console.WriteLine("Need an input");
            return;
        }
        var input = args[1];
        Console.WriteLine(input);

        using (TextReader stream = new StringReader(input)) {
            var lexer = new CalcLexer(new AntlrInputStream(stream));
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new CalcParser(tokenStream);
            var tree = parser.prog();

            var visitor = new EvalVisitor();
            int result = visitor.Visit(tree);
            Console.WriteLine($"Result = {result}");
        }
    }
}

public class EvalVisitor : CalcBaseVisitor<int> {
    public override int VisitAdd(CalcParser.AddContext ctx) {
        return Visit(ctx.expr(0)) + Visit(ctx.expr(1));
    }

    public override int VisitMul(CalcParser.MulContext ctx) {
        return Visit(ctx.expr(0)) * Visit(ctx.expr(1));
    }

    public override int VisitInt(CalcParser.IntContext ctx) {
        return int.Parse(ctx.INT().GetText());
    }
}

