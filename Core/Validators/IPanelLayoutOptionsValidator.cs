using Core.Layouts;

namespace Core.Validators;

public interface IPanelLayoutOptionsValidator
{
    ValidationResult Validate(PanelLayoutOptions options);
}
