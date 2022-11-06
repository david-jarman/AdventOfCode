namespace problem_solving;

public class Snailfish
{
    public void Solve_Par1()
    {
        var pairs = File.ReadAllLines("day18/input.txt")
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(n => Pair.Parse(n));

        Pair summedPair = pairs.First();
        foreach (Pair pair in pairs.Skip(1))
        {
            summedPair = Pair.Add(summedPair, pair);

            summedPair.Reduce();
        }

        Console.WriteLine($"Final magnitude: {summedPair.Magnitude()}");
    }
}

public class Pair : Element
{
    public static Pair Parse(string s)
    {
        Stack<Pair> stack = new Stack<Pair>();
        Pair root = null;
        int i = 0;

        bool left = true;

        while (i < s.Length)
        {
            char c = s[i];
            if (c == '[')
            {
                i++;
                if (stack.Count == 0)
                {
                    root = new Pair();
                    stack.Push(root);
                }
                else
                {
                    var parent = stack.Peek();
                    var newPair = new Pair();

                    if (left)
                    {
                        parent.Left = newPair;
                        parent.Left.Parent = parent;
                    }
                    else
                    {
                        parent.Right = newPair;
                        parent.Right.Parent = parent;
                    }

                    stack.Push(newPair);
                }

                left = true;
            }
            else if (c >= '0' && c <='9')
            {
                int value = c - '0';
                i++;
                if (s[i] >= '0' && s[i] <= '9')
                {
                    value *= 10;
                    value += s[i] - '0';
                    i++;
                }

                var parent = stack.Pop();
                var newChild = new ValueElement(value);
                newChild.Parent = parent;

                if (left) parent.Left = newChild;
                else parent.Right = newChild;
                stack.Push(parent); // push it back onto the stack, and let the ']' pop it officially
            }
            else if (c == ']')
            {
                i++;
                stack.Pop();
            }
            else if (c == ',')
            {
                i++;
                left = false;
            }
        }

        return root;
    }

    public Element Left { get; set; }

    public Element Right { get; set; }

    public Pair(Element left, Element right)
    {
        Left = left;
        Right = right;
    }

    public Pair()
    {
        Left = null;
        Right = null;
    }

    private bool IsValuePair()
    {
        return Left is ValueElement && Right is ValueElement;
    }

    public void Reduce()
    {
        bool reduced = false;

        while (!reduced)
        {
            // If any pair is nested inside four pairs, the leftmost such pair explodes.
            var nestedPair = this.FindNestedPair();
            if (nestedPair != null)
            {
                nestedPair.Explode();
                continue;
            }

            // If any regular number is 10 or greater, the leftmost such regular number splits.
            ValueElement? valueG10 = this.FindLeftmostValueElementGreaterThan10();
            if (valueG10 != null)
            {
                valueG10.Split();
            }
            else
            {
                reduced = true;
            }
        }
    }

    public void Explode()
    {

    }

    public ValueElement? FindLeftmostValueElementGreaterThan10()
    {
        Stack<Element> s = new();

        s.Push(this.Right);
        s.Push(this.Left);

        while (s.Count > 0)
        {
            Element e = s.Pop();

            if (e is ValueElement value && value.Value >= 10)
            {
                return value;
            }
            else if (e is Pair p)
            {
                s.Push(p.Right);
                s.Push(p.Left);
            }
        }

        return null;
    }

    public Pair? FindNestedPair()
    {
        Stack<(Pair pair, int level)> s = new();

        if (this.Right is Pair right_pair)
        {
            s.Push((right_pair, 1));
        }
        if (this.Left is Pair left_pair)
        {
            s.Push((left_pair, 1));
        }

        while (s.Count > 0)
        {
            (var pair, int level) = s.Pop();

            if (level >= 4 && pair.IsValuePair())
            {
                return pair;
            }

            if (pair.Right is Pair right)
            {
                s.Push((right, level + 1));
            }
            if (pair.Left is Pair left)
            {
                s.Push((left, level + 1));
            }
        }

        return null;
    }

    public static Pair Add(Pair pair1, Pair pair2)
    {
        var newPair = new Pair(pair1, pair2);
        pair1.Parent = newPair;
        pair2.Parent = newPair;
        return newPair;
    }

    public override long Magnitude()
    {
        return this.Left.Magnitude() * 3 + this.Right.Magnitude() * 2;
    }

    public override string ToString()
    {
        return $"[{Left},{Right}]";
    }
}

public class ValueElement : Element
{
    public int Value { get; set; }

    public ValueElement(int value)
    {
        Value = value;
    }

    public void Split()
    {

    }

    public ValueElement? GetValueElementLeft()
    {
        // algo: cursor upwards in the parents until your previous parent is not the left item of the next parent
        // At that point, go left in the pair, then recurse downards right until you hit a value element

        Pair? parentCursor = this.Parent;
        while (parentCursor != null && parentCursor == parentCursor.Parent?.Left)
        {
            parentCursor = parentCursor.Parent;
        }

        // at this point, parent is at the pair where the previous parent was the "right" child
        // take the left element of this parent, and go to the rightmost value in it
        Element? element = parentCursor?.Left;
        while (element != null && element is Pair pairElement)
        {
            element = pairElement.Right;
        }

        if (element is ValueElement valueElement)
        {
            return valueElement;
        }

        return null;
    }

    public override string ToString()
    {
        return $"{Value}";
    }

    public override long Magnitude()
    {
        return this.Value;
    }
}

public abstract class Element
{
    public Pair? Parent { get; set; }

    public abstract long Magnitude();
}