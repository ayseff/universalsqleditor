using System.Collections.Generic;
using SqlEditor.SqlParser.Entities;

namespace SqlEditor.SqlParser.Interfaces
{
    public interface IStatement
    {
        bool Terminated { get; set; }
        bool ParsedCompletely { get; set; }
        string Identifier { get; }
        List<Table> Tables { get; }
    }
}