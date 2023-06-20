namespace CommentService.Services.EncryptDecryptData
{
    public class EncryptDecryptData: IEncryptDecryptData
    {
        public string EncryptDataToBase64(string data)
        {
            if (data == null)
                return null;

            var encryptDataByte = System.Text.ASCIIEncoding.UTF8.GetBytes(data);

            return Convert.ToBase64String(encryptDataByte);
        }

        public string DecryptDataFromBase64(string dataBase64) 
        {
            if (dataBase64 == null)
                return null;

            var bytes = System.Convert.FromBase64String(dataBase64);

            return System.Text.Encoding.UTF8.GetString(bytes);

        }
    }
}
