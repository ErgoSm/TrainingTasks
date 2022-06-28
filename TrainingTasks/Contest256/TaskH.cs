var t = Convert.ToInt32(Console.ReadLine());

List<string> field;
Dictionary<int, Dictionary<int, int>> occupiedFields;
List<int> ships;
List<Corner> corners;
string[] input;
int n, m;
string result = "YES";


for (int k = 0; k < t; k++)
{
    result = "YES";
    input = Console.ReadLine().Split(' ');
    n = Convert.ToInt16(input[0]);
    m = Convert.ToInt16(input[1]);

    occupiedFields = new Dictionary<int, Dictionary<int, int>>(n);
    ships = new List<int>();
    for (int i = 0; i < n; i++)
    {
        occupiedFields.Add(i, new Dictionary<int, int>(m));
        for (int j = 0; j < m; j++)
            occupiedFields[i].Add(j, 0);
    }

    corners = new List<Corner>();
    field = new List<string>();
    for (int i = 0; i < n; i++)
        field.Add(Console.ReadLine());

    if (n == m && n == 1)
    {
        Console.WriteLine("YES");
        Console.WriteLine(1);
        continue;
    }

    //find corners and orientation
    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < m; j++)
        {
            var check = CheckCorner(i, j);
            if (check < 0)
            {
                result = "NO";
                break;
            }
            else if (check > 0)
            {
                var newCorner = new Corner(j, i, check);
                corners.Add(newCorner);
                //occupiedFields[i][j] = 1;
                if (!FillAround(newCorner))
                {
                    result = "NO";
                    break;
                }
            }

        }

        if (result == "NO")
            break;
    }

    if (result == "NO")
    {
        Console.WriteLine(result);
        continue;
    }

    //ships fillng
    foreach (var corner in corners)
    {
        if (corner.rotate == 1000)
        {
            ships.Add(1);
        }
        else
        {
            var decks = CountDecks(corner.y, corner.x, corner.rotate);
            if (decks < 0)
            {
                result = "NO";
                break;
            }

            ships.Add(decks);
        }
    }

    var numberOfOccupied = 0;
    foreach (var row in occupiedFields.Values)
        numberOfOccupied += row.Values.Where(x => x == 1).Count();

    if (ships.Sum() != numberOfOccupied)
        result = "NO";

    Console.WriteLine(result);
    if (result == "YES")
    {
        ships.Sort();
        Console.WriteLine(string.Join(' ', ships));
    }
}

bool FillAround(Corner corner)
{
    var result = true;

    if (corner.rotate == 1000)
    {
        result &= CheckAndFill(corner.x - 1, corner.y - 1);
        result &= CheckAndFill(corner.x - 1, corner.y);
        result &= CheckAndFill(corner.x - 1, corner.y + 1);
        result &= CheckAndFill(corner.x, corner.y + 1);
        result &= CheckAndFill(corner.x + 1, corner.y + 1);
        result &= CheckAndFill(corner.x + 1, corner.y);
        result &= CheckAndFill(corner.x + 1, corner.y - 1);
        result &= CheckAndFill(corner.x, corner.y - 1);
    }
    else if (corner.rotate == 360)
    {
        result &= CheckAndFill(corner.x, corner.y - 1);
        result &= CheckAndFill(corner.x - 1, corner.y - 1);
        result &= CheckAndFill(corner.x - 1, corner.y);
    }
    else if (corner.rotate == 90)
    {
        result &= CheckAndFill(corner.x, corner.y - 1);
        result &= CheckAndFill(corner.x + 1, corner.y - 1);
        result &= CheckAndFill(corner.x + 1, corner.y);
    }
    else if (corner.rotate == 180)
    {
        result &= CheckAndFill(corner.x + 1, corner.y);
        result &= CheckAndFill(corner.x + 1, corner.y + 1);
        result &= CheckAndFill(corner.x, corner.y + 1);
    }
    else if (corner.rotate == 270)
    {
        result &= CheckAndFill(corner.x - 1, corner.y);
        result &= CheckAndFill(corner.x - 1, corner.y + 1);
        result &= CheckAndFill(corner.x, corner.y + 1);
    }

    return result;
}

bool CheckAndFill(int x, int y)
{
    if (x >= 0 && x < m && y >= 0 && y < n)
    {
        if (occupiedFields[y][x] == 1)
            return false;

        occupiedFields[y][x] = 2;
    }

    return true;
}

void FillOccupied(int x, int y, int value)
{
    if (x >= 0 && x < m && y >= 0 && y < n)
        occupiedFields[y][x] = value;
}

int CheckCorner(int y, int x)
{
    bool current = field[y][x] == '*';
    FillOccupied(x, y, current ? 1 : 0);

    var left = x > 0 && field[y][x - 1] == '*';
    FillOccupied(x - 1, y, left ? 1 : 0);

    var right = (x < m - 1) && field[y][x + 1] == '*';
    FillOccupied(x + 1, y, right ? 1 : 0);

    var up = y > 0 && field[y - 1][x] == '*';
    FillOccupied(x, y - 1, up ? 1 : 0);

    var bottom = (y < n - 1) && field[y + 1][x] == '*';
    FillOccupied(x, y + 1, bottom ? 1 : 0);


    if (current)
    {
        var cnt = left ? 1 : 0;
        cnt += right ? 1 : 0;
        cnt += up ? 1 : 0;
        cnt += bottom ? 1 : 0;

        if (cnt > 2)
            return -1;

        if (bottom && right && !left && !up)
            return 360;
        else if (bottom && left && !right && !up)
            return 90;
        else if (up && left && !right && !bottom)
            return 180;
        else if (up && right && !left && !bottom)
            return 270;
        else if (!up && !bottom && !left && !right)
            return 1000; //one-deck ship
        else
            return 0;
    }
    else
        return 0;
}

int CountDecks(int yStart, int xStart, int rotation)
{
    var result = true;

    var xStep = rotation == 360 || rotation == 270 ? 1 : -1;
    var yStep = rotation == 90 || rotation == 360 ? 1 : -1;
    var counter = 1;

    bool xDeck = true;
    bool yDeck = true;

    int nextX = 0;
    int nextY = 0;

    while (xDeck && yDeck)
    {
        nextX = xStart + xStep * counter;
        xDeck = nextX >= 0 && nextX < m && field[yStart][nextX] == '*';
        nextY = yStart + yStep * counter;
        yDeck = nextY >= 0 && nextY < n && field[nextY][xStart] == '*';

        if (xDeck)
        {
            //occupiedFields[yStart][nextX] = 1;
            result &= CheckAndFill(nextX, yStart + 1);
            result &= CheckAndFill(nextX, yStart - 1);
        }

        if (yDeck)
        {
            //occupiedFields[nextY][xStart] = 1;
            result &= CheckAndFill(xStart - 1, nextY);
            result &= CheckAndFill(xStart + 1, nextY);
        }

        counter++;
    }

    if (counter > 5)
        return -1;

    result &= CheckAndFill(nextX, yStart + 1);
    result &= CheckAndFill(nextX, yStart);
    result &= CheckAndFill(nextX, yStart - 1);
    result &= CheckAndFill(xStart - 1, nextY);
    result &= CheckAndFill(xStart, nextY);
    result &= CheckAndFill(xStart + 1, nextY);


    return result && !xDeck && !yDeck ? (counter - 1) * 2 - 1 : -1;
}

public record Corner(int x, int y, int rotate);