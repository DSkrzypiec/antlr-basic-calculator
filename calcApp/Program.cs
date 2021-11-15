using System.IO;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

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

            var walker = new ParseTreeWalker();
            walker.Walk(new Evaluator(), tree);
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

public class Evaluator : CalcBaseListener {
    private Stack<int> _stack = new Stack<int>();

    public override void ExitProg(CalcParser.ProgContext context) {
        var res = _stack.Pop();
        Console.WriteLine($"Listner calculated: {res}");
    }

    public override void ExitInt(CalcParser.IntContext context) {
        var val = int.Parse(context.INT().GetText());
        _stack.Push(val);
    }

    public override void ExitAdd(CalcParser.AddContext context) {
        var right = _stack.Pop();
        var left = _stack.Pop();
        _stack.Push(left + right);
    }

    public override void ExitMul(CalcParser.MulContext context) {
        var right = _stack.Pop();
        var left = _stack.Pop();
        _stack.Push(left * right);
    }

}

