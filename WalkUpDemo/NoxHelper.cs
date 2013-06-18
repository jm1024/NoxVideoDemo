using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

static class NoxHelper
{
    public static string getNoxData(string host, string op, bool xmlOutput, params NoxApiParameter[] parameters)
    {
        StringBuilder uri = new StringBuilder();
        uri.Append("http://" + host + "/api.aspx?op=" + op);

        foreach (NoxApiParameter parameter in parameters)
        {
            uri.Append("&" + parameter.ParameterName + "=" + parameter.ParmeterValue);
        }

        if (xmlOutput)
            uri.Append("&output=xml");

        WebRequest req = HttpWebRequest.Create(uri.ToString());
        req.Method = "GET";

        string source = "";
        try
        {
            using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream()))
            {
                source = reader.ReadToEnd();
            }
        }
        catch { }

        return source;
    }

    public static List<Dictionary<string, string>> parseNoxData(string noxData)
    {
        List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();
        string[] dataSplit = noxData.Split(Environment.NewLine.ToCharArray());

        Dictionary<string, string> dataRecord = new Dictionary<string, string>();
        string firstElement = "";
        foreach (string line in dataSplit)
        {
            string name = line.Substring(0, line.IndexOf("|"));
            string value = line.Substring(line.IndexOf("|") + 1);
            if (firstElement == "")
                firstElement = name;
            else if (firstElement == name)
            {
                dataList.Add(dataRecord);
                dataRecord = new Dictionary<string, string>();
            }
            dataRecord.Add(name, value);
        }

        return dataList;
    }
}

public class NoxApiParameter
{
    private string m_parameterName = "";
    private string m_parameterValue = "";

    public string ParameterName
    {
        get { return m_parameterName; }
        set { m_parameterName = value; }
    }

    public string ParmeterValue
    {
        get { return m_parameterValue; }
        set { m_parameterValue = value; }
    }

    public NoxApiParameter(string name, string value)
    {
        m_parameterName = name;
        m_parameterValue = value;
    }
}

