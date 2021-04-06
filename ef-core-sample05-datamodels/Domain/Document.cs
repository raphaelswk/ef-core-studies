namespace MasteringEFCore.Domain
{
    public class Document
    {
        private string _pps;

        public int Id { get; set; }

        public void SetPPS(string pps)
        {
            // validations
            if (string.IsNullOrEmpty(pps))
            {
                throw new System.Exception("Invalid PPS");
            }

            _pps = pps;
        }

        public string GetPPS()
        {
            return _pps;
        }
    }
}