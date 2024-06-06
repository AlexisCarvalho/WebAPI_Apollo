using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_Apollo.Domain.Model
{
    [Table("TestTable")]
    public class TestTable
    {
        int _id;
        string _nome;
        DateTime _criadaEm;

        public TestTable()
        {
            _id = 0;
            _nome = "WRITE SUCCESS";
            _criadaEm = DateTime.Now;
        }

        [Key]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }

        public DateTime CriadaEm
        {
            get { return _criadaEm; }
            set { _criadaEm = value; }
        }
    }
}
