using System;
using System.Collections.Generic;
using System.ComponentModel;
using Utilities.Collections;

namespace SqlEditor.SqlHistory
{
    public class SqlHistoryList : BindingList<ExecutedSqlStatement>
    {
        public bool HasChanged { get; private set; }

        public void Initialize(IEnumerable<ExecutedSqlStatement> statements)
        {
            this.Clear();
            this.AddRange(statements);
            HasChanged = false;
        }
        
        public void InsertAtBeginning(string statement, string connectionName, DateTime runDateTime)
        {
            if (statement == null) throw new ArgumentNullException("statement");
            if (connectionName == null) throw new ArgumentNullException("connectionName");
            var st = new ExecutedSqlStatement { SqlStatement = statement, RunDateTime = runDateTime, ConnectionName = connectionName };
            InsertAtBeginning(st);
        }

        public void InsertAtBeginning(ExecutedSqlStatement executedSql)
        {
            if (executedSql == null) throw new ArgumentNullException("executedSql");
            Insert(0, executedSql);
        }

        protected override void OnListChanged(ListChangedEventArgs e)
        {
            HasChanged = true;
            base.OnListChanged(e);
        }
    }
}
