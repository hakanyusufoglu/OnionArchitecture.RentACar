namespace Core.CrossCuttingConcerns.Exceptions.Types
{
    //Bu yapı sayesinde name, age gibi propertyde birden fazla hata olabileceği ve bunu kullanıcıya sunabileceği iç içe bir yapı kuruldu.
    public class ValidationException : Exception
    {
        //Hatalar liste şeklinde kullanıcıya verilmesi gerekir.
        public IEnumerable<ValidationExceptionModel> Errors { get; }
        //Hata olmayabilir bu yüzden boş geçilecek
        public ValidationException(string? message) : base(message)
        {
            //Boş bir liste oluştur
            Errors = Array.Empty<ValidationExceptionModel>();
        }
        public ValidationException(string? message, Exception? innerException):base(message, innerException)
        {
            Errors=Array.Empty<ValidationExceptionModel>();
        }
        //Hata olabilir ve bu yüzden hataları doldur
        public ValidationException(IEnumerable<ValidationExceptionModel> errors):base(BuildErrorMessage(errors))
        {
            Errors = errors;
        }
        //İlgili alanın hata aldığına dair hatayı güzel bir formatta verilmesi için bu metot gerçekleştirildi.
        private static string BuildErrorMessage(IEnumerable<ValidationExceptionModel> errors)
        {
            // Hata mesajlarını oluşturan bir metot. Parametre olarak bir IEnumerable<ValidationExceptionModel> alır.

            // IEnumerable<string> tipinde bir dizi oluşturuyoruz. Bu dizi, hata mesajlarını içerecektir.
            IEnumerable<string> arr = errors.Select(
                x => $"{Environment.NewLine} -- {x.Property}: {string.Join(Environment.NewLine, values: x.Errors ?? Array.Empty<string>())}"
            );

            // Hata mesajlarını birleştirerek sonuç bir hata mesajı dizesi olarak döndürülüyor.
            return $"Validation failed: {string.Join(string.Empty, arr)}";
            // Yukarıdaki kod, hata mesajlarını "Validation failed:" metniyle birleştirerek bir hata mesajı dizesi oluşturur.
            // Hata mesajları, ValidationExceptionModel nesnelerinden gelen özellik (Property) ve hata listesi (Errors) bilgileri ile biçimlendirilir.
            // Hata listesi eğer boşsa, Array.Empty<string>() kullanılarak bir boş dize döndürülür.
            // Tüm hata mesajları, string.Join ile birleştirilir ve sonuç olarak döndürülür.
        }
    }
    public class ValidationExceptionModel
    {
        //Hangi property'de hata var? (örnek: name)
        public string? Property { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
