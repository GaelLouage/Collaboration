namespace Collaboration.Dtos
{
    public class ResultDto<T>
    {
        public List<string> Errors { get; set; } = new List<string>();
        public T Result { get; set; }
    }
}
