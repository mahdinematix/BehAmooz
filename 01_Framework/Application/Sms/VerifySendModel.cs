namespace _01_Framework.Application.Sms;

public class VerifySendModel
{
    public string Mobile { get; set; }
    public string TemplateId { get; set; }
    public VerifySendParameterModel[] Parameters { get; set; }
}