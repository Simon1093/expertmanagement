using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;
using Newtonsoft.Json;

public class InputMatrixParser
{
    public static List<PrimaryGraph.Verticle> ParseSquareMatrix(string inputMatrix, string fileName)
    {
        List<char> separators = SquareMatrixHelper.FindNumberSeparators(inputMatrix);
        char separator = SquareMatrixHelper.SelectPopularSeparator(separators);
        string delimiter = SquareMatrixHelper.GetDelimiter(separator, inputMatrix);
        List<string[]> matrixData = SquareMatrixHelper.GetMatrixData(inputMatrix, delimiter);
        int startLine = SquareMatrixHelper.GetStartLine(matrixData, delimiter, inputMatrix);
        List<ConnectionsJson> connections = SquareMatrixHelper.MatrixConnections(matrixData);

        List<PrimaryGraph.Verticle> graph = new List<PrimaryGraph.Verticle>();
        var numberGroups = connections.GroupBy(i => i.from);

        foreach (var grp in numberGroups)
        {
            PrimaryGraph.Verticle v = new PrimaryGraph.Verticle();
            v.verticle_id = (grp.Key + 1).ToString();
            foreach (ConnectionsJson connection in connections)
            {
                PrimaryGraph.Connections c = new PrimaryGraph.Connections();
                List<PrimaryGraph.Connections> cons = new List<PrimaryGraph.Connections>();
                c.connectedTo = connection.to.ToString();
                c.strength = connection.value;
                cons.Add(c);
                v.connections = cons;
            }

            graph.Add(v);
        }



        //List<FullMatrixData> fullMatrixData = new List<FullMatrixData>();
        //string jsonMatrix = SquareMatrixHelper.MatrixToJson(matrixData, fileName); 
        //fullMatrixData.Add(new FullMatrixData { delimiter = delimiter, separator = separator, matrixData = matrixData, matrixJson = jsonMatrix, startLine = startLine });

        return graph;
    }

    public static List<FullMatrixData> ParseLineMatrix(string inputMatrix, string fileName)
    {
        List<char> separators = LineMatrixHelper.FindNumberSeparators(inputMatrix);
        char separator = LineMatrixHelper.SelectPopularSeparator(separators);
        string delimiter = LineMatrixHelper.GetDelimiter(separator, inputMatrix);
        List<string[]> matrixData = LineMatrixHelper.GetMatrixData(inputMatrix, delimiter);
        int startLine = LineMatrixHelper.GetStartLine(matrixData, delimiter, inputMatrix);

        List<FullMatrixData> fullMatrixData = new List<FullMatrixData>();
        string jsonMatrix = LineMatrixHelper.MatrixToJson(matrixData, fileName);

        fullMatrixData.Add(new FullMatrixData { delimiter = delimiter, separator = separator, matrixData = matrixData, matrixJson = jsonMatrix, startLine = startLine });

        return fullMatrixData;
    }

    public static List<FullMatrixData> ParseColumnMatrix(string inputMatrix, string fileName)
    {
        List<char> separators = ColumnMatrixHelper.FindNumberSeparators(inputMatrix);
        char separator = ColumnMatrixHelper.SelectPopularSeparator(separators);
        string delimiter = ColumnMatrixHelper.GetDelimiter(separator, inputMatrix);
        List<string[]> matrixData = ColumnMatrixHelper.GetMatrixData(inputMatrix, delimiter);
        int startLine = ColumnMatrixHelper.GetStartLine(matrixData, delimiter, inputMatrix);

        List<FullMatrixData> fullMatrixData = new List<FullMatrixData>();
        string jsonMatrix = ColumnMatrixHelper.MatrixToJson(matrixData, fileName);

        fullMatrixData.Add(new FullMatrixData { delimiter = delimiter, separator = separator, matrixData = matrixData, matrixJson = jsonMatrix, startLine = startLine });

        return fullMatrixData;
    }
}

