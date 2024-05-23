namespace WebAPI_Apollo.Model
{
    public class Story
    {
        private int _id;
        private long _tempoPermanencia;
        // Estudar possibilidades para mudar para DateTime ou outra classe mais indicada

        public Story()
        {
            Id = 0;
            _tempoPermanencia = 0;
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public long TempoPermanencia
        {
            get { return _tempoPermanencia; }
            set { _tempoPermanencia = value; }
        }
    }
}
