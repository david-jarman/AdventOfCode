namespace problem_solving;

public class Snailfish
{
    public void Solve_Par1()
    {
        // var pairs = File.ReadAllLines("day18/input.txt")
        //     .Where(l => !string.IsNullOrWhiteSpace(l))
        //     .Select(n => Pair.Parse(n));

        // Pair summedPair = pairs.First();
        // foreach (Pair pair in pairs.Skip(1))
        // {
        //     summedPair = Pair.Add(summedPair, pair);

        //     summedPair.Reduce();
        // }

        // Console.WriteLine($"Final magnitude: {summedPair.Magnitude()}");

        //string testExplode = "[[[[[9,8],1],2],3],4]";
        string testExplode = "[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]";
        string afterReduce = "[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]";

        var pair = Pair.Parse(testExplode);

        pair.Reduce();
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
        if (this.Left is not ValueElement valueLeft)
        {
            throw new InvalidOperationException("Only value pairs can be exploded");
        }

        var valueLeftOfLeft = valueLeft.GetValueElementLeft();
        if (valueLeftOfLeft != null)
        {
            valueLeftOfLeft.Value += valueLeft.Value;
        }

        if (this.Right is not ValueElement valueRight)
        {
            throw new InvalidOperationException("Only value pairs can be exploded");
        }

        // Do right as well
        var valueRightOfRight = valueRight.GetValueElementRight();
        if (valueRightOfRight != null)
        {
            valueRightOfRight.Value += valueRight.Value;
        }

        // Replace self with a 0 in the parent pair
        // Am I left or right, dad?
        if (this.Parent?.Left == this)
        {
            // I'm left
            this.Parent.Left = new ValueElement(0);
        }
        else if (this.Parent?.Right == this)
        {
            // I'm  right
            this.Parent.Right = new ValueElement(0);
        }
        else
        {
            throw new InvalidOperationException("Am I adopted?");
        }

        // :( goodbye world
        this.Parent = null;
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
        // Cursor up the parent chain until we find that we are not the "left" child of our parent.
        // Then cursor over to the parents left child. Their rightmost value is our brother from another mother

        Pair? cursor = this.Parent;

        while (cursor != null && cursor == cursor.Parent?.Left)
        {
            cursor = cursor.Parent;
        }

        if (cursor == null)
        {
            return null;
        }

        Element? element = cursor.Parent?.Left;
        while (element is Pair pairElement)
        {
            element = pairElement.Right;
        }

        if (element is ValueElement valueElement)
        {
            return valueElement;
        }

        return null;
    }

    public ValueElement? GetValueElementRight()
    {
        // Cursor up the parent chain until we find that we are not the "right" child of our parent.
        // Then cursor over to the parents right child. Their leftmost value is our brother from another mother

        Pair? cursor = this.Parent;

        while (cursor != null && cursor == cursor.Parent?.Right)
        {
            cursor = cursor.Parent;
        }

        if (cursor == null)
        {
            return null;
        }

        Element? element = cursor.Parent?.Right;
        while (element is Pair pairElement)
        {
            element = pairElement.Left;
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