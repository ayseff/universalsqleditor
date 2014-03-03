namespace SqlEditor.SqlParser.Entities
{
    public enum TransactionDescriptor
    {
        None,
        Tran,
        Transaction,
        Work
    }

    public abstract class TransactionStatement : CustomStatement
    {
        public TransactionStatement()
        {
            Descriptor = TransactionDescriptor.None;
        }

        public TransactionDescriptor Descriptor { get; set; }
        public string Name { get; set; }
    }

    public class BeginTransactionStatement : TransactionStatement
    {
        public bool Distributed { get; set; }
    }

    public class CommitTransactionStatement : TransactionStatement
    {
    }

    public class RollbackTransactionStatement : TransactionStatement
    {
    }
}