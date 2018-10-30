using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;
using Newtonsoft.Json;
using System.IO;


class SquareMatrixHelper
{
    public static List<string[]> GetMatrixData(string inputMatrix, string delimiter)
    {
        List<string> matrixLines = new List<string>();
        string matrixLine = "";
        for (int i = 0; i < inputMatrix.Length; i++)
        {
            if ((inputMatrix[i] == '\r' || inputMatrix[i] == '\n') && matrixLine != "")
            {
                matrixLines.Add(matrixLine);
                matrixLine = "";

            }
            else if (inputMatrix[i] != '\r' && inputMatrix[i] != '\n')
            {
                matrixLine = matrixLine + inputMatrix[i];
                if (i == inputMatrix.Length - 1)
                {
                    matrixLines.Add(matrixLine);
                }
            }
        }

        List<string[]> matrixData = new List<string[]>();
        foreach (string line in matrixLines)
        {
            string[] lineData = line.Split(new string[] { delimiter }, StringSplitOptions.None);
            matrixData.Add(lineData);
        }

        bool matrixHat = true;
        matrixData = CutOffData(matrixData, matrixHat);

        return matrixData;
    }

    public static string MatrixToJson(List<string[]> matrixData, string fileName)
    {
        List<MatrixJson> matrixJson = new List<MatrixJson>();
        List<ConnectionsJson> connections = new List<ConnectionsJson>();

        for (int i = 0; i < matrixData.Count; i++)
        {
            for (int j = 0; j < matrixData.Count; j++)
            {
                connections.Add(new ConnectionsJson { from = i, to = j, value = Helper.ConvertToDouble(matrixData[i][j]) });
            }

        }

        matrixJson.Add(new MatrixJson { n = matrixData.Count, name = fileName, connections = connections });

        return JsonConvert.SerializeObject(matrixJson);
    }

    public static List<ConnectionsJson> MatrixConnections(List<string[]> matrixData)
    {
        List<ConnectionsJson> connections = new List<ConnectionsJson>();
        for (int i = 0; i < matrixData.Count; i++)
        {
            for (int j = 0; j < matrixData.Count; j++)
            {
                connections.Add(new ConnectionsJson { from = i, to = j, value = Helper.ConvertToDouble(matrixData[i][j]) });
            }
        }

        return connections;
    }

    static List<string[]> CutOffData(List<string[]> matrixData, bool matrixHat)
    {
        int[] numbersCount = new int[matrixData.Count];
        for (int i = 0; i < matrixData.Count; i++)
        {
            numbersCount[i] = matrixData[i].Length;
        }

        if (matrixHat)
        {
            List<string> matrixHat = GetMatrixHat(matrixData);
        }

        int maxNumers = numbersCount.Max();
        List<int> dataToRemove = new List<int>();
        var groups = numbersCount.GroupBy(i => i);
        foreach (var group in groups)
        {
            if (group.Key < maxNumers || group.Key == 1)
            {
                for (int i = 0; i < numbersCount.Length; i++)
                {
                    if (numbersCount[i] == group.Key)
                    {
                        dataToRemove.Add(i);
                    }
                }
            }
        }
        int offset = 0;
        foreach (int index in dataToRemove)
        {
            matrixData.RemoveAt(index - offset);
            offset = offset + 1;
        }

        return matrixData;
    }

    static List<string> GetMatrixHat(List<string[]> matrixData)
    {
        List<string> hatVertical = new List<string>();
        List<string> hatHorizontal = new List<string>();
        for (int i = 0; i < matrixData[0].Length; i++)
        {
            if (matrixData[0][i] != "" && matrixData[0][i] != null)
            {
                hatVertical.Add(matrixData[0][i]);
            }
        }

        for (int i = 0; i < matrixData.Count; i++)
        {
            if (matrixData[i][0] != "" && matrixData[i][0] != null)
            {
                hatHorizontal.Add(matrixData[i][0]);
            }
        }

        if (hatHorizontal.Count == hatVertical.Count)
        {
            for (int i = 0; i < hatVertical.Count; i++)
            {
                if (hatVertical[i] != hatHorizontal[i])
                {
                    throw new Exception("Matrix Hat Is Not Valid!");
                }
            }
        } else {
             throw new Exception("Matrix Hat Is Not Valid!");
        }

        return hatHorizontal;
    }

    static List<char> FindAllDelimiters(string inputMatrix)
    {
        List<char> delimiters = new List<char>();

        for (int i = 0; i < inputMatrix.Length; i++)
        {
            if (!Char.IsDigit(inputMatrix[i]) && inputMatrix[i] != '-')
            {
                delimiters.Add(inputMatrix[i]);
            }

        }

        return delimiters;
    }

    public static string GetDelimiter(char numberSeparator, string inputMatrix)
    {
        string delimiter = "";

        List<string> numbers = new List<string>();
        List<string> delimiters = new List<string>();

        for (int i = 0; i < inputMatrix.Length; i++)
        {
            if (!Char.IsDigit(inputMatrix[i]) && inputMatrix[i] != '-' && inputMatrix[i] != numberSeparator)
            {
                delimiter = delimiter + inputMatrix[i];
            }
            else
            {
                if (delimiter != "")
                {
                    delimiters.Add(delimiter);
                    delimiter = "";
                }
            }
        }

        string mostPopularDelimiter = SelectPopularDelimiter(delimiters);

        return mostPopularDelimiter;
    }

    public static List<char> FindNumberSeparators(string inputMatrix)
    {
        List<char> separators = new List<char>();

        for (int i = 0; i < inputMatrix.Length - 2; i++)
        {
            if (Char.IsDigit(inputMatrix[i]) && Char.IsDigit(inputMatrix[i + 2]))
            {
                if (inputMatrix[i + 1] == '.' || inputMatrix[i + 1] == ',')
                {
                    separators.Add(inputMatrix[i + 1]);
                }
            }

        }

        return separators;
    }

    static List<GroupChar> getGroupsChar(List<char> list)
    {
        var numberGroups = list.GroupBy(i => i);

        List<GroupChar> groups = new List<GroupChar>();

        foreach (var grp in numberGroups)
        {
            GroupChar group = new GroupChar();
            group.Key = grp.Key;
            group.Count = grp.Count();

            groups.Add(group);
        }

        return groups;
    }

    static List<GroupString> getGroupsString(List<string> list)
    {
        var numberGroups = list.GroupBy(i => i);

        List<GroupString> groups = new List<GroupString>();

        foreach (var grp in numberGroups)
        {
            GroupString group = new GroupString();
            group.Value = grp.Key;
            group.Count = grp.Count();

            groups.Add(group);
        }

        return groups;
    }

    static string SelectPopularDelimiter(List<string> delimiters)
    {
        List<GroupString> delimiterGroups = getGroupsString(delimiters);
        GroupString mostPopularDelimiter = delimiterGroups.MaxBy(t => t.Count);

        return mostPopularDelimiter.Value;
    }

    public static char SelectPopularSeparator(List<char> separators)
    {
        if (separators.Count > 0)
        {
            List<GroupChar> separatorGroups = getGroupsChar(separators);
            GroupChar mostPopularSeparator = separatorGroups.MaxBy(t => t.Count);
            return mostPopularSeparator.Key;
        }
        else
        {
            return '.';
        }
    }

    public static int GetStartLine(List<string[]> matrixData, string delimiter, string inputMatrix)
    {
        string firstStringToFind = "";
        for (int i = 0; i < matrixData[0].Length; i++)
        {
            if (i == 0)
            {
                firstStringToFind += matrixData[0][i];
            }
            else
            {
                firstStringToFind += delimiter + matrixData[0][i];
            }
        }

        List<string> matrixLines = new List<string>();
        string matrixLine = "";
        for (int i = 0; i < inputMatrix.Length; i++)
        {
            if ((inputMatrix[i] == '\r' || inputMatrix[i] == '\n') && matrixLine != "")
            {
                matrixLines.Add(matrixLine);
                matrixLine = "";

            }
            else if (inputMatrix[i] != '\r' && inputMatrix[i] != '\n')
            {
                matrixLine = matrixLine + inputMatrix[i];
            }
        }

        int startLine = 0;

        foreach (string line in matrixLines)
        {
            if (line.IndexOf(firstStringToFind) >= 0)
            {
                break;
            }
            else
            {
                startLine++;
            }
        }

        return startLine;
    }
}

public class GroupChar
{
    public int ID { get; set; }
    public char Key;
    public int Count;
}

public class GroupString
{
    public int ID { get; set; }
    public string Value;
    public int Count;
}

public class FullMatrixData
{
    public char separator;
    public string delimiter;
    public List<string[]> matrixData;
    public string matrixJson;
    public int startLine;
}

public class MatrixJson
{
    [JsonProperty("name")]
    public string name;
    [JsonProperty("n")]
    public int n;
    [JsonProperty("connections")]
    public List<ConnectionsJson> connections;
}

public class ConnectionsJson
{
    public int from;
    public int to;
    public double value;
}


