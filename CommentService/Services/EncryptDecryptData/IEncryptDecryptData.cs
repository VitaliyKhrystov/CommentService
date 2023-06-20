namespace CommentService.Services.EncryptDecryptData
{
    public interface IEncryptDecryptData
    {
        string EncryptDataToBase64(string data);
        string DecryptDataFromBase64(string dataBase64);
    }
}
